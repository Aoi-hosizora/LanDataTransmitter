import 'package:grpc/grpc.dart';
import 'package:lan_data_transmitter/model/transmitter.pbgrpc.dart';
import 'package:lan_data_transmitter/service/global.dart';
import 'package:lan_data_transmitter/util/util.dart';

class GrpcClientService {
  String address;
  int port;

  GrpcClientService(this.address, this.port);

  Tuple<ClientChannel, TransmitterClient> createClient() {
    var channel = ClientChannel(
      address,
      port: port,
      options: ChannelOptions(credentials: ChannelCredentials.insecure()),
    );
    var client = TransmitterClient(channel);
    return Tuple(channel, client);
  }

  Future<String> connect(String name) async {
    var cc = createClient();
    ConnectReply reply;
    try {
      var request = ConnectRequest()..clientName = name;
      reply = await cc.item2.connect(request);
    } catch (ex) {
      throw Exception('无法连接到服务器：$ex');
    } finally {
      await cc.item1.shutdown();
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
      reply = await cc.item2.disconnect(request);
    } catch (ex) {
      throw Exception('无法连接到服务器：$ex');
    } finally {
      await cc.item1.shutdown();
    }
    if (!reply.accepted) {
      throw Exception('当前客户端未连接到服务器');
    }
  }
}
