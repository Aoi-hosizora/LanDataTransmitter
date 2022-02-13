import 'package:grpc/grpc.dart';
import 'package:lan_data_transmitter/model/transmitter.pbgrpc.dart';
import 'package:lan_data_transmitter/service/global.dart';

class ChannelClientTuple {
  ClientChannel channel;
  TransmitterClient client;

  ChannelClientTuple(this.channel, this.client);
}

class GrpcClientService {
  String address;
  int port;

  GrpcClientService(this.address, this.port);

  ChannelClientTuple createClient() {
    var channel = ClientChannel(
      address,
      port: port,
      options: ChannelOptions(credentials: ChannelCredentials.insecure()),
    );
    var client = TransmitterClient(channel);
    return ChannelClientTuple(channel, client);
  }

  Future<String> connect(String name) async {
    var cc = createClient();
    ConnectReply reply;
    try {
      var request = ConnectRequest()..clientName = name;
      reply = await cc.client.connect(request);
    } catch (ex) {
      throw Exception('无法连接到服务器：$ex');
    } finally {
      await cc.channel.shutdown();
    }
    if (!reply.accepted) {
      throw Exception('客户端指定的名称已存在');
    }
    return reply.clientId;
  }

  Future disconnect() async {
    var cc = createClient();
    DisconnectReply reply;
    try {
      var request = DisconnectRequest()..clientId = Global.client!.id;
      reply = await cc.client.disconnect(request);
    } catch (ex) {
      throw Exception('无法连接到服务器：$ex');
    } finally {
      await cc.channel.shutdown();
    }
    if (!reply.accepted) {
      throw Exception('当前客户端未连接到服务器');
    }
  }
}
