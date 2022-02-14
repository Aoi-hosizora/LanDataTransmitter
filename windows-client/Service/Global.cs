using System.Collections.Generic;
using LanDataTransmitter.Model;

namespace LanDataTransmitter.Service {

    public enum ApplicationBehavior { AsServer, AsClient }

    public enum ApplicationState { Preparing, Running, Stopped }

    public static class Global {

        public static ApplicationBehavior Behavior;
        public static ApplicationState State;
        public static MessageRepository Messages;

        public static class Server {
            public static GrpcServerService Service;
            public static Dictionary<string, ClientObject> ConnectedClients;
        }

        public static class Client {
            public static GrpcClientService Service;
            public static string Id;
            public static string Name;
        }

        public static void InitializeServer(GrpcServerService service) {
            Behavior = ApplicationBehavior.AsServer;
            State = ApplicationState.Running;
            Messages = new MessageRepository();
            Server.Service = service;
            Server.ConnectedClients = new Dictionary<string, ClientObject>();
        }

        public static void InitializeClient(string id, string name, GrpcClientService service) {
            Behavior = ApplicationBehavior.AsClient;
            State = ApplicationState.Running;
            Messages = new MessageRepository();
            Client.Service = service;
            Client.Id = id;
            Client.Name = name;
        }
    } // static class Global
}
