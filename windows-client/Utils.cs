using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace LanDataTransmitter {

    public static class Utils {

        private static Dictionary<string, string> _networkInterfaces;

        public static IEnumerable<string> GetNetworkInterfaces() {
            var interfaces = new List<(NetworkInterface, UnicastIPAddressInformation)>();
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces()) {
                foreach (var addr in ni.GetIPProperties().UnicastAddresses) {
                    if (addr.Address.AddressFamily == AddressFamily.InterNetwork) {
                        interfaces.Add((ni, addr));
                        break;
                    }
                }
            }

            const string all = "All interfaces (serve on 0.0.0.0)";
            _networkInterfaces = new Dictionary<string, string> { [all] = "0.0.0.0" };
            foreach (var (ni, addr) in interfaces) {
                _networkInterfaces[ni.Description] = addr.Address.ToString();
            }

            var result = interfaces.Select(i => i.Item1.Description).ToList();
            result.Insert(0, all);
            return result;
        }

        public static string GetNetworkInterfaceIPv4(string description) {
            var ok = _networkInterfaces.TryGetValue(description, out var address);
            if (!ok) {
                address = "unknown";
            }
            return address;
        }

        public static string GetNowString() {
            return DateTime.Now.ToString("yyyyMMdd_HHmmss");
        }

        public static string GenerateGlobalId() {
            return Guid.NewGuid().ToString();
        }
    }

    public class BidChannel<TForward, TBackward> {
        private readonly Channel<TForward> _forwardChannel;
        private readonly Channel<TBackward> _backwardChannel;

        public BidChannel(int capacity) {
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
