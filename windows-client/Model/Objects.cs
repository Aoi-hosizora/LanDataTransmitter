using System;
using System.Collections.Generic;
using LanDataTransmitter.Util;

namespace LanDataTransmitter.Model {

    public class ClientObject {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FullDisplayName => Name == "" ? Id : $"{Id} ({Name})";
        public ulong ConnectedTimestamp { get; set; }
        public bool Pulling { get; set; } // ignore when AsClient
        public BiChannel<PullReply, Exception> PullChannel { get; set; } // ignore when AsClient

        public static Tuple<string, string> ExtractIdAndName(string s) {
            var sp = s.Split(' ');
            return new Tuple<string, string>(sp.Length == 0 ? "" : sp[0], sp.Length <= 1 ? "" : sp[1]);
        }

        public ClientObject(string id, string name, ulong connectTimestamp, bool pulling = false, BiChannel<PullReply, Exception> pullChannel = null) {
            Id = id;
            Name = name;
            ConnectedTimestamp = connectTimestamp;
            Pulling = pulling;
            PullChannel = pullChannel;
        }
    } // class ClientObject

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
        public ulong Timestamp { get; set; }
        public string Text { get; set; }

        public MessageRecord(string clientId, string clientName, string messageId, ulong timestamp, string text) {
            IsCtS = false; // this field will be set when AddCtSMessage or AddStCMessage invoked
            ClientId = clientId;
            ClientName = clientName;
            MessageId = messageId;
            Timestamp = timestamp;
            Text = text;
        }
    } // class MessageRecord


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
}
