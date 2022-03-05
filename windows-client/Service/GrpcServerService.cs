using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcChannel = Grpc.Core.Channel;
using LanDataTransmitter.Model;
using LanDataTransmitter.Util;

namespace LanDataTransmitter.Service {

    public delegate void ConnectStateChangedCallback(ClientObject client);
    public delegate void MessageReceivedCallback(MessageRecord request);

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
            } catch (Exception ex) {
                _serverImpl = null;
                _server = null;
                throw new Exception($"指定的监听端口被占用 ({ex.Message})");
            }
        }

        public async Task Shutdown() {
            await DisconnectAll("服务器已关闭");
            if (_server != null) {
                await _server.ShutdownAsync().WithTimeout(TimeSpan.FromSeconds(2)); // shutdown may be slow
            }
        }

        public async Task DisconnectAll(string message = null) {
            message ??= "服务器要求断开连接";
            var tasks = new List<Task>();
            Global.Server.ConnectedClients.Values.Where(obj => obj.PullChannel != null).ToList().ForEach(obj => {
                tasks.Add(Task.Run(async () => {
                    var reply = new PullReply { Accepted = true, Type = PulledType.Disconnect };
                    try {
                        await obj.PullChannel.SendForward(reply);
                        // await obj.PullChannel.ReceiveBackward();
                        obj.PullChannel.Complete(message);
                    } catch (Exception) {
                        // ignore all exceptions
                    }
                }));
            });
            await Task.WhenAll(tasks.ToArray()).WithTimeout(TimeSpan.FromSeconds(3));
        }

        public void SetupTransmitter(ConnectStateChangedCallback onConnected, ConnectStateChangedCallback onDisconnected, MessageReceivedCallback onReceived) {
            _serverImpl.SetupCallbacks(onConnected: onConnected, onDisconnected: onDisconnected, onReceived: onReceived);
        }

        public async Task<MessageRecord> SendText(string clientId, string text, DateTime time) {
            var ok = Global.Server.ConnectedClients.TryGetValue(clientId, out var obj);
            if (!ok) {
                throw new Exception($"服务器暂未连接到客户端 {clientId}");
            }
            var (messageId, timestamp) = (Utils.GenerateGlobalId(), Utils.ToTimestamp(time));
            var reply = new PullReply {
                Accepted = true, Type = PulledType.Text,
                Text = new PulledTextReply(messageId, new TextMessage(timestamp, text.UnifyToCrlf())),
            };
            await _serverImpl.SendToClient(obj, reply); // C <- S, send
            var record = new MessageRecord(clientId, obj.Name, messageId).WithText(
                new MessageRecord.TextMessage(timestamp, text.UnifyToCrlf()));
            return record;
        }

        public async Task<MessageRecord> SendFile(string clientId, string filename, ulong filesize, DateTime time) {
            var ok = Global.Server.ConnectedClients.TryGetValue(clientId, out var obj);
            if (!ok) {
                throw new Exception($"服务器暂未连接到客户端 {clientId}");
            }
            var (messageId, timestamp) = (Utils.GenerateGlobalId(), Utils.ToTimestamp(time));
            var reply = new PullReply {
                Accepted = true, Type = PulledType.File,
                File = new PulledFileReply(messageId, new FileMessage(timestamp, filename, filesize, false, null)) // TODO
            };
            await _serverImpl.SendToClient(obj, reply); // C <- S, send
            var record = new MessageRecord(clientId, obj.Name, messageId).WithFile(
                new MessageRecord.FileMessage(timestamp, filename, filesize)); // TODO
            return record;
        }

    } // class GrpcServerService

    internal class TransmitterImpl : Transmitter.TransmitterBase {

        private ConnectStateChangedCallback _onConnected;
        private ConnectStateChangedCallback _onDisconnected;
        private MessageReceivedCallback _onReceived;

        public void SetupCallbacks(ConnectStateChangedCallback onConnected, ConnectStateChangedCallback onDisconnected, MessageReceivedCallback onReceived) {
            _onConnected = onConnected;
            _onDisconnected = onDisconnected;
            _onReceived = onReceived;
        }

        public override Task<ConnectReply> Connect(ConnectRequest request, ServerCallContext context) {
            var name = request.ClientName; // may be empty
            if (name.Length > 0 && Global.Server.ConnectedClients.Any(kv => kv.Value.Name == name)) {
                return Task.FromResult(new ConnectReply { Accepted = false }); // name conflict
            }
            var clientId = Utils.GenerateGlobalId();
            var obj = new ClientObject(clientId, name, Utils.ToTimestamp(DateTime.Now));
            _onConnected?.Invoke(obj);
            return Task.FromResult(new ConnectReply { Accepted = true, ClientId = clientId });
        }

        public override Task<DisconnectReply> Disconnect(DisconnectRequest request, ServerCallContext context) {
            var contains = Global.Server.ConnectedClients.TryGetValue(request.ClientId, out var obj);
            if (!contains) {
                return Task.FromResult(new DisconnectReply { Accepted = false }); // not connect yet
            }
            obj.PullChannel?.Complete("客户端主动断开连接");
            _onDisconnected?.Invoke(obj);
            return Task.FromResult(new DisconnectReply { Accepted = true });
        }

        public async Task SendToClient(ClientObject clientObject, PullReply reply) {
            var chan = clientObject.PullChannel;
            if (chan == null) {
                return;
            }
            Exception ex;
            try {
                await chan.SendForward(reply); // #1
                ex = await chan.ReceiveBackward(); // #4
            } catch (ChannelClosedException) {
                // 服务器已关闭 / 服务器要求断开连接 / 客户端主动断开连接
                return; // ignore this kind of exceptions
            }
            if (ex != null) {
                throw new Exception(Utils.FormatGrpcException(ex, true));
            }
        }

        public override async Task Pull(PullRequest request, IServerStreamWriter<PullReply> responseStream, ServerCallContext context) {
            var contains = Global.Server.ConnectedClients.TryGetValue(request.ClientId, out var obj);
            if (!contains || obj.Pulling) {
                await responseStream.WriteAsync(new PullReply { Accepted = false }); // reject pulling
                return;
            }
            var chan = new BiChannel<PullReply, Exception>();
            obj.StartPulling(chan);
            while (true) {
                try {
                    var reply = await chan.ReceiveForward(); // #2
                    await responseStream.WriteAsync(reply); // C <- S, send
                    await chan.SendBackward(null /* ex */); // #3
                } catch (ChannelClosedException) {
                    // 服务器已关闭 / 服务器要求断开连接 / 客户端主动断开连接
                    return; // also close stream 
                } catch (Exception ex) {
                    try { // Exception comes from WriteAsync
                        await chan.SendBackward(ex);
                    } catch (ChannelClosedException) {
                        return;
                    }
                }
            }
        }

        public override Task<PushTextReply> PushText(PushTextRequest request, ServerCallContext context) {
            var contains = Global.Server.ConnectedClients.TryGetValue(request.ClientId, out var obj);
            if (!contains) {
                return Task.FromResult(new PushTextReply { Accepted = false }); // not connect yet
            }
            var messageId = Utils.GenerateGlobalId();
            var record = new MessageRecord(obj.Id, obj.Name, messageId).WithText(
                new MessageRecord.TextMessage(request.Text.Timestamp, request.Text.Text.UnifyToCrlf())); // C -> S, recv
            _onReceived?.Invoke(record);
            return Task.FromResult(new PushTextReply { Accepted = true, MessageId = messageId });
        }

        public override Task<PushFileReply> PushFile(PushFileRequest request, ServerCallContext context) {
            var contains = Global.Server.ConnectedClients.TryGetValue(request.ClientId, out var obj);
            if (!contains) {
                return Task.FromResult(new PushFileReply { Accepted = false }); // not connect yet
            }
            var messageId = Utils.GenerateGlobalId();
            var record = new MessageRecord(obj.Id, obj.Name, messageId).WithFile(
                new MessageRecord.FileMessage(request.File.Timestamp, request.File.Filename, request.File.Filesize)); // C -> S, recv
            // TODO
            _onReceived?.Invoke(record);
            return Task.FromResult(new PushFileReply { Accepted = true, MessageId = messageId });
        }

    } // class TransmitterImpl
}
