using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace LanDataTransmitter {

    static class Utils {

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

        public static string GetGlobalId() {
            return Guid.NewGuid().ToString();
        }
    }
}
