using System;
using System.Collections.Generic;

namespace LanDataTransmitter {

    public enum ApplicationBehavior { AsServer, AsClient }

    public static class Global {

        public static ApplicationBehavior Behavior;

        public static void InitializeServer(GrpcServerService service) {
            Behavior = ApplicationBehavior.AsServer;
            Server.Service = service;
            Server.ConnectedClients = new Dictionary<string, ClientObject>();
        }

        public static void InitializeClient(GrpcClientService service) {
            Behavior = ApplicationBehavior.AsClient;
            Client.Service = service;
        }

        public static class Server {
            public static Dictionary<string, ClientObject> ConnectedClients;
            public static GrpcServerService Service;
        }

        public static class Client {
            public static string Id;
            public static string Name;
            public static GrpcClientService Service;
        }
    }

    public class ClientObject {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime ConnectedTime { get; set; }
        public bool Polling { get; set; }
        public BidChannel<PullTextReply, Exception> Channel { get; set; }
    }
}
