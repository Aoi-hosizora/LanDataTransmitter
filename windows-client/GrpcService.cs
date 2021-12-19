using Grpc.Core;
using System;
using System.IO;
using Channels = System.Threading.Channels;
using System.Threading.Tasks;

namespace LanDataTransmitter {

    class GrpcService {

        public string Address { get; set; }
        public int Port { get; set; }

        public delegate void FinishCallback();
        public delegate void ClientBindCallback(string clientId);
        public delegate void ClientUnbindCallback(string clientId);
        public delegate void ReceivedFromClientCallback(PushTextRequest request);
        public delegate void ReceivedFromServerCallback(PullTextReply reply);

        private Server _server;
        private TransmitterImpl _serverImpl;

        // ====================
        // server serve related
        // ====================

        /// <summary>For Server</summary>
        public async Task Serve() {
            _serverImpl = new TransmitterImpl();
            _server = new Server {
                Services = { Transmitter.BindService(_serverImpl) },
                Ports = { new ServerPort(Address, Port, ServerCredentials.Insecure) },
            };
            try {
                await Task.Delay(millisecondsDelay: 100);
                _server.Start();
            } catch (IOException) {
                throw new Exception("端口可能被占用");
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>For Server</summary>
        public async Task Shutdown() {
            // TODO
            Global.PullChannel?.Complete();
            if (_server != null) {
                await _server.ShutdownAsync();
            }
        }

        /// <summary>For Server</summary>
        public async Task ForceDisconnect() {
            // TODO
            await new Task(() => {
                Global.PullChannel?.Complete();
            });
        }

        // ======================
        // client connect related
        // ======================

        /// <summary>For Client</summary>
        public async Task Connect() {
            var channel = new Channel(Address, Port, ChannelCredentials.Insecure);
            var client = new Transmitter.TransmitterClient(channel);
            try {
                var reply = await client.PingAsync(new PingRequest { Message = "ping" });
                if (!reply.Accepted) {
                    throw new Exception("服务器已经绑定着一个客户端，无法绑定新的客户端");
                }
                Global.SelfClientId = reply.Id;
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            } finally {
                await channel.ShutdownAsync();
            }
        }

        /// <summary>For Client</summary>
        public async Task Disconnect() {
            var channel = new Channel(Address, Port, ChannelCredentials.Insecure);
            var client = new Transmitter.TransmitterClient(channel);
            try {
                var reply = await client.StopAsync(new StopRequest { Id = Global.SelfClientId });
                if (!reply.Valid) {
                    throw new Exception("请求不合法，与服务器所绑定的客户端不匹配");
                }
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            } finally {
                await channel.ShutdownAsync();
            }
        }

        // ===============
        // receive related
        // ===============

        /// <summary>For Server</summary>
        public void SetupTransmitterServer(ClientBindCallback onBind, ClientUnbindCallback onUnbind, ReceivedFromClientCallback onReceived) {
            _serverImpl.OnClientBind = onBind; // 回调：服务器绑定了客户端
            _serverImpl.OnClientUnbind = onUnbind; // 回调：服务器取消绑定了客户端
            _serverImpl.OnReceivedFromClient = onReceived; // 回调：服务器收到了来自客户端的消息
        }

        /// <summary>For Client</summary>
        public void StartReceivingTextFromServer(FinishCallback onFinish, ReceivedFromServerCallback onReceived) {
            var channel = new Channel(Address, Port, ChannelCredentials.Insecure);
            var client = new Transmitter.TransmitterClient(channel);
            try {
                var stream = client.PullText(new PullTextRequest()).ResponseStream;
                Task.Run(async () => {
                    while (await stream.MoveNext()) {
                        var reply = stream.Current;
                        onReceived?.Invoke(reply); // 回调：客户端收到了来自服务器的消息
                    }
                    onFinish?.Invoke(); // 回调：来自服务器的消息已结束
                });
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        // ============
        // send related
        // ============

        /// <summary>For Server</summary>
        public async Task SendTextToClient(string text) {
            try {
                await Global.PullChannel.WriteForwardChannel(new PullTextReply { Message = text });
                var ex = await Global.PullChannel.ReadBackwardChannel();
                if (ex != null) {
                    throw ex;
                }
            } catch (Channels.ChannelClosedException) {
                Console.WriteLine("SendTextToClient: ChannelClosedException");
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>For Client</summary>
        public async Task SendTextToServer(string text) {
            var channel = new Channel(Address, Port, ChannelCredentials.Insecure);
            var client = new Transmitter.TransmitterClient(channel);
            try {
                await client.PushTextAsync(new PushTextRequest { Message = text });
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            } finally {
                await channel.ShutdownAsync();
            }
        }

    } // class GrpcService

    class BidirectionalChannel<TForward, TBackward> {
        public BidirectionalChannel(int capacity) {
            _forwardChannel = Channels.Channel.CreateBounded<TForward>(capacity);
            _backwardChannel = Channels.Channel.CreateBounded<TBackward>(capacity);
        }

        private readonly Channels.Channel<TForward> _forwardChannel;
        private readonly Channels.Channel<TBackward> _backwardChannel;

        public async Task WriteForwardChannel(TForward data) {
            await _forwardChannel.Writer.WriteAsync(data);
        }

        public async Task WriteBackwardChannel(TBackward data) {
            await _backwardChannel.Writer.WriteAsync(data);
        }

        public async Task<TForward> ReadForwardChannel() {
            return await _forwardChannel.Reader.ReadAsync();
        }

        public async Task<TBackward> ReadBackwardChannel() {
            return await _backwardChannel.Reader.ReadAsync();
        }

        public void Complete(string message = null) {
            try {
                _forwardChannel.Writer.Complete(new Exception(message ?? "连接已断开"));
                _backwardChannel.Writer.Complete(new Exception(message ?? "连接已断开"));
            } catch (InvalidOperationException) { }
        }

    } // class BidirectionalChannel

    class TransmitterImpl : Transmitter.TransmitterBase {

        public GrpcService.ClientBindCallback OnClientBind { get; set; }
        public GrpcService.ClientUnbindCallback OnClientUnbind { get; set; }
        public GrpcService.ReceivedFromClientCallback OnReceivedFromClient { get; set; }

        public override Task<PingReply> Ping(PingRequest request, ServerCallContext context) {
            if (Global.IsBindingClient) {
                return Task.FromResult(new PingReply { Accepted = false });
            }
            Global.IsBindingClient = true;
            Global.BindClientId = Utils.GetGlobalId();
            OnClientBind?.Invoke(Global.BindClientId);
            return Task.FromResult(new PingReply { Accepted = true, Id = Global.BindClientId });
        }

        public override Task<StopReply> Stop(StopRequest request, ServerCallContext context) {
            if (request.Id != Global.BindClientId) {
                return Task.FromResult(new StopReply { Valid = false });
            }
            Global.IsBindingClient = false;
            OnClientUnbind?.Invoke(Global.BindClientId);
            Global.BindClientId = "";
            return Task.FromResult(new StopReply { Valid = true });
        }

        public override Task<PushTextReply> PushText(PushTextRequest request, ServerCallContext context) {
            OnReceivedFromClient?.Invoke(request);
            return Task.FromResult(new PushTextReply());
        }

        public override async Task PullText(PullTextRequest request, IServerStreamWriter<PullTextReply> responseStream, ServerCallContext context) {
            if (Global.PullChannel == null) {
                Console.WriteLine("PullText: Global.pullTextChannel == null");
                return;
            }
            while (true) {
                try {
                    var reply = await Global.PullChannel.ReadForwardChannel();
                    await responseStream.WriteAsync(reply);
                    await Global.PullChannel.WriteBackwardChannel(null);
                } catch (Channels.ChannelClosedException) {
                    Console.WriteLine("PullText: ChannelClosedException");
                    return;
                } catch (Exception ex) {
                    try {
                        await Global.PullChannel.WriteBackwardChannel(ex);
                    } catch (Channels.ChannelClosedException) { }
                }
            }
        }

    } // class TransmitterImpl
}
