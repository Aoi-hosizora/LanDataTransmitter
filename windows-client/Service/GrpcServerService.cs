using Grpc.Core;
using GrpcChannel = Grpc.Core.Channel;
using System;
using System.IO;
using System.Linq;
using ThreadChannels = System.Threading.Channels;
using System.Threading.Tasks;
using LanDataTransmitter.Util;

namespace LanDataTransmitter.Service {

    public delegate void MessageReceivedCallback(MessageRecord request);
    public delegate void ConnectStateChangedCallback(string clientId, string clientName);

    public class GrpcServerService {

        public string Address { get; }
        public int Port { get; }

        public GrpcServerService(string address, int port) {
            Address = address;
            Port = port;
        }

        private TransmitterImpl _serverImpl;
        private Server _server;

        public async Task Serve() {
            if (!Utils.ValidIpv4Address(Address)) {
                throw new Exception($"IP 地址 \"{Address}\" 格式有误");
            }
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
            await DisconnectAll("服务器已关闭");
            if (_server != null) {
                await _server.ShutdownAsync();
            }
        }

        public async Task DisconnectAll(string message = null) {
            message ??= "服务器要求断开连接";
            foreach (var id in Global.Server.ConnectedClients.Keys) {
                var chan = Global.Server.ConnectedClients[id].Channel;
                await chan.SendForward(new PullTextReply { Accepted = true, Closing = true });
                var _ = await chan.ReceiveBackward();
                chan.Complete(message);
            }
            Global.Server.ConnectedClients.Clear();
        }

        public async Task<MessageRecord> SendText(string clientId, string text, DateTime time) {
            var ok = Global.Server.ConnectedClients.TryGetValue(clientId, out var obj);
            if (!ok) {
                throw new Exception($"服务器暂未连接到客户端 {clientId}");
            }
            var messageId = Utils.GenerateGlobalId();
            var timestamp = Utils.ToTimestamp(time);
            var chan = obj.Channel;
            try {
                var reply = new PullTextReply { Accepted = true, Closing = false, MessageId = messageId, Text = text, Timestamp = timestamp };
                await chan.SendForward(reply); // capacity=1, goto SetupTransmitter.PullText
                var ex = await chan.ReceiveBackward();
                if (ex != null) {
                    throw ex;
                }
            } catch (ThreadChannels.ChannelClosedException) {
                // 服务器已关闭 / 服务器要求断开连接 / 客户端主动断开连接
            } catch (Exception ex) {
                throw new Exception("无法连接到服务器：" + ex.Message);
            }
            var record = MessageRecord.CreateForStCMessage(clientId, obj.Name, messageId, text, timestamp);
            Global.StCMessages.Add(record); // <<<
            return record; // 通过返回值通知调用方：消息发送成功 (-> C)
        }

        public void SetupTransmitter(ConnectStateChangedCallback onConnected, ConnectStateChangedCallback onDisconnected, MessageReceivedCallback onReceived) {
            // !!!!!!
            async Task PullText(ClientObject client, IServerStreamWriter<PullTextReply> writer) {
                while (true) {
                    var chan = client.Channel;
                    try {
                        var reply = await chan.ReceiveForward();
                        await writer.WriteAsync(reply); // Note: only one write can be pending at a time
                        await chan.SendBackward(null /* ex */); // capacity=1
                    } catch (ThreadChannels.ChannelClosedException) {
                        // 服务器已关闭 / 服务器要求断开连接 / 客户端主动断开连接
                        return;
                    } catch (Exception ex) {
                        try {
                            await chan.SendBackward(ex);
                        } catch (ThreadChannels.ChannelClosedException) {
                            // 服务器已关闭 / 服务器要求断开连接 / 客户端主动断开连接
                            return;
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

    } // class GrpcServerService

    internal class TransmitterImpl : Transmitter.TransmitterBase {
        public delegate Task PullTextHandler(ClientObject client, IServerStreamWriter<PullTextReply> stream);

        private ConnectStateChangedCallback _onConnected;
        private ConnectStateChangedCallback _onDisconnected;
        private MessageReceivedCallback _onReceived;
        private PullTextHandler _pullTextHandler;

        public void Setup(ConnectStateChangedCallback onConnected, ConnectStateChangedCallback onDisconnected, MessageReceivedCallback onReceived, PullTextHandler pullTextHandler) {
            _onConnected = onConnected;
            _onDisconnected = onDisconnected;
            _onReceived = onReceived;
            _pullTextHandler = pullTextHandler;
        }

        public override Task<ConnectReply> Connect(ConnectRequest request, ServerCallContext context) {
            var id = Utils.GenerateGlobalId();
            var name = request.ClientName; // can be empty
            if (name.Length > 0 && Global.Server.ConnectedClients.Any(kv => kv.Value.Name == name)) {
                return Task.FromResult(new ConnectReply { Accepted = false }); // conflict
            }
            var obj = new ClientObject { Id = id, Name = name, ConnectedTime = DateTime.Now, Channel = new BiChannel<PullTextReply, Exception>(1) };
            Global.Server.ConnectedClients[id] = obj;
            _onConnected?.Invoke(id, name);
            return Task.FromResult(new ConnectReply { Accepted = true, ClientId = id });
        }

        public override Task<DisconnectReply> Disconnect(DisconnectRequest request, ServerCallContext context) {
            var id = request.ClientId;
            var contains = Global.Server.ConnectedClients.TryGetValue(id, out var obj);
            if (!contains) {
                return Task.FromResult(new DisconnectReply { Accepted = false }); // not connect yet
            }
            Global.Server.ConnectedClients[id].Channel.Complete("客户端主动断开连接");
            Global.Server.ConnectedClients.Remove(id);
            _onDisconnected?.Invoke(id, obj.Name);
            return Task.FromResult(new DisconnectReply { Accepted = true });
        }

        public override Task<PushTextReply> PushText(PushTextRequest request, ServerCallContext context) {
            // !!!!!!
            var contains = Global.Server.ConnectedClients.TryGetValue(request.ClientId, out var obj);
            if (!contains) {
                return Task.FromResult(new PushTextReply { Accepted = false }); // not connect yet
            }
            var text = request.Text.Trim();
            if (text.Length == 0) {
                return Task.FromResult(new PushTextReply { Accepted = false }); // empty text
            }
            var messageId = Utils.GenerateGlobalId();
            var record = MessageRecord.CreateForCtSMessage(obj.Id, obj.Name, messageId, text, request.Timestamp);
            Global.CtSMessages.Add(record); // <<<
            _onReceived?.Invoke(record); // 通过回调通知调用方：成功收到消息 (<- C)
            return Task.FromResult(new PushTextReply { Accepted = true });
        }

        public override async Task PullText(PullTextRequest request, IServerStreamWriter<PullTextReply> responseStream, ServerCallContext context) {
            var contains = Global.Server.ConnectedClients.TryGetValue(request.ClientId, out var obj);
            if (!contains || obj.Polling) {
                // not connect yet || stream is closed || pulling already
                await responseStream.WriteAsync(new PullTextReply { Accepted = false });
                return;
            }
            Global.Server.ConnectedClients[request.ClientId].Polling = true;
            await _pullTextHandler.Invoke(obj, responseStream);
        }

    } // class TransmitterImpl

}
