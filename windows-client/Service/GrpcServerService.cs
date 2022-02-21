﻿using System;
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
                throw new Exception($"指定的监听端口被占用，详细原因：{ex.Message}");
            }
        }

        public async Task Shutdown() {
            await DisconnectAll("服务器已关闭");
            // if (_server != null) {
            //     await _server.ShutdownAsync();
            // }
            _server?.ShutdownAsync(); // <- slow
        }

        public async Task DisconnectAll(string message = null) {
            message ??= "服务器要求断开连接";
            var tasks = new List<Task>();
            Global.Server.ConnectedClients.Values.ToList().ForEach(obj => {
                tasks.Add(Task.Run(async () => {
                    var chan = obj.PullChannel;
                    try {
                        var data = new PulledDisconnectReply { Disconnect = true };
                        var reply = new PullReply { Accepted = true, Type = PulledType.Disconnect, Disconnect = data };
                        await chan.SendForward(reply);
                        var _ = await chan.ReceiveBackward(); // ignore exception
                        chan.Complete(message);
                    } catch (Exception) {
                        // ignore all exceptions
                    }
                }));
            });
            await Task.WhenAll(tasks.ToArray());
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
                var data = new PulledTextReply { MessageId = messageId, Timestamp = timestamp, Text = text.UnifyToCrlf() };
                var reply = new PullReply { Accepted = true, Type = PulledType.Text, Text = data };
                await chan.SendForward(reply); // C <- S, send // goto TransmitterImpl.Pull
                var ex = await chan.ReceiveBackward(); // from TransmitterImpl.Pull
                if (ex != null) {
                    throw ex;
                }
            } catch (ChannelClosedException) {
                // 服务器已关闭 / 服务器要求断开连接 / 客户端主动断开连接
            } catch (Exception ex) {
                throw new Exception(Utils.CheckGrpcException(ex, true));
            }
            var record = new MessageRecord(clientId, obj.Name, messageId, timestamp, text.UnifyToCrlf());
            return record; // 通过返回值通知调用方：消息成功发送至客户端
        }

        public void SetupTransmitter(ConnectStateChangedCallback onConnected, ConnectStateChangedCallback onDisconnected, MessageReceivedCallback onReceived) {
            _serverImpl.SetupCallbacks(
                onConnected: onConnected,
                onDisconnected: onDisconnected,
                onReceived: onReceived // C -> S, recv
            );
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
            var clientId = Utils.GenerateGlobalId();
            var name = request.ClientName; // can be empty
            if (name.Length > 0 && Global.Server.ConnectedClients.Any(kv => kv.Value.Name == name)) {
                return Task.FromResult(new ConnectReply { Accepted = false }); // conflict
            }
            var connectTimestamp = Utils.ToTimestamp(DateTime.Now);
            var chan = new BiChannel<PullReply, Exception>();
            var obj = new ClientObject(clientId, name, connectTimestamp, false, chan);
            _onConnected?.Invoke(obj);
            return Task.FromResult(new ConnectReply { Accepted = true, ConnectTimestamp = connectTimestamp, ClientId = clientId });
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
            var contains = Global.Server.ConnectedClients.TryGetValue(request.ClientId, out var obj);
            if (!contains) {
                return Task.FromResult(new PushTextReply { Accepted = false }); // not connect yet
            }
            var messageId = Utils.GenerateGlobalId();
            var record = new MessageRecord(obj.Id, obj.Name, messageId, request.Timestamp, request.Text.UnifyToCrlf()); // C -> S, recv
            _onReceived?.Invoke(record); // 通过回调通知调用方：成功收到来自客户端的消息
            return Task.FromResult(new PushTextReply { Accepted = true, MessageId = messageId });
        }

        public override async Task Pull(PullRequest request, IServerStreamWriter<PullReply> responseStream, ServerCallContext context) {
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
                    // TODO error???
                    await responseStream.WriteAsync(reply); // C <- S, send 
                    await chan.SendBackward(null /* ex */); // goto GrpcServerService.SendText
                } catch (ChannelClosedException) {
                    // 服务器已关闭 / 服务器要求断开连接 / 客户端主动断开连接
                    return; // close stream also
                } catch (Exception ex) {
                    try {
                        await chan.SendBackward(ex);
                    } catch (ChannelClosedException) {
                        return;
                    }
                }
            }
        }

    } // class TransmitterImpl
}
