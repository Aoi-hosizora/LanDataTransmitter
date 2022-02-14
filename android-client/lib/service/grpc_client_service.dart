import 'package:grpc/grpc.dart';
import 'package:lan_data_transmitter/model/objects.dart';
import 'package:lan_data_transmitter/model/transmitter.dart';
import 'package:lan_data_transmitter/service/global.dart';
import 'package:lan_data_transmitter/service/grpc_server_service.dart';
import 'package:lan_data_transmitter/util/extensions.dart';
import 'package:lan_data_transmitter/util/util.dart' as util;

class GrpcClientService {
  String address;
  int port;

  GrpcClientService(this.address, this.port);

  util.Tuple<ClientChannel, TransmitterClient> _createClient() {
    var channel = ClientChannel(
      address,
      port: port,
      options: ChannelOptions(
        credentials: ChannelCredentials.insecure(),
        codecRegistry: CodecRegistry(codecs: [GzipCodec(), IdentityCodec()]),
      ),
    );
    var client = TransmitterClient(channel);
    return util.Tuple(channel, client);
  }

  Future<String> connect(String name) async {
    var cc = _createClient();
    ConnectReply reply;
    try {
      var request = ConnectRequest(clientName: name);
      reply = await cc.item2.connect(request); // 使服务器记录客户端信息
    } on Exception catch (ex) {
      throw Exception('无法连接到服务器：${ex.message()}');
    } finally {
      await cc.item1.shutdown();
    }
    if (!reply.accepted) {
      throw Exception('客户端指定的名称已存在');
    }
    return reply.clientId;
  }

  Future<void> disconnect() async {
    var cc = _createClient();
    DisconnectReply reply;
    try {
      var request = DisconnectRequest(clientId: Global.client!.id);
      reply = await cc.item2.disconnect(request); // 使服务器更新并删除客户端信息
    } on Exception catch (ex) {
      throw Exception('无法连接到服务器：${ex.message()}');
    } finally {
      await cc.item1.shutdown();
    }
    if (!reply.accepted) {
      throw Exception('当前客户端未连接到服务器');
    }
  }

  Future<MessageRecord> sendText(String text, DateTime time) async {
    var cc = _createClient();
    var timestamp = util.toTimestamp(time);
    PushTextReply reply;
    try {
      var request = PushTextRequest(clientId: Global.client!.id, timestamp: timestamp, text: text);
      reply = await cc.item2.pushText(request);
    } on Exception catch (ex) {
      throw Exception('无法连接到服务器：${ex.message()}');
    } finally {
      await cc.item1.shutdown();
    }
    var record = MessageRecord(clientId: Global.client!.id, clientName: Global.client!.name, messageId: reply.messageId, timestamp: timestamp, text: text); // C -> S
    return record; // 通过返回值通知调用方：消息成功发送至服务器
  }

  Future<bool> startPulling({required MessageReceivedCallback onReceived}) async {
    var cc = _createClient();
    ResponseStream<PullReply> stream;
    try {
      var request = PullRequest(clientId: Global.client!.id);
      stream = cc.item2.pull(request);
    } on Exception catch (ex) {
      throw Exception('无法连接到服务器：${ex.message()}');
    }
    // !!!!!!
    await for (var reply in stream) {
      if (!reply.accepted) {
        throw Exception('当前客户端未连接到服务器，或者当前客户端重复接收消息');
      }
      switch (reply.type) {
        case PulledType.DISCONNECT:
          await cc.item1.shutdown();
          return false; // 被动断开
        case PulledType.TEXT:
          var p = reply.text;
          var record = MessageRecord(clientId: Global.client!.id, clientName: Global.client!.name, messageId: p.messageId, timestamp: p.timestamp, text: p.text); // C <- S
          onReceived.call(record); // 通过回调通知调用方：成功收到来自服务器的消息
          break;
      }
    }
    await cc.item1.shutdown();
    return true; // 主动断开
  }
} // class GrpcClientService
