import 'package:grpc/grpc.dart';
import 'package:lan_data_transmitter/model/transmitter.pbgrpc.dart';

class TransmitterImpl extends TransmitterServiceBase {
  @override
  Future<ConnectReply> connect(ServiceCall call, ConnectRequest request) async {
    return ConnectReply()..clientId = 'Hello, ${request.clientName}';
  }

  @override
  Future<DisconnectReply> disconnect(ServiceCall call, DisconnectRequest request) {
    throw UnimplementedError();
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
