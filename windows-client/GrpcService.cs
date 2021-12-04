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

        public delegate void ServedCallback();
        public delegate void ServeFailedCallback(string reason);
        public delegate void DisplayTextCallback(PushTextRequest request);
        public delegate void ConnectedCallback();
        public delegate void ConnectFailedCallback(string reason);

        private Server _server;
        private TransmitterImplConfig _implConfig;

        public void Serve(ServedCallback onSuccess, ServeFailedCallback onFailed) {
            _implConfig = new TransmitterImplConfig();
            _server = new Server {
                Services = { Transmitter.BindService(new TransmitterImpl(_implConfig)) },
                Ports = { new ServerPort(Address, Port, ServerCredentials.Insecure) },
            };
            try {
                _server.Start();
                onSuccess?.Invoke();
            } catch (Exception ex) {
                onFailed?.Invoke(ex.Message);
            }
        }

        public void SetupTransmitter(DisplayTextCallback onDisplay, Channels.Channel<PullTextReply> pullTextChannel) {
            _implConfig.OnDisplay = onDisplay;
            _implConfig.PullTextChannel = pullTextChannel;
        }

        public bool Shutdown() {
            _server?.ShutdownAsync().Wait();
            return true;
        }

        public void Connect(ConnectedCallback onSuccess, ConnectFailedCallback onFailed) {
            var channel = new Channel(Address, Port, ChannelCredentials.Insecure);
            var client = new Transmitter.TransmitterClient(channel);
            try {
                var reply = client.Ping(new PingRequest { Message = "ping" });
                if (reply.Message != "ping") {
                    throw new Exception("got a non-ping response, maybe the server has always bind a client.");
                }
                onSuccess?.Invoke();
            } catch (Exception ex) {
                onFailed?.Invoke(ex.Message);
            } finally {
                channel.ShutdownAsync().Wait();
            }
        }

        public void Disconnect() {
            var channel = new Channel(Address, Port, ChannelCredentials.Insecure);
            var client = new Transmitter.TransmitterClient(channel);
            try {
                client.Stop(new StopRequest { });
            } catch (Exception) {
                // ...
            } finally {
                channel.ShutdownAsync().Wait();
            }
        }

    }

    class TransmitterImplConfig {
        public GrpcService.DisplayTextCallback OnDisplay { set; get; }
        public Channels.Channel<PullTextReply> PullTextChannel { set; get; }
    }

    class TransmitterImpl : Transmitter.TransmitterBase {

        private readonly TransmitterImplConfig _config;

        public TransmitterImpl(TransmitterImplConfig config) {
            _config = config;
        }

        public override Task<PingReply> Ping(PingRequest request, ServerCallContext context) {
            if (Global.clientBind) {
                return Task.FromResult(new PingReply { Message = "" });
            }
            Global.clientBind = true;
            return Task.FromResult(new PingReply { Message = request.Message });
        }

        public override Task<StopReply> Stop(StopRequest request, ServerCallContext context) {
            Global.clientBind = false;
            return Task.FromResult(new StopReply());
        }

        public override Task<PushTextReply> PushText(PushTextRequest request, ServerCallContext context) {
            _config.OnDisplay?.Invoke(request);
            return Task.FromResult(new PushTextReply());
        }

        public override async Task PullText(PullTextRequest request, IServerStreamWriter<PullTextReply> responseStream, ServerCallContext context) {
            if (_config.PullTextChannel == null) {
                return;
            }
            while (true) {
                var reply = await _config.PullTextChannel.Reader.ReadAsync();
                await responseStream.WriteAsync(reply);
            }
        }

    }
}
