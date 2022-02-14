import 'package:lan_data_transmitter/model/objects.dart';
import 'package:lan_data_transmitter/service/grpc_client_service.dart';
import 'package:lan_data_transmitter/service/grpc_server_service.dart';

enum ApplicationBehavior { server, client }
enum ApplicationState { preparing, running, stopped }

class Global {
  static ApplicationBehavior behavior = ApplicationBehavior.server;
  static ApplicationState state = ApplicationState.preparing;
  static MessageRepository? messages;
  static _ServerGlobal? server;
  static _ClientGlobal? client;

  static void initializeServer(GrpcServerService service) {
    Global.behavior = ApplicationBehavior.server;
    Global.state = ApplicationState.running;
    Global.messages = MessageRepository();
    Global.server = _ServerGlobal()
      ..service = service
      ..connectedClients = {};
  }

  static void initializeClient(GrpcClientService service, String id, String name) {
    Global.behavior = ApplicationBehavior.client;
    Global.state = ApplicationState.running;
    Global.messages = MessageRepository();
    Global.client = _ClientGlobal()
      ..service = service
      ..id = id
      ..name = name;
  }
} // class Global

class _ServerGlobal {
  late GrpcServerService service;
  late Map<String, ClientObject> connectedClients;
} // class ServerGlobal

class _ClientGlobal {
  late GrpcClientService service;
  late String id;
  late String name;
} // class ClientGlobal
