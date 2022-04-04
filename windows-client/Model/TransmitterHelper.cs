using Google.Protobuf;

namespace LanDataTransmitter.Model {

    // Text

    public partial class PushTextRequest {
        public PushTextRequest(string clientId, TextMessage text) {
            ClientId = clientId;
            Text = text;
        }
    }

    public partial class PulledTextReply {
        public PulledTextReply(string messageId, TextMessage text) {
            MessageId = messageId;
            Text = text;
        }
    }

    public partial class TextMessage {
        public TextMessage(ulong timestamp, string text) {
            Timestamp = timestamp;
            Text = text;
        }
    }

    // File

    public partial class PushFileRequest {
        public PushFileRequest(string clientId, FileMessage file) {
            ClientId = clientId;
            File = file;
        }
    }

    public partial class PulledFileReply {
        public PulledFileReply(string messageId, FileMessage file) {
            MessageId = messageId;
            File = file;
        }
    }

    public partial class FileMessage {
        public FileMessage(ulong timestamp, string filename, int filesize, bool direct, ByteString data) {
            Timestamp = timestamp;
            Filename = filename;
            Filesize = (uint) filesize;
            Direct = direct;
            Data = data;
        }
    }

    public partial class PushFileChunksRequest {
        public PushFileChunksRequest(string clientId, string messageId, FileChunk chunk) {
            ClientId = clientId;
            MessageId = messageId;
            Chunk = chunk;
        }
    }

    public partial class PullFileChunksReply {
        public PullFileChunksReply(bool accepted, FileChunk chunk) {
            Accepted = accepted;
            Chunk = chunk;
        }
    }

    public partial class FileChunk {
        public FileChunk(int start, int length, ByteString data) {
            Start = (uint) start;
            Length = (uint) length;
            Data = data;
        }
    }
}
