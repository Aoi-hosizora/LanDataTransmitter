using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace LanDataTransmitter {

    static class Utils {

        public static List<string> GetNetworkInterfaces() {
            var result = new List<string>();
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces()) {
                if (ni.GetIPProperties().UnicastAddresses.Any(addr => addr.Address.AddressFamily == AddressFamily.InterNetwork)) {
                    result.Add(ni.Description);
                }
            }
            return result;
        }

        public static string GetNetworkInterfaceIPv4(string description) {
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces()) {
                if (ni.Description == description) {
                    foreach (var addr in ni.GetIPProperties().UnicastAddresses) {
                        if (addr.Address.AddressFamily == AddressFamily.InterNetwork) {
                            return addr.Address.ToString();
                        }
                    }
                }
            }
            return "unknown";
        }

        public static string GetGlobalId() {
            return Guid.NewGuid().ToString();
        }
    }
}
