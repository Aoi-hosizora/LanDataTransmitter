using System;
using System.IO;
using System.Linq;
using ThreadChannels = System.Threading.Channels;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcChannel = Grpc.Core.Channel;
using LanDataTransmitter.Util;

namespace LanDataTransmitter.Service {

    public delegate void MessageReceivedCallback(MessageRecord request);
    public delegate void ConnectStateChangedCallback(ClientObject client);

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
                var chan = Global.Server.ConnectedClients[id].PullChannel;
                var data = new PulledDisconnectReply { Disconnect = true };
                await chan.SendForward(new PullReply { Accepted = true, Type = PulledType.Disconnect, Disconnect = data });
                var _ = await chan.ReceiveBackward();
                chan.Complete(message);
            }
        }

        public async Task<MessageRecord> SendText(string clientId, string text, DateTime time) {
            var ok = Global.Server.ConnectedClients.TryGetValue(clientId, out var obj);
            if (!ok) {
                throw new Exception($"服务器暂未连接到客户端 {clientId}");
            }
            var messageId = Utils.GenerateGlobalId();
            var timestamp = Utils.ToTimestamp(time);
            var chan = obj.PullChannel;
            try {
                // !!!!!!
                var data = new PulledTextReply { MessageId = messageId, Timestamp = timestamp, Text = text };
                var reply = new PullReply { Accepted = true, Type = PulledType.Text, Text = data };
                await chan.SendForward(reply); // goto TransmitterImpl.Pull
                var ex = await chan.ReceiveBackward(); // from TransmitterImpl.Pull
                if (ex != null) {
                    throw ex;
                }
            } catch (ThreadChannels.ChannelClosedException) {
                // 服务器已关闭 / 服务器要求断开连接 / 客户端主动断开连接
            } catch (Exception ex) {
                throw new Exception("无法连接到服务器：" + ex.Message);
            }
            var record = MessageRecord.Create(clientId, obj.Name, messageId, timestamp, text); // S -> C
            return record; // 通过返回值通知调用方：消息成功发送至客户端
        }

        public void SetupTransmitter(ConnectStateChangedCallback onConnected, ConnectStateChangedCallback onDisconnected, MessageReceivedCallback onReceived) {
            _serverImpl.Setup(
                onConnected: onConnected, // 回调：服务器连接了新客户端
                onDisconnected: onDisconnected, // 回调：服务器取消连接客户端
                onReceived: onReceived // 回调：服务器收到了来自客户端的消息
            );
        }

    } // class GrpcServerService

    internal class TransmitterImpl : Transmitter.TransmitterBase {

        private ConnectStateChangedCallback _onConnected;
        private ConnectStateChangedCallback _onDisconnected;
        private MessageReceivedCallback _onReceived;

        public void Setup(ConnectStateChangedCallback onConnected, ConnectStateChangedCallback onDisconnected, MessageReceivedCallback onReceived) {
            _onConnected = onConnected;
            _onDisconnected = onDisconnected;
            _onReceived = onReceived;
        }

        public override Task<ConnectReply> Connect(ConnectRequest request, ServerCallContext context) {
            var clientId = Utils.GenerateGlobalId();
            var name = request.ClientName; // can be empty
            if (name.Length > 0 && Global.Server.ConnectedClients.Any(kv => kv.Value.Name == name)) {
                return Task.FromResult(new ConnectReply { Accepted = false }); // conflict
            }
            var chan = new BiChannel<PullReply, Exception>(1);
            var obj = new ClientObject { Id = clientId, Name = name, ConnectedTime = DateTime.Now, Pulling = false, PullChannel = chan };
            _onConnected?.Invoke(obj);
            return Task.FromResult(new ConnectReply { Accepted = true, ClientId = clientId });
        }

        public override Task<DisconnectReply> Disconnect(DisconnectRequest request, ServerCallContext context) {
            var contains = Global.Server.ConnectedClients.TryGetValue(request.ClientId, out var obj);
            if (!contains) {
                return Task.FromResult(new DisconnectReply { Accepted = false }); // not connect yet
            }
            obj.PullChannel.Complete("客户端主动断开连接");
            _onDisconnected?.Invoke(obj);
            return Task.FromResult(new DisconnectReply { Accepted = true });
        }

        public override Task<PushTextReply> PushText(PushTextRequest request, ServerCallContext context) {
            // C -> S
            var contains = Global.Server.ConnectedClients.TryGetValue(request.ClientId, out var obj);
            if (!contains) {
                return Task.FromResult(new PushTextReply { Accepted = false }); // not connect yet
            }
            var messageId = Utils.GenerateGlobalId();
            var record = MessageRecord.Create(obj.Id, obj.Name, messageId, request.Timestamp, request.Text); // C -> S
            _onReceived?.Invoke(record); // 通过回调通知调用方：成功收到来自客户端的消息
            return Task.FromResult(new PushTextReply { Accepted = true, MessageId = messageId });
        }

        public override async Task Pull(PullRequest request, IServerStreamWriter<PullReply> responseStream, ServerCallContext context) {
            // C <- S
            var contains = Global.Server.ConnectedClients.TryGetValue(request.ClientId, out var obj);
            if (!contains || obj.Pulling) {
                await responseStream.WriteAsync(new PullReply { Accepted = false });
                return;
            }
            obj.Pulling = true;
            var chan = obj.PullChannel;
            // !!!!!!
            while (true) {
                // WriteAsync: Only one write can be pending at a time
                try {
                    var reply = await chan.ReceiveForward(); // from GrpcServerService.SendText
                    await responseStream.WriteAsync(reply);
                    await chan.SendBackward(null /* ex */); // goto GrpcServerService.SendText
                } catch (ThreadChannels.ChannelClosedException) {
                    // 服务器已关闭 / 服务器要求断开连接 / 客户端主动断开连接
                    return; // close stream also
                } catch (Exception ex) {
                    try {
                        await chan.SendBackward(ex);
                    } catch (ThreadChannels.ChannelClosedException) {
                        return;
                    }
                }
            }
        }

    } // class TransmitterImpl
}
