using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace LanDataTransmitter.Util {

    public class BiChannel<TForward, TBackward> {
        private readonly Channel<TForward> _forwardChannel;
        private readonly Channel<TBackward> _backwardChannel;

        public BiChannel(int capacity) {
            _forwardChannel = Channel.CreateBounded<TForward>(capacity);
            _backwardChannel = Channel.CreateBounded<TBackward>(capacity);
        }

        public async Task SendForward(TForward data) {
            await _forwardChannel.Writer.WriteAsync(data);
        }

        public async Task SendBackward(TBackward data) {
            await _backwardChannel.Writer.WriteAsync(data);
        }

        public async Task<TForward> ReceiveForward() {
            return await _forwardChannel.Reader.ReadAsync();
        }

        public async Task<TBackward> ReceiveBackward() {
            return await _backwardChannel.Reader.ReadAsync();
        }

        public void Complete(string message = null) {
            message ??= "channel is completed";
            try {
                _forwardChannel.Writer.Complete(new Exception(message));
                _backwardChannel.Writer.Complete(new Exception(message));
            } catch (InvalidOperationException) { }
        }
    }
}
