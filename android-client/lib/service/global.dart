import 'package:lan_data_transmitter/service/grpc_client_service.dart';
import 'package:lan_data_transmitter/service/grpc_server_service.dart';

enum ApplicationBehavior { server, client }
enum ApplicationState { preparing, running, stopped }

class Global {
  static ApplicationBehavior behavior = ApplicationBehavior.server;
  static ApplicationState state = ApplicationState.preparing;
  static ServerGlobal? server;
  static ClientGlobal? client;

  static void initializeServer(GrpcServerService service) {
    Global.behavior = ApplicationBehavior.server;
    Global.state = ApplicationState.running;
    Global.server = ServerGlobal(service);
  }

  static void initializeClient(GrpcClientService service, String id, String name) {
    Global.behavior = ApplicationBehavior.client;
    Global.state = ApplicationState.running;
    Global.client = ClientGlobal(service, id, name);
  }
}

class ServerGlobal {
  GrpcServerService service;
  Map<String, ClientObject> connectedClients;

  ServerGlobal(this.service) : connectedClients = {};
}

class ClientGlobal {
  GrpcClientService service;
  String id;
  String name;

  ClientGlobal(this.service, this.id, this.name);
}

class ClientObject {
  String id;
  String name;
  DateTime connectedTime;
  bool pulling;

  ClientObject(this.id, this.name, this.connectedTime, this.pulling);

  String get fullDisplayName => name == '' ? id : '$id ($name)';
}
