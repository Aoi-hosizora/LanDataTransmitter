import 'package:lan_data_transmitter/model/objects.dart';
import 'package:lan_data_transmitter/service/grpc_client_service.dart';
import 'package:lan_data_transmitter/service/grpc_server_service.dart';

enum ApplicationBehavior { asServer, asClient }
enum ApplicationState { preparing, running, stopped }

class Global {
  static ApplicationBehavior behavior = ApplicationBehavior.asServer;
  static ApplicationState state = ApplicationState.preparing;
  static MessageRepository? messages;
  static _ServerGlobal? server;
  static _ClientGlobal? client;

  static void initializeServer(GrpcServerService service) {
    Global.behavior = ApplicationBehavior.asServer;
    Global.state = ApplicationState.running;
    Global.messages = MessageRepository();
    Global.server = _ServerGlobal()
      ..service = service
      ..connectedClients = {};
  }

  static void initializeClient(GrpcClientService service, ClientObject obj) {
    Global.behavior = ApplicationBehavior.asClient;
    Global.state = ApplicationState.running;
    Global.messages = MessageRepository();
    Global.client = _ClientGlobal()
      ..service = service
      ..obj = obj;
  }
} // class Global

class _ServerGlobal {
  late GrpcServerService service;
  late Map<String, ClientObject> connectedClients;
} // class ServerGlobal

class _ClientGlobal {
  late GrpcClientService service;
  late ClientObject obj;
} // class ClientGlobal
