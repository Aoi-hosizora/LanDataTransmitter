import 'package:grpc/grpc.dart';
import 'package:lan_data_transmitter/model/transmitter.pbgrpc.dart';
import 'package:lan_data_transmitter/service/global.dart';
import 'package:lan_data_transmitter/util/util.dart' as util;

typedef ConnectStateChangedCallback = Function(ClientObject);

class GrpcServerService {
  String address;
  int port;
  TransmitterImpl? _serverImpl;
  Server? _server;

  GrpcServerService(this.address, this.port);

  Future serve() async {
    _serverImpl = TransmitterImpl();
    _server = Server([_serverImpl!]);
    await _server!.serve(address: address, port: port);
  }
}

class TransmitterImpl extends TransmitterServiceBase {
  ConnectStateChangedCallback? _onConnected;
  ConnectStateChangedCallback? _onDisconnected;

  void setupCallbacks(ConnectStateChangedCallback onConnected, ConnectStateChangedCallback onDisconnected) {
    _onConnected = onConnected;
    _onDisconnected = onDisconnected;
  }

  @override
  Future<ConnectReply> connect(ServiceCall call, ConnectRequest request) {
    var clientId = util.generateGlobalId();
    var name = request.clientName;
    if (name.isNotEmpty && Global.server!.connectedClients.values.any((v) => v.name == name)) {
      return Future.value(ConnectReply(accepted: false)); // conflict
    }
    var obj = ClientObject(clientId, name, DateTime.now(), false);
    _onConnected?.call(obj);
    return Future.value(ConnectReply(accepted: true, clientId: clientId));
  }

  @override
  Future<DisconnectReply> disconnect(ServiceCall call, DisconnectRequest request) {
    var obj = Global.server!.connectedClients[request.clientId];
    if (obj == null) {
      return Future.value(DisconnectReply(accepted: false)); // not connect yet
    }
    _onDisconnected?.call(obj);
    return Future.value(DisconnectReply(accepted: true));
  }

  @override
  Future<PushTextReply> pushText(ServiceCall call, PushTextRequest request) {
    throw UnimplementedError();
  }

  @override
  Stream<PullReply> pull(ServiceCall call, PullRequest request) {
    throw UnimplementedError();
  }
}
