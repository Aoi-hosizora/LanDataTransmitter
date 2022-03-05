using System;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcChannel = Grpc.Core.Channel;
using LanDataTransmitter.Model;
using LanDataTransmitter.Util;

namespace LanDataTransmitter.Service {

    public class GrpcClientService {

        public string Address { get; }
        public int Port { get; }

        public GrpcClientService(string address, int port) {
            Address = address;
            Port = port;
        }

        private async Task<T> RequestServer<T>(Func<GrpcChannel, Transmitter.TransmitterClient, Task<T>> callback, bool autoShutdown = true) {
            var channel = new GrpcChannel(Address, Port, ChannelCredentials.Insecure);
            var client = new Transmitter.TransmitterClient(channel); // not connect yet
            try {
                return await callback.Invoke(channel, client); // use callback to request server and get reply
            } catch (Exception ex) {
                throw new Exception(Utils.FormatGrpcException(ex, false));
            } finally {
                if (autoShutdown) {
                    await channel.ShutdownAsync();
                }
            }
        }

        private ClientObject CurrentClient => Global.Client.Obj;

        public async Task<ClientObject> Connect(string name) {
            if (!Utils.ValidIpv4Address(Address)) {
                throw new Exception($"IP 地址 \"{Address}\" 格式有误");
            }
            var request = new ConnectRequest { ClientName = name /* may be empty */ };
            var reply = await RequestServer(async (_, client) => await client.ConnectAsync(request));
            if (!reply.Accepted) {
                throw new Exception("客户端指定的名称已存在");
            }
            return new ClientObject(reply.ClientId, name, Utils.ToTimestamp(DateTime.Now));
        }

        public async Task Disconnect() {
            var request = new DisconnectRequest { ClientId = CurrentClient.Id };
            var reply = await RequestServer(async (_, client) => await client.DisconnectAsync(request));
            if (!reply.Accepted) {
                throw new Exception("当前客户端未连接到服务器");
            }
        }

        public async Task<MessageRecord> SendText(string text, DateTime time) {
            var timestamp = Utils.ToTimestamp(time);
            var request = new PushTextRequest(CurrentClient.Id, new TextMessage(timestamp, text.UnifyToCrlf()));
            var reply = await RequestServer(async (_, client) => await client.PushTextAsync(request)); // C -> S, send
            if (!reply.Accepted) {
                throw new Exception("当前客户端未连接到服务器");
            }
            var record = new MessageRecord(CurrentClient.Id, CurrentClient.Name, reply.MessageId).WithText(
                new MessageRecord.TextMessage(timestamp, text.UnifyToCrlf()));
            return record;
        }

        public async Task<MessageRecord> SendFile(string filename, ulong filesize, DateTime time) {
            var timestamp = Utils.ToTimestamp(time);
            var request = new PushFileRequest(CurrentClient.Id, new FileMessage(timestamp, filename, filesize, false, null)); // TODO
            var reply = await RequestServer(async (_, client) => await client.PushFileAsync(request)); // C -> S, send 
            if (!reply.Accepted) {
                throw new Exception("当前客户端未连接到服务器");
            }
            var record = new MessageRecord(CurrentClient.Id, CurrentClient.Name, reply.MessageId).WithFile(
                new MessageRecord.FileMessage(timestamp, filename, filesize)); // TODO
            return record;
        }

        public async Task<bool> StartPulling(MessageReceivedCallback onReceived) {
            var request = new PullRequest { ClientId = CurrentClient.Id };
            var (channel, stream) = await RequestServer((ch, client) =>
                Task.FromResult(new Tuple<GrpcChannel, IAsyncStreamReader<PullReply>>(ch, client.Pull(request).ResponseStream)), false);
            while (await stream.MoveNext()) {
                var reply = stream.Current;
                if (!reply.Accepted) { // pulling is rejected
                    await channel.ShutdownAsync();
                    throw new Exception("当前客户端未连接到服务器，或者当前客户端重复接收消息");
                }
                // !!!
                switch (reply.Type) {
                    case PulledType.Disconnect: {
                        await channel.ShutdownAsync();
                        return false; // 被动断开
                    }
                    case PulledType.Text: {
                        var pulled = reply.Text; // C <- S, recv
                        var record = new MessageRecord(CurrentClient.Id, CurrentClient.Name, pulled.MessageId).WithText(
                            new MessageRecord.TextMessage(pulled.Text.Timestamp, pulled.Text.Text.UnifyToCrlf()));
                        onReceived?.Invoke(record);
                        break;
                    }
                    case PulledType.File: {
                        var pulled = reply.File; // C <- S, recv
                        var record = new MessageRecord(CurrentClient.Id, CurrentClient.Name, pulled.MessageId).WithFile(
                            new MessageRecord.FileMessage(pulled.File.Timestamp, pulled.File.Filename, pulled.File.Filesize));
                        // TODO
                        onReceived?.Invoke(record);
                        break;
                    }
                }
            }
            await channel.ShutdownAsync();
            return true; // 主动断开
        }

    } // class GrpcClientService
}
