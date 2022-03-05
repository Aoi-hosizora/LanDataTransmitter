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
        public FileMessage(ulong timestamp, string filename, ulong filesize, bool useChunk, ByteString data) {
            Timestamp = timestamp;
            Filename = filename;
            Filesize = filesize;
            UseChunk = useChunk;
            Data = data;
        }
    }
}
