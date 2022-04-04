using System;
using System.IO;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using GrpcChannel = Grpc.Core.Channel;
using LanDataTransmitter.Model;
using LanDataTransmitter.Util;
using Microsoft.VisualBasic;

namespace LanDataTransmitter.Service {

    public class GrpcClientService {

        public string Address { get; }
        public int Port { get; }

        public GrpcClientService(string address, int port) {
            Address = address;
            Port = port;
        }

        public async Task<T> RequestServer<T>(Func<GrpcChannel, Transmitter.TransmitterClient, Task<T>> callback, bool autoShutdown = true) {
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
            var record = new MessageRecord(true, CurrentClient.Id, CurrentClient.Name, reply.MessageId).WithText(
                new MessageRecord.TextMessage(timestamp, text.UnifyToCrlf()));
            return record;
        }
        
        // https://docs.microsoft.com/en-us/aspnet/core/grpc/client?view=aspnetcore-6.0

        public async Task SendFile(string f, DateTime time, Action<MessageRecord, bool, double> callback) {
            var (fi, err) = Utils.CheckSentFile(f);
            if (!string.IsNullOrWhiteSpace(err)) {
                throw new Exception(err);
            }
            var (filepath, filename, filesize) = (fi.FullName, fi.Name, (int) fi.Length);
            var (buf, direct, bs) = (new byte[512 * 1024], filesize <= 512 * 1024, ByteString.Empty);
            if (direct) { // (0B, 512KB]
                using var file = fi.OpenRead();
                bs = await buf.ReadFileToByteString(file, 0);
            }
            var timestamp = Utils.ToTimestamp(time);
            var request = new PushFileRequest(CurrentClient.Id, new FileMessage(timestamp, filename, filesize, direct, bs /* not empty if direct */));
            var reply = await RequestServer(async (_, client) => await client.PushFileAsync(request)); // C -> S, send
            if (!reply.Accepted) {
                throw new Exception("当前客户端未连接到服务器");
            }
            var record = new MessageRecord(true, CurrentClient.Id, CurrentClient.Name, reply.MessageId).WithFile(
                new MessageRecord.FileMessage(timestamp, filename, filesize, filepath, direct == false));
            if (direct) {
                callback(record, true, 1.0);
            } else {
                // (512KB, 512MB]
                callback(record, false, 0.0);
                var _ = Task.Run(async () => {
                    var chunksReply = await RequestServer(async (_, client) => {
                        return await SendChunksToServer(client, fi, buf, record, callback);
                    });
                    if (!chunksReply.Accepted) {
                        // TODO
                    }
                });
            }
        }

        public static async Task<PushFileChunksReply> SendChunksToServer(
            Transmitter.TransmitterClient client, FileInfo fi, byte[] buf,
            MessageRecord record, Action<MessageRecord, bool, double> callback
        ) {
            using var call = client.PushFileChunks(); // TODO exception
            var requestStream = call.RequestStream;
            using (var file = fi.OpenRead()) {
                var sentBytes = 0;
                while (true) {
                    var bs = await buf.ReadFileToByteString(file, 0);
                    if (bs.IsEmpty) {
                        break;
                    }
                    var chunkRequest = new PushFileChunksRequest(record.ClientId, record.MessageId, new FileChunk(sentBytes, bs.Length, bs));
                    await requestStream.WriteAsync(chunkRequest); // TODO exception
                    sentBytes += bs.Length;
                    callback(record, false, (double) sentBytes / record.File.Filesize);
                }
            }
            await requestStream.CompleteAsync();
            var chunksReply = await call.ResponseAsync; // TODO exception
            record.File.FinishProcessing();
            callback(record, true, 1.0);
            return chunksReply;
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
                        var (pulled, text) = (reply.PulledText, reply.PulledText.Text); // C <- S, recv
                        var record = new MessageRecord(false, CurrentClient.Id, CurrentClient.Name, pulled.MessageId).WithText(
                            new MessageRecord.TextMessage(text.Timestamp, text.Text.UnifyToCrlf()));
                        onReceived?.Invoke(record);
                        break;
                    }
                    case PulledType.File: {
                        var (pulled, file) = (reply.PulledFile, reply.PulledFile.File); // C <- S, recv
                        var filepath = Utils.GenerateFilepath(file.Filename);
                        var record = new MessageRecord(false, CurrentClient.Id, CurrentClient.Name, pulled.MessageId).WithFile(
                            new MessageRecord.FileMessage(file.Timestamp, file.Filename, (int) file.Filesize, filepath, file.Direct == false));
                        if (file.Direct) {
                            using var f = File.OpenWrite(filepath);
                            await file.Data.WriteByteStringToFile(f);
                            onReceived?.Invoke(record);
                            break;
                        }
                        var _ = Task.Run(async () => {
                            var chunksRequest = new PullFileChunksRequest { ClientId = CurrentClient.Id, MessageId = pulled.MessageId };
                            var (chunksChannel, chunksStream) = await RequestServer((ch, client) => // TODO
                                Task.FromResult(new Tuple<GrpcChannel, IAsyncStreamReader<PullFileChunksReply>>(ch, client.PullFileChunks(chunksRequest).ResponseStream)), false);
                            using var f = File.OpenWrite(filepath);
                            var accepted = true;
                            while (await chunksStream.MoveNext()) {
                                var chunksReply = chunksStream.Current;
                                if (!chunksReply.Accepted) {
                                    accepted = false;
                                    break;
                                }

                                await chunksReply.Chunk.Data.WriteByteStringToFile(f);
                            }
                            if (accepted) {
                                record.File.FinishProcessing();
                                onReceived?.Invoke(record);
                            }
                            await chunksChannel.ShutdownAsync();
                        });
                        break;
                    }
                }
            }
            await channel.ShutdownAsync();
            return true; // 主动断开
        }

    } // class GrpcClientService
}
