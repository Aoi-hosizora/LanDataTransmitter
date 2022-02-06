using Grpc.Core;
using GrpcChannel = Grpc.Core.Channel;
using System;
using System.IO;
using System.Linq;
using ThreadChannels = System.Threading.Channels;
using System.Threading.Tasks;
using LanDataTransmitter.Util;

namespace LanDataTransmitter.Service {

    public delegate void ReceivedFromServerCallback(PullTextReply reply);
    public delegate void ReceivedFromClientCallback(PushTextRequest request);
    public delegate void ConnectStateChangedCallback(string clientId);

    public class GrpcClientService {
        public string Address { get; }
        public int Port { get; }

        public GrpcClientService(string address, int port) {
            Address = address;
            Port = port;
        }

        public async Task Connect(string name) {
            var channel = new GrpcChannel(Address, Port, ChannelCredentials.Insecure);
            var client = new Transmitter.TransmitterClient(channel);
            ConnectReply reply;
            try {
                reply = await client.ConnectAsync(new ConnectRequest { Name = name });
            } catch (Exception ex) {
                throw new Exception("无法连接到服务器：" + ex.Message);
            } finally {
                await channel.ShutdownAsync();
            }
            if (!reply.Accepted) {
                throw new Exception("客户端指定的名称已存在");
            }
            Global.Client.Id = reply.Id;
            Global.Client.Name = reply.Name;
        }

        public async Task Disconnect() {
            var channel = new GrpcChannel(Address, Port, ChannelCredentials.Insecure);
            var client = new Transmitter.TransmitterClient(channel);
            DisconnectReply reply;
            try {
                reply = await client.DisconnectAsync(new DisconnectRequest { Id = Global.Client.Id });
            } catch (Exception ex) {
                throw new Exception("无法连接到服务器：" + ex.Message);
            } finally {
                await channel.ShutdownAsync();
            }
            if (!reply.Accepted) {
                throw new Exception("当前客户端暂未连接到服务器，或者连接已经断开");
            }
        }

        public async Task SendText(string text, DateTime time) {
            var channel = new GrpcChannel(Address, Port, ChannelCredentials.Insecure);
            var client = new Transmitter.TransmitterClient(channel);
            PushTextReply reply;
            try {
                var request = new PushTextRequest { Id = Global.Client.Id, Text = text, Timestamp = new DateTimeOffset(time).ToUnixTimeSeconds() };
                reply = await client.PushTextAsync(request);
            } catch (Exception ex) {
                throw new Exception("无法连接到服务器：" + ex.Message);
            } finally {
                await channel.ShutdownAsync();
            }
            if (!reply.Accepted) {
                throw new Exception("当前客户端暂未连接到服务器，或者连接已经断开");
            }
        }

        public async Task<bool> StartReceivingText(ReceivedFromServerCallback onReceived) {
            var channel = new GrpcChannel(Address, Port, ChannelCredentials.Insecure);
            var client = new Transmitter.TransmitterClient(channel);
            IAsyncStreamReader<PullTextReply> stream;
            try {
                stream = client.PullText(new PullTextRequest { Id = Global.Client.Id }).ResponseStream;
            } catch (Exception ex) {
                throw new Exception("无法连接到服务器：" + ex.Message);
            }
            // <<<<<
            while (await stream.MoveNext()) {
                var reply = stream.Current;
                if (!reply.Accepted) {
                    throw new Exception("当前客户端暂未连接到服务器，或者连接已经断开，或者当前客户端重复接收消息");
                }
                if (reply.Closing) {
                    return false; // 被动断开
                }
                onReceived?.Invoke(reply); // 回调：客户端收到来自服务器的消息
            }
            return true; // 主动断开
        }
    } // GrpcClientService

    public class GrpcServerService {
        public string Address { get; }
        public int Port { get; }

        public GrpcServerService(string address, int port) {
            Address = address;
            Port = port;
        }

        private Server _server;
        private TransmitterImpl _serverImpl;

        public async Task Serve() {
            _serverImpl = new TransmitterImpl();
            _server = new Server {
                Services = { Transmitter.BindService(_serverImpl) },
                Ports = { new ServerPort(Address, Port, ServerCredentials.Insecure) },
            };
            try {
                await Task.Run(() => _server.Start());
            } catch (IOException) {
                _serverImpl = null;
                _server = null;
                throw new Exception("所监听的端口被占用");
            }
        }

        public async Task Shutdown() {
            foreach (var id in Global.Server.ConnectedClients.Keys) {
                var channel = Global.Server.ConnectedClients[id].Channel;
                await channel.SendForward(new PullTextReply { Accepted = true, Closing = true });
                var _ = await channel.ReceiveBackward();
                channel.Complete("服务器已关闭");
            }
            Global.Server.ConnectedClients.Clear();
            if (_server != null) {
                await _server.ShutdownAsync();
            }
        }

        public async Task ForceDisconnectAll() {
            foreach (var id in Global.Server.ConnectedClients.Keys) {
                var channel = Global.Server.ConnectedClients[id].Channel;
                await channel.SendForward(new PullTextReply { Accepted = true, Closing = true });
                var _ = await channel.ReceiveBackward();
                channel.Complete("服务器要求断开连接");
            }
            Global.Server.ConnectedClients.Clear();
        }

        public async Task SendText(string id, string text, DateTime time) {
            var ok = Global.Server.ConnectedClients.TryGetValue(id, out var obj);
            if (!ok) {
                throw new Exception($"服务器暂未连接到客户端 {id}");
            }
            var chan = obj.Channel;
            try {
                var reply = new PullTextReply {
                    Accepted = true, Closing = false,
                    Text = text, Timestamp = new DateTimeOffset(time).ToUnixTimeSeconds()
                };
                await chan.SendForward(reply); // cap 1
                var ex = await chan.ReceiveBackward();
                if (ex != null) {
                    throw ex;
                }
            } catch (ThreadChannels.ChannelClosedException) {
                // 服务器已关闭 / 服务器要求断开连接 / 客户端主动断开连接
                // throw new NotImplementedException();
            } catch (Exception ex) {
                throw new Exception("无法连接到服务器：" + ex.Message);
            }
        }

        public void SetupTransmitter(ConnectStateChangedCallback onConnected, ConnectStateChangedCallback onDisconnected, ReceivedFromClientCallback onReceived) {
            async Task PullText(ClientObject client, IServerStreamWriter<PullTextReply> writer) {
                while (true) {
                    var chan = client.Channel;
                    try {
                        var reply = await chan.ReceiveForward();
                        await writer.WriteAsync(reply); // Note: only one write can be pending at a time
                        await chan.SendBackward(null /* ex */); // cap 1
                    } catch (ThreadChannels.ChannelClosedException) {
                        // 服务器已关闭 / 服务器要求断开连接 / 客户端主动断开连接
                        return;
                        // throw new NotImplementedException();
                    } catch (Exception ex) {
                        try {
                            await chan.SendBackward(ex);
                        } catch (ThreadChannels.ChannelClosedException) {
                            // 服务器已关闭 / 服务器要求断开连接 / 客户端主动断开连接
                            return;
                            // throw new NotImplementedException();
                        }
                    }
                }
            }
            _serverImpl.Setup(
                onConnected, // 回调：服务器连接了新客户端
                onDisconnected, // 回调：服务器取消连接客户端
                onReceived, // 回调：服务器收到了来自客户端的消息
                PullText // 用于将 S 的信息发送给指定 C
            );
        }
    } // GrpcServerService

    internal class TransmitterImpl : Transmitter.TransmitterBase {
        public delegate Task PullTextHandler(ClientObject client, IServerStreamWriter<PullTextReply> stream);

        private ConnectStateChangedCallback _onConnected;
        private ConnectStateChangedCallback _onDisconnected;
        private ReceivedFromClientCallback _onReceived;
        private PullTextHandler _pullTextHandler;

        public void Setup(ConnectStateChangedCallback onConnected, ConnectStateChangedCallback onDisconnected, ReceivedFromClientCallback onReceived, PullTextHandler pullTextHandler) {
            _onConnected = onConnected;
            _onDisconnected = onDisconnected;
            _onReceived = onReceived;
            _pullTextHandler = pullTextHandler;
        }

        public override Task<ConnectReply> Connect(ConnectRequest request, ServerCallContext context) {
            var id = Utils.GenerateGlobalId();
            var name = request.Name.Trim(); // can be empty
            if (name.Length > 0 && Global.Server.ConnectedClients.Any(kv => kv.Value.Name == name)) {
                return Task.FromResult(new ConnectReply { Accepted = false }); // conflict
            }
            var obj = new ClientObject { Id = id, Name = name, Channel = new BiChannel<PullTextReply, Exception>(1), ConnectedTime = DateTime.Now };
            Global.Server.ConnectedClients[id] = obj;
            _onConnected?.Invoke(id);
            return Task.FromResult(new ConnectReply { Accepted = true, Id = id, Name = name });
        }

        public override Task<DisconnectReply> Disconnect(DisconnectRequest request, ServerCallContext context) {
            if (!Global.Server.ConnectedClients.ContainsKey(request.Id)) {
                return Task.FromResult(new DisconnectReply { Accepted = false }); // not connect yet
            }
            Global.Server.ConnectedClients[request.Id].Channel.Complete("客户端主动断开连接");
            Global.Server.ConnectedClients.Remove(request.Id);
            _onDisconnected?.Invoke(request.Id);
            return Task.FromResult(new DisconnectReply { Accepted = true });
        }

        public override Task<PushTextReply> PushText(PushTextRequest request, ServerCallContext context) {
            if (!Global.Server.ConnectedClients.ContainsKey(request.Id)) {
                return Task.FromResult(new PushTextReply { Accepted = false }); // not connect yet
            }
            request.Text = request.Text.Trim();
            if (request.Text.Length == 0) {
                return Task.FromResult(new PushTextReply { Accepted = false }); // empty text
            }
            _onReceived?.Invoke(request);
            return Task.FromResult(new PushTextReply { Accepted = true });
        }

        public override async Task PullText(PullTextRequest request, IServerStreamWriter<PullTextReply> responseStream, ServerCallContext context) {
            if (_pullTextHandler == null) {
                return;
            }
            var ok = Global.Server.ConnectedClients.TryGetValue(request.Id, out var obj);
            if (!ok || obj.Polling) {
                // not connect yet || stream is closed || pulling already
                await responseStream.WriteAsync(new PullTextReply { Accepted = false });
                return;
            }
            Global.Server.ConnectedClients[request.Id].Polling = true;
            await _pullTextHandler.Invoke(obj, responseStream);
        }
    } // class TransmitterImpl

}
