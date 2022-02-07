using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace LanDataTransmitter.Util {

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
    } // Utils

    internal static class NativeMethods {
        public const long SB_HORZ = 0;
        public const long SB_VERT = 1;
        public const long SB_BOTH = 3;

        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);

        [DllImport("user32.dll")]
        public static extern bool ShowScrollBar(IntPtr hWnd, int wBar, [MarshalAs(UnmanagedType.Bool)] bool bShow);

    } // NativeMethods
}
