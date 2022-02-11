using System;
using ThreadChannels = System.Threading.Channels;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcChannel = Grpc.Core.Channel;
using LanDataTransmitter.Util;

namespace LanDataTransmitter.Service {

    public class GrpcClientService {

        public string Address { get; }
        public int Port { get; }

        public GrpcClientService(string address, int port) {
            Address = address;
            Port = port;
        }

        private Tuple<GrpcChannel, Transmitter.TransmitterClient> CreateClient() {
            var channel = new GrpcChannel(Address, Port, ChannelCredentials.Insecure);
            var client = new Transmitter.TransmitterClient(channel); // <- here will not connect to server
            return new Tuple<Channel, Transmitter.TransmitterClient>(channel, client);
        }

        public async Task<string> Connect(string name) {
            if (!Utils.ValidIpv4Address(Address)) {
                throw new Exception($"IP 地址 \"{Address}\" 格式有误");
            }
            var (channel, client) = CreateClient();
            ConnectReply reply;
            try {
                var request = new ConnectRequest { ClientName = name /* may be empty */ };
                reply = await client.ConnectAsync(request); // 使服务器记录客户端信息
            } catch (Exception ex) {
                throw new Exception("无法连接到服务器：" + ex.Message);
            } finally {
                await channel.ShutdownAsync();
            }
            if (!reply.Accepted) {
                throw new Exception("客户端指定的名称已存在");
            }
            return reply.ClientId;
        }

        public async Task Disconnect() {
            var (channel, client) = CreateClient();
            DisconnectReply reply;
            try {
                var request = new DisconnectRequest { ClientId = Global.Client.Id };
                reply = await client.DisconnectAsync(request); // 使服务器更新并删除客户端信息
            } catch (Exception ex) {
                throw new Exception("无法连接到服务器：" + ex.Message);
            } finally {
                await channel.ShutdownAsync();
            }
            if (!reply.Accepted) {
                throw new Exception("当前客户端未连接到服务器");
            }
        }

        public async Task<MessageRecord> SendText(string text, DateTime time) {
            var (channel, client) = CreateClient();
            var timestamp = Utils.ToTimestamp(time);
            PushTextReply reply;
            try {
                var request = new PushTextRequest { ClientId = Global.Client.Id, Timestamp = timestamp, Text = text };
                reply = await client.PushTextAsync(request);
            } catch (Exception ex) {
                throw new Exception("无法连接到服务器：" + ex.Message);
            } finally {
                await channel.ShutdownAsync();
            }
            if (!reply.Accepted) {
                throw new Exception("当前客户端未连接到服务器");
            }
            var record = MessageRecord.Create(Global.Client.Id, Global.Client.Name, reply.MessageId, timestamp, text); // C -> S
            return record; // 通过返回值通知调用方：消息成功发送至服务器
        }

        public async Task<bool> StartPulling(MessageReceivedCallback onReceived) {
            var (channel, client) = CreateClient();
            IAsyncStreamReader<PullReply> stream;
            try {
                stream = client.Pull(new PullRequest { ClientId = Global.Client.Id }).ResponseStream;
            } catch (Exception ex) {
                throw new Exception("无法连接到服务器：" + ex.Message);
            }
            // !!!!!!
            while (await stream.MoveNext()) {
                var reply = stream.Current;
                if (!reply.Accepted) {
                    throw new Exception("当前客户端未连接到服务器，或者当前客户端重复接收消息");
                }
                switch (reply.Type) {
                    case PulledType.Disconnect:
                        await channel.ShutdownAsync();
                        return false; // 被动断开
                    case PulledType.Text:
                        var pulled = reply.Text;
                        var record = MessageRecord.Create(Global.Client.Id, Global.Client.Name, pulled.MessageId, pulled.Timestamp, pulled.Text); // S -> C
                        onReceived?.Invoke(record); // 通过回调通知调用方：成功收到来自服务器的消息
                        break;
                }
            }
            await channel.ShutdownAsync();
            return true; // 主动断开
        }

    } // class GrpcClientService
}
