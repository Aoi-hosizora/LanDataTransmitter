using System;
using System.Collections.Generic;
using LanDataTransmitter.Util;

namespace LanDataTransmitter.Model {

    // ==============
    // client related
    // ==============

    public class ClientObject {
        public string Id { get; }
        public string Name { get; }
        public string FullDisplayName => Name == "" ? Id : $"{Id} ({Name})";
        public ulong ConnectedTimestamp { get; }
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
        public bool FromMe { get; }
        public string ClientId { get; }
        public string ClientName { get; }
        public string ClientShortDisplayName => ClientName == "" ? ClientId : ClientName;
        public string ClientFullDisplayName => ClientName == "" ? ClientId : $"{ClientId} ({ClientName})";

        // message
        public string MessageId { get; }

        public MessageRecord(bool fromMe, string clientId, string clientName, string messageId) {
            FromMe = fromMe;
            ClientId = clientId;
            ClientName = clientName;
            MessageId = messageId;
        }
    } // partial class MessageRecord

    public partial class MessageRecord {
        // message
        public enum MessageType { Text, File }
        public MessageType Type { get; private set; }
        public TextMessage Text { get; private set; }
        public FileMessage File { get; private set; }

        public MessageRecord WithText(TextMessage text) {
            Type = MessageType.Text;
            Text = text;
            File = null;
            return this;
        }

        public MessageRecord WithFile(FileMessage file) {
            Type = MessageType.File;
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
            public int Filesize { get; }
            public string Filepath { get; }
            public bool Processing { get; private set; }

            public FileMessage(ulong timestamp, string filename, int filesize, string filepath, bool processing) {
                CreatedTimestamp = timestamp;
                Filename = filename;
                Filesize = filesize;
                Filepath = filepath;
                Processing = processing;
            }

            public void FinishProcessing() {
                Processing = false;
            }
        } // class FileMessage
    } // partial class MessageRecord

    public class MessageRepository {
        public List<MessageRecord> Records { get; }
        public int SentCount { get; private set; }
        public int ReceivedCount => (Records?.Count ?? 0) - SentCount;

        public MessageRepository() {
            Records = new List<MessageRecord>();
        }

        public void AddMessage(MessageRecord r) {
            Records.Add(r);
            if (r.FromMe) {
                SentCount++;
            }
        }
    } // class MessageRepository
}
