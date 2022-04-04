using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace LanDataTransmitter.Util {

    public static class Utils {

        private static Dictionary<string, string> _networkInterfaces;

        public static IEnumerable<string> GetNetworkInterfaces() {
            if (_networkInterfaces == null || _networkInterfaces.Count == 0) {
                var interfaces = new List<(NetworkInterface, UnicastIPAddressInformation)>();
                foreach (var ni in NetworkInterface.GetAllNetworkInterfaces()) {
                    foreach (var addr in ni.GetIPProperties().UnicastAddresses) {
                        if (addr.Address.AddressFamily == AddressFamily.InterNetwork) {
                            interfaces.Add((ni, addr));
                            break;
                        }
                    }
                }

                _networkInterfaces = new Dictionary<string, string> { ["All interfaces (serve on 0.0.0.0)"] = "0.0.0.0" };
                foreach (var (ni, addr) in interfaces) {
                    _networkInterfaces[ni.Description] = addr.Address.ToString();
                }
            }
            return _networkInterfaces.Keys.ToList();
        }

        public static string GetNetworkInterfaceIPv4(string description) {
            if (_networkInterfaces == null || _networkInterfaces.Count == 0) {
                var _ = GetNetworkInterfaces();
            }
            var ok = _networkInterfaces!.TryGetValue(description, out var address);
            if (!ok) {
                address = "0.0.0.0";
            }
            return address;
        }

        public static string GenerateGlobalId() {
            return Guid.NewGuid().ToString();
        }

        public static bool ValidIpv4Address(string address) {
            return IPAddress.TryParse(address, out _);
        }

        public static ulong ToTimestamp(DateTime time) {
            return (ulong) new DateTimeOffset(time).ToUnixTimeSeconds();
        }

        public static DateTime FromTimestamp(ulong timestamp) {
            return DateTimeOffset.FromUnixTimeSeconds((long) timestamp).LocalDateTime;
        }

        public static string FormatTimeForShow(DateTime time) {
            var now = DateTime.Now;
            return time.ToString(time.DayOfYear == now.DayOfYear ? "HH:mm:ss" : "MM-dd HH:mm:ss");
        }

        public static string FormatGrpcException(Exception ex, bool isServer) {
            var err = ex.Message;
            if (err.Contains("Network is unreachable")) {
                return "当前无网络连接";
            }
            if (isServer) {
                return $"无法连接到客户端 ({ex.Message})";
            }
            return $"无法连接到服务器 ({ex.Message})";
        }

        public static Tuple<FileInfo, string> CheckSentFile(string f) {
            if (!File.Exists(f)) {
                return new Tuple<FileInfo, string>(null, $"文件 \"{f}\" 不存在");
            }
            var fi = new FileInfo(f);
            if (fi.Length <= 0) { // (-inf, 0B]
                return new Tuple<FileInfo, string>(null, $"不允许传输大小为 0B 的文件 ({f})");
            }
            if (fi.Length > 512 * 1024 * 1024) { // (512MB, inf)
                return new Tuple<FileInfo, string>(null, $"不支持传输大小超过 512MB 的文件 ({f})");
            }
            return new Tuple<FileInfo, string>(fi, "");
        }

        private static readonly string ReceivedFolderPath = Path.Combine(Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config", "LanDataTransmitter"), "received");

        public static string GenerateFilepath(string filename) {
            if (!Directory.Exists(ReceivedFolderPath)) {
                Directory.CreateDirectory(ReceivedFolderPath);
            }
            filename = Path.GetFileName(filename);
            var pureName = Path.GetFileNameWithoutExtension(filename);
            var extension = Path.GetExtension(filename);
            var count = 0;
            while (true) {
                var filepath = Path.Combine(ReceivedFolderPath, count == 0 ? filename : $"{pureName}-{count}{extension}");
                if (!Directory.Exists(filepath) && !File.Exists(filepath)) {
                    return filepath;
                }
                count++;
            }
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
