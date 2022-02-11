using System;
using System.Collections.Generic;
using LanDataTransmitter.Util;

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

    public class ClientObject {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FullDisplayName => Name == "" ? Id : $"{Id} ({Name})";
        public DateTime ConnectedTime { get; set; }
        public bool Pulling { get; set; }
        public BiChannel<PullReply, Exception> PullChannel { get; set; }

        public static Tuple<string, string> ExtractIdAndName(string s) {
            var sp = s.Split(' ');
            return new Tuple<string, string>(sp.Length == 0 ? "" : sp[0], sp.Length <= 1 ? "" : sp[1]);
        }
    } // class ClientObject

    public class MessageRepository {
        public List<MessageRecord> Records { get; }
        public int CtSCount { get; private set; }
        public int StCCount => (Records?.Count ?? 0) - CtSCount;

        public MessageRepository() {
            Records = new List<MessageRecord>();
        }

        public void AddCtSMessage(MessageRecord r) {
            r.IsCtS = true;
            Records.Add(r);
            CtSCount++;
        }

        public void AddStCMessage(MessageRecord r) {
            r.IsCtS = false;
            Records.Add(r);
        }
    } // class MessageRepository

    public class MessageRecord {
        // client
        public bool IsCtS { get; set; }
        public bool IsStC => !IsCtS;
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientDisplayName => ClientName == "" ? ClientId : ClientName;
        public string ClientFullDisplayName => ClientName == "" ? ClientId : $"{ClientId} ({ClientName})";

        // message
        public string MessageId { get; set; }
        public long Timestamp { get; set; }
        public string Text { get; set; }

        public static MessageRecord Create(string clientId, string clientName, string messageId, long timestamp, string text) {
            return new MessageRecord {
                IsCtS = false, // this field will be set when AddCtSMessage or AddStCMessage invoked
                ClientId = clientId, ClientName = clientName,
                MessageId = messageId, Timestamp = timestamp, Text = text
            };
        }
    } // class MessageRecord
}
