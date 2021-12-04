using Grpc.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Channels = System.Threading.Channels;

namespace LanDataTransmitter {

    class GrpcService {

        public string Address { get; set; }
        public int Port { get; set; }

        public delegate void SuccessCallback();
        public delegate void FailedCallback(string reason);
        public delegate void FinishCallback();
        public delegate void ClientBindCallack(bool bind);
        public delegate void DisplayClientTextCallback(PushTextRequest request);
        public delegate void DisplayServerTextCallback(PullTextReply reply);

        private Server _server;
        private TransmitterImplConfig _implConfig;

        // =============
        // serve related
        // =============

        /// <summary>For Server</summary>
        public async Task Serve(SuccessCallback onSuccess, FailedCallback onFailed) {
            _implConfig = new TransmitterImplConfig();
            _server = new Server {
                Services = { Transmitter.BindService(new TransmitterImpl(_implConfig)) },
                Ports = { new ServerPort(Address, Port, ServerCredentials.Insecure) },
            };
            try {
                await Task.Delay(millisecondsDelay: 200);
                _server.Start();
                onSuccess?.Invoke();
            } catch (IOException) {
                onFailed?.Invoke("无法启动服务器，可能由于端口被占用");
            } catch (Exception ex) {
                onFailed?.Invoke(ex.Message);
            }
        }

        /// <summary>For Server</summary>
        public async Task Shutdown(SuccessCallback onSuccess) {
            Global.pullTextChannel.Writer.Complete(new Exception("连接已断开"));
            Global.pullTextExceptionChannel.Writer.Complete(new Exception("连接已断开"));
            // TODO
            await _server?.ShutdownAsync();
            onSuccess?.Invoke();
        }

        public void ForceDisconnect(SuccessCallback onSuccess) {
            Global.pullTextChannel.Writer.Complete(new Exception("连接已断开"));
            Global.pullTextExceptionChannel.Writer.Complete(new Exception("连接已断开"));
            onSuccess?.Invoke();
        }

        // ===============
        // connect related
        // ===============

        /// <summary>For Client</summary>
        public async Task Connect(SuccessCallback onSuccess, FailedCallback onFailed) {
            var channel = new Channel(Address, Port, ChannelCredentials.Insecure);
            var client = new Transmitter.TransmitterClient(channel);
            try {
                var reply = await client.PingAsync(new PingRequest { Message = "ping" });
                if (!reply.Accepted) {
                    throw new Exception("服务器已经绑定着一个客户端，无法绑定新的客户端");
                }
                Global.clientId = reply.Id;
                onSuccess?.Invoke();
            } catch (Exception ex) {
                onFailed?.Invoke(ex.Message);
            } finally {
                await channel.ShutdownAsync();
            }
        }

        /// <summary>For Client</summary>
        public async Task Disconnect(SuccessCallback onSuccess, FailedCallback onFailed) {
            var channel = new Channel(Address, Port, ChannelCredentials.Insecure);
            var client = new Transmitter.TransmitterClient(channel);
            try {
                var reply = await client.StopAsync(new StopRequest { Id = Global.clientId });
                if (!reply.Valid) {
                    throw new Exception("请求不合法，与服务器所绑定的客户端不匹配");
                }
                onSuccess?.Invoke();
            } catch (Exception ex) {
                onFailed?.Invoke(ex.Message);
            } finally {
                await channel.ShutdownAsync();
            }
        }

        // ===============
        // receive related
        // ===============

        /// <summary>For Server</summary>
        public void SetupTransmitter(ClientBindCallack onClientBind, DisplayClientTextCallback onDisplayText) {
            _implConfig.OnClientBind = onClientBind; // 服务器对客户端的绑定信息更改的回调
            _implConfig.OnDisplayText = onDisplayText; // 处理接收到来自客户端的消息的回调
        }

        /// <summary>For Client</summary>
        public void StartHandlePullText(SuccessCallback onSuccess, FailedCallback onFailed, FinishCallback onFinish, DisplayServerTextCallback onDisplayText) {
            var channel = new Channel(Address, Port, ChannelCredentials.Insecure);
            var client = new Transmitter.TransmitterClient(channel);
            try {
                var stream = client.PullText(new PullTextRequest()).ResponseStream;
                var th = new Thread(async () => {
                    while (await stream.MoveNext()) {
                        var reply = stream.Current;
                        onDisplayText?.Invoke(reply);
                    }
                    onFinish?.Invoke();
                });
                th.Start();
                onSuccess?.Invoke();
            } catch (Exception ex) {
                onFailed?.Invoke(ex.Message);
            }
        }

        // ============
        // send related
        // ============

        /// <summary>For Client</summary>
        public async Task SendTextToServer(string text, SuccessCallback onSuccess, FailedCallback onFailed) {
            var channel = new Channel(Address, Port, ChannelCredentials.Insecure);
            var client = new Transmitter.TransmitterClient(channel);
            try {
                await client.PushTextAsync(new PushTextRequest { Message = text });
                onSuccess?.Invoke();
            } catch (Exception ex) {
                onFailed?.Invoke(ex.Message);
            } finally {
                await channel.ShutdownAsync();
            }
        }

        /// <summary>For Server</summary>
        public async Task SendTextToClient(string text, SuccessCallback onSuccess, FailedCallback onFailed) {
            try {
                await Global.pullTextChannel.Writer.WriteAsync(new PullTextReply { Message = text });
                var ex = await Global.pullTextExceptionChannel.Reader.ReadAsync();
                if (ex != null) {
                    throw ex;
                }
                onSuccess?.Invoke();
            } catch (Channels.ChannelClosedException) {
                Console.WriteLine("SendTextToClient: ChannelClosedException");
                return;
            } catch (Exception ex) {
                onFailed?.Invoke(ex.Message);
            }
        }

    }

    class TransmitterImplConfig {
        public GrpcService.ClientBindCallack OnClientBind { set; get; }
        public GrpcService.DisplayClientTextCallback OnDisplayText { set; get; }
    }

    class TransmitterImpl : Transmitter.TransmitterBase {

        private readonly TransmitterImplConfig _config;

        public TransmitterImpl(TransmitterImplConfig config) {
            _config = config;
        }

        public override Task<PingReply> Ping(PingRequest request, ServerCallContext context) {
            if (Global.isBindClient) {
                return Task.FromResult(new PingReply { Accepted = false });
            }
            Global.isBindClient = true;
            Global.bindClientId = Utils.GetGlobalId();
            _config.OnClientBind?.Invoke(true);
            return Task.FromResult(new PingReply { Accepted = true, Id = Global.bindClientId });
        }

        public override Task<StopReply> Stop(StopRequest request, ServerCallContext context) {
            if (request.Id != Global.bindClientId) {
                return Task.FromResult(new StopReply { Valid = false });
            }
            Global.isBindClient = false;
            Global.bindClientId = "";
            _config.OnClientBind?.Invoke(false);
            return Task.FromResult(new StopReply { Valid = true });
        }

        public override Task<PushTextReply> PushText(PushTextRequest request, ServerCallContext context) {
            _config.OnDisplayText?.Invoke(request);
            return Task.FromResult(new PushTextReply());
        }

        public override async Task PullText(PullTextRequest request, IServerStreamWriter<PullTextReply> responseStream, ServerCallContext context) {
            if (Global.pullTextChannel == null) {
                Console.WriteLine("PullText: Global.pullTextChannel == null");
                return;
            }
            while (true) {
                try {
                    var reply = await Global.pullTextChannel.Reader.ReadAsync();
                    await responseStream.WriteAsync(reply);
                    await Global.pullTextExceptionChannel.Writer.WriteAsync(null);
                } catch (Channels.ChannelClosedException) {
                    Console.WriteLine("PullText: ChannelClosedException");
                    return;
                } catch (Exception ex) {
                    try {
                        await Global.pullTextExceptionChannel.Writer.WriteAsync(ex);
                    } catch (Channels.ChannelClosedException) { }
                    continue;
                }
            }
        }

    }
}
