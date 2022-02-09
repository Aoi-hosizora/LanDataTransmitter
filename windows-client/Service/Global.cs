using System;
using System.Collections.Generic;
using LanDataTransmitter.Util;

namespace LanDataTransmitter.Service {

    public enum ApplicationBehavior { AsServer, AsClient }
    public enum ApplicationState { Preparing, Running, Stopped }

    public static class Global {

        public static ApplicationBehavior Behavior;
        public static ApplicationState State = ApplicationState.Preparing;
        public static List<MessageRecord> CtSMessages;
        public static List<MessageRecord> StCMessages;

        public static class Server {
            public static GrpcServerService Service;
            public static Dictionary<string, ClientObject> ConnectedClients;
        }

        public static class Client {
            public static string Id;
            public static string Name;
            public static GrpcClientService Service;
        }

        public static void InitializeServer(GrpcServerService service) {
            Behavior = ApplicationBehavior.AsServer;
            State = ApplicationState.Running;
            CtSMessages = new List<MessageRecord>();
            StCMessages = new List<MessageRecord>();
            Server.Service = service;
            Server.ConnectedClients = new Dictionary<string, ClientObject>();
        }

        public static void InitializeClient(string id, string name, GrpcClientService service) {
            Behavior = ApplicationBehavior.AsClient;
            State = ApplicationState.Running;
            CtSMessages = new List<MessageRecord>();
            StCMessages = new List<MessageRecord>();
            Client.Id = id;
            Client.Name = name;
            Client.Service = service;
        }
    }

    public class MessageRecord {
        public string Id { get; set; }
        public bool ClientToServer { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string Text { get; set; }
        public long Timestamp { get; set; }
        public string ClientNameOrId => ClientName != "" ? ClientName : ClientId;

        public static MessageRecord CreateForCtSMessage(string clientId, string clientName, string messageId, string text, long timestamp) {
            return new MessageRecord {
                ClientToServer = true, ClientId = clientId, ClientName = clientName,
                Id = messageId, Text = text, Timestamp = timestamp
            };
        }

        public static MessageRecord CreateForStCMessage(string clientId, string clientName, string messageId, string text, long timestamp) {
            return new MessageRecord {
                ClientToServer = false, ClientId = clientId, ClientName = clientName,
                Id = messageId, Text = text, Timestamp = timestamp
            };
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
