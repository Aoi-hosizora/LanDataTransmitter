using System;
using System.Collections.Generic;
using LanDataTransmitter.Util;

namespace LanDataTransmitter.Service {

    public enum ApplicationBehavior { AsServer, AsClient }

    public static class Global {

        public static ApplicationBehavior Behavior;
        public static List<MessageRecord> MessagesFromClient;
        public static List<MessageRecord> MessagesFromServer;
        public static bool Stopped;

        public static class Server {
            public static GrpcServerService Service;
            public static Dictionary<string, ClientObject> ConnectedClients;
            public static int ConnectedClientsCount => ConnectedClients?.Count ?? 0;
        }

        public static class Client {
            public static string Id;
            public static string Name;
            public static GrpcClientService Service;
        }

        public static void InitializeServer(GrpcServerService service) {
            Behavior = ApplicationBehavior.AsServer;
            MessagesFromClient = new List<MessageRecord>();
            MessagesFromServer = new List<MessageRecord>();
            Stopped = false;
            Server.Service = service;
            Server.ConnectedClients = new Dictionary<string, ClientObject>();
        }

        public static void InitializeClient(string id, string name, GrpcClientService service) {
            Behavior = ApplicationBehavior.AsClient;
            MessagesFromClient = new List<MessageRecord>();
            MessagesFromServer = new List<MessageRecord>();
            Stopped = false;
            Client.Id = id;
            Client.Name = name;
            Client.Service = service;
        }
    }

    public class MessageRecord {
        public string Id { get; set; }
        public bool FromServer { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string Text { get; set; }
        public long Timestamp { get; set; }

        public static MessageRecord CreateForMessageFromClient(string messageId, string text, long timestamp) {
            return new MessageRecord { Id = messageId, FromServer = false, ClientId = Global.Client.Id, ClientName = Global.Client.Name, Text = text, Timestamp = timestamp };
        }

        public static MessageRecord CreateForMessageFromServer(string messageId, string text, long timestamp) {
            return new MessageRecord { Id = messageId, FromServer = true, Text = text, Timestamp = timestamp };
        }
    }

    public class ClientObject {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime ConnectedTime { get; set; }
        public bool Polling { get; set; }
        public BiChannel<PullTextReply, Exception> Channel { get; set; }
    }
}
