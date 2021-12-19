using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace LanDataTransmitter {

    static class Utils {

        public static IEnumerable<string> GetNetworkInterfaces() {
            var result = new List<string>();
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces()) {
                if (ni.GetIPProperties().UnicastAddresses.Any(addr => addr.Address.AddressFamily == AddressFamily.InterNetwork)) {
                    result.Add(ni.Description);
                }
            }
            result.Insert(0, "All interfaces (serve on 0.0.0.0)");
            return result;
        }

        public static string GetNetworkInterfaceIPv4(string description) {
            if (description == "All interfaces (serve on 0.0.0.0)") {
                return "0.0.0.0";
            }
            var ni = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(ni => ni.Description == description);
            if (ni != null) {
                var addr = ni.GetIPProperties().UnicastAddresses.FirstOrDefault(addr => addr.Address.AddressFamily == AddressFamily.InterNetwork);
                if (addr != null) {
                    return addr.Address.ToString();
                }
            }
            return "unknown";
        }

        public static string GetGlobalId() {
            return Guid.NewGuid().ToString();
        }
    }
}
