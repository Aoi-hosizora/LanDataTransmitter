using Grpc.Core;
using GrpcChannel = Grpc.Core.Channel;
using System;
using System.IO;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace LanDataTransmitter {

    public delegate Task FinishCallback(bool alsoDisconnect);
    public delegate void ClientBindCallback(string clientId);
    public delegate void ClientUnbindCallback(string clientId);
    public delegate void ReceivedFromClientCallback(PushTextRequest request);
    public delegate void ReceivedFromServerCallback(PullTextReply reply);

    class GrpcService {

        public string Address { get; set; }
        public int Port { get; set; }

        private Server _server;
        private TransmitterImpl _serverImpl;

        // =========================
        // serve and connect related
        // =========================

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

        /// <summary>For Client</summary>
        public async Task Connect() {
            var channel = new GrpcChannel(Address, Port, ChannelCredentials.Insecure);
            var client = new Transmitter.TransmitterClient(channel);
            try {
                var reply = await client.ConnectAsync(new ConnectRequest { Name = "" });
                // if (!reply.Accepted) {
                //     throw new Exception("服务器已经绑定着一个客户端，无法绑定新的客户端");
                // }
                Global.SelfClientId = reply.Id;
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            } finally {
                await channel.ShutdownAsync();
            }
        }

        // ===============================
        // shutdown and disconnect related
        // ===============================

        /// <summary>For Server</summary>
        public async Task Shutdown() {
            foreach (var ch in Global.PullChannels.Keys) {
                await Global.PullChannels[ch].Complete("连接已断开");
            }
            if (_server != null) {
                await _server.ShutdownAsync();
            }
        }

        /// <summary>For Server</summary>
        public async Task ForceDisconnectAll() {
            // TODO
            foreach (var ch in Global.PullChannels.Keys) {
                await Global.PullChannels[ch].WriteForwardChannel(new PullTextReply { Closing = true, Message = "" });
                await Global.PullChannels[ch].Complete("连接已断开");
            }
        }

        /// <summary>For Client</summary>
        public async Task Disconnect() {
            // TODO
            var channel = new GrpcChannel(Address, Port, ChannelCredentials.Insecure);
            var client = new Transmitter.TransmitterClient(channel);
            try {
                var reply = await client.DisconnectAsync(new DisconnectRequest { Id = Global.SelfClientId });
                if (!reply.Accepted) {
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
            _serverImpl.Setup(
                pullTextHandler: async (request, writer) => {
                    while (true) {
                        // TODO
                        var channel = Global.PullChannels[request.Id];
                        try {
                            var reply = await channel.ReadForwardChannel();
                            await writer.WriteAsync(reply);
                            await channel.WriteBackwardChannel(null);
                        } catch (ChannelClosedException) {
                            // TODO
                            Console.WriteLine("pullTextHandler: ChannelClosedException");
                            return;
                        } catch (Exception ex) {
                            try {
                                await channel.WriteBackwardChannel(ex);
                            } catch (ChannelClosedException) { }
                        }
                    }
                },
                onClientBind: onBind, // 回调：服务器绑定了客户端
                onClientUnbind: onUnbind, // 回调：服务器取消绑定了客户端
                onReceivedFromClient: onReceived // 回调：服务器收到了来自客户端的消息
            );
        }

        /// <summary>For Client</summary>
        public void StartReceivingTextFromServer(FinishCallback onFinish, ReceivedFromServerCallback onReceived) {
            var channel = new GrpcChannel(Address, Port, ChannelCredentials.Insecure);
            var client = new Transmitter.TransmitterClient(channel);
            try {
                var stream = client.PullText(new PullTextRequest { Id = Global.SelfClientId }).ResponseStream;
                Task.Run(async () => {
                    // TODO
                    bool needDisconnect = false;
                    while (await stream.MoveNext()) {
                        var reply = stream.Current;
                        if (reply.Closing) {
                            needDisconnect = true;
                            break;
                        }
                        onReceived?.Invoke(reply); // 回调：客户端收到了来自服务器的消息
                    }
                    if (onFinish != null) {
                        await onFinish.Invoke(needDisconnect); // 回调：来自服务器的消息已结束
                    }
                });
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        // ============
        // send related
        // ============

        /// <summary>For Server</summary>
        public async Task SendTextToClient(string id, string text) {
            var channel = Global.PullChannels[id];
            try {
                await channel.WriteForwardChannel(new PullTextReply { Closing = false, Message = text });
                var ex = await channel.ReadBackwardChannel();
                if (ex != null) {
                    throw ex;
                }
            } catch (ChannelClosedException) {
                await Task.Run(() => { });
                Console.WriteLine("SendTextToClient: ChannelClosedException");
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>For Client</summary>
        public async Task SendTextToServer(string text) {
            var channel = new GrpcChannel(Address, Port, ChannelCredentials.Insecure);
            var client = new Transmitter.TransmitterClient(channel);
            try {
                await client.PushTextAsync(new PushTextRequest { Id = Global.SelfClientId, Message = text });
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            } finally {
                await channel.ShutdownAsync();
            }
        }

    } // class GrpcService

    class BidirectionalChannel<TForward, TBackward> {
        public BidirectionalChannel(int capacity) {
            _forwardChannel = System.Threading.Channels.Channel.CreateBounded<TForward>(capacity);
            _backwardChannel = System.Threading.Channels.Channel.CreateBounded<TBackward>(capacity);
        }

        private readonly Channel<TForward> _forwardChannel;
        private readonly Channel<TBackward> _backwardChannel;

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

        public async Task Complete(string message = null) {
            await Task.Run(() => {
                try {
                    _forwardChannel.Writer.Complete(new Exception(message ?? "channel is completed"));
                    _backwardChannel.Writer.Complete(new Exception(message ?? "channel is completed"));
                } catch (InvalidOperationException) { }
            });
        }

    } // class BidirectionalChannel

    class TransmitterImpl : Transmitter.TransmitterBase {

        private ClientBindCallback _onClientBind;
        private ClientUnbindCallback _onClientUnbind;
        private ReceivedFromClientCallback _onReceivedFromClient;

        public delegate Task PullTextHandler(PullTextRequest request, IServerStreamWriter<PullTextReply> stream);
        private PullTextHandler _pullTextHandler;

        public void Setup(ClientBindCallback onClientBind, ClientUnbindCallback onClientUnbind, ReceivedFromClientCallback onReceivedFromClient, PullTextHandler pullTextHandler) {
            _onClientBind = onClientBind;
            _onClientUnbind = onClientUnbind;
            _onReceivedFromClient = onReceivedFromClient;
            _pullTextHandler = pullTextHandler;
        }

        public override Task<ConnectReply> Connect(ConnectRequest request, ServerCallContext context) {
            var id = Utils.GetGlobalId();
            Global.BindClients.Add(id);
            Global.PullChannels[id] = new BidirectionalChannel<PullTextReply, Exception>(1);
            _onClientBind?.Invoke(id);
            return Task.FromResult(new ConnectReply { Accepted = true, Id = id });
        }

        public override async Task<DisconnectReply> Disconnect(DisconnectRequest request, ServerCallContext context) {
            var id = request.Id.Trim();
            if (!Global.BindClients.Contains(id)) {
                return new DisconnectReply { Accepted = false };
            }
            Global.BindClients.Remove(id);
            await Global.PullChannels[id].Complete();
            _onClientUnbind?.Invoke(id);
            return new DisconnectReply { Accepted = true };
        }

        public override Task<PushTextReply> PushText(PushTextRequest request, ServerCallContext context) {
            if (!Global.BindClients.Contains(request.Id) || request.Message.Trim().Length == 0) {
                return Task.FromResult(new PushTextReply { Accepted = false });
            }
            _onReceivedFromClient?.Invoke(request);
            return Task.FromResult(new PushTextReply { Accepted = true });
        }

        public override async Task PullText(PullTextRequest request, IServerStreamWriter<PullTextReply> responseStream, ServerCallContext context) {
            if (_pullTextHandler == null || !Global.BindClients.Contains(request.Id)) {
                // stream is closed
                return;
            }
            await _pullTextHandler.Invoke(request, responseStream);
        }

    } // class TransmitterImpl
}
