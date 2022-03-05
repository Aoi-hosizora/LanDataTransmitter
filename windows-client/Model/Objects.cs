using System;
using System.Collections.Generic;
using LanDataTransmitter.Util;

namespace LanDataTransmitter.Model {

    // ==============
    // client related
    // ==============

    public class ClientObject {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FullDisplayName => Name == "" ? Id : $"{Id} ({Name})";
        public ulong ConnectedTimestamp { get; set; }
        public bool Pulling { get; private set; } // ignore when AsClient
        public BiChannel<PullReply, Exception> PullChannel { get; private set; } // ignore when AsClient

        public static Tuple<string, string> ExtractIdAndName(string s) {
            var sp = s.Split(' ');
            return new Tuple<string, string>(sp.Length == 0 ? "" : sp[0], sp.Length <= 1 ? "" : sp[1]);
        }

        public ClientObject(string id, string name, ulong connectTimestamp) {
            Id = id;
            Name = name;
            ConnectedTimestamp = connectTimestamp;
            Pulling = false;
            PullChannel = null;
        }

        public void StartPulling(BiChannel<PullReply, Exception> pullChannel) {
            Pulling = true;
            PullChannel = pullChannel;
        }
    } // class ClientObject

    // ===============
    // message related
    // ===============

    public partial class MessageRecord {
        // client
        public bool IsCts { get; set; }
        public bool IsStc => !IsCts;
        public string ClientId { get; }
        public string ClientName { get; }
        public string ClientShortDisplayName => ClientName == "" ? ClientId : ClientName;
        public string ClientFullDisplayName => ClientName == "" ? ClientId : $"{ClientId} ({ClientName})";

        // message
        public string MessageId { get; }

        public MessageRecord(string clientId, string clientName, string messageId) {
            IsCts = false; // this field will be set when AddCtsMessage or AddStcMessage invoked
            ClientId = clientId;
            ClientName = clientName;
            MessageId = messageId;
        }
    } // partial class MessageRecord

    public partial class MessageRecord {
        // message
        public enum MessageType { TEXT, FILE }
        public MessageType Type { get; private set; }
        public TextMessage Text { get; private set; }
        public FileMessage File { get; private set; }

        public MessageRecord WithText(TextMessage text) {
            Type = MessageType.TEXT;
            Text = text;
            File = null;
            return this;
        }

        public MessageRecord WithFile(FileMessage file) {
            Type = MessageType.FILE;
            Text = null;
            File = file;
            return this;
        }

        public class TextMessage {
            public ulong CreatedTimestamp { get; }
            // public ulong UpdatedTimestamp { get; private set; }
            public string Text { get; private set; }

            public TextMessage(ulong timestamp, string text) {
                CreatedTimestamp = timestamp;
                Text = text;
            }

            // public void Update(ulong timestamp, string text) {
            //     UpdatedTimestamp = timestamp;
            //     Text = text;
            // }
        } // class TextMessage

        public class FileMessage {
            public ulong CreatedTimestamp { get; }
            public string Filename { get; }
            public ulong Filesize { get; }
            public bool Receiving { get; private set; }
            public string LocalFilePath { get; private set; }

            public FileMessage(ulong timestamp, string filename, ulong filesize) {
                CreatedTimestamp = timestamp;
                Filename = filename;
                Filesize = filesize;
                Receiving = false; // TODO
                LocalFilePath = ""; // TODO
            }
        } // class FileMessage
    } // partial class MessageRecord

    public class MessageRepository {
        public List<MessageRecord> Records { get; }
        public int CtsCount { get; private set; }
        public int StcCount => (Records?.Count ?? 0) - CtsCount;

        public MessageRepository() {
            Records = new List<MessageRecord>();
        }

        public void AddCtsMessage(MessageRecord r) {
            r.IsCts = true;
            Records.Add(r);
            CtsCount++;
        }

        public void AddStcMessage(MessageRecord r) {
            r.IsCts = false;
            Records.Add(r);
        }
    } // class MessageRepository
}
