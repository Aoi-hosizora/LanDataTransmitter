using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LanDataTransmitter.Service {

    public class History {

        private class HistoryObject {
            public List<int> ServedPorts { get; set; }
            public List<string> TargetAddresses { get; set; }
            public List<int> TargetPorts { get; set; }
            public List<string> ClientNames { get; set; }
        }

        private HistoryObject _history;

        private static readonly string ConfigureFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config", "LanDataTransmitter");
        private static readonly string HistoryFilePath = Path.Combine(ConfigureFolderPath, "history.json");

        public History() {
            // load
            Load();

            // validate, set to default value if history is empty
            var changed = false;
            if (_history.ServedPorts == null || _history.ServedPorts.Count == 0) {
                _history.ServedPorts = new List<int> { 10240 };
                changed = true;
            }
            if (_history.TargetAddresses == null || _history.TargetAddresses.Count == 0) {
                _history.TargetAddresses = new List<string> { "127.0.0.1" };
                changed = true;
            }
            if (_history.TargetPorts == null || _history.TargetPorts.Count == 0) {
                _history.TargetPorts = new List<int> { 10240 };
                changed = true;
            }
            if (_history.ClientNames == null) {
                _history.ClientNames = new List<string>();
                changed = true;
            }
            if (changed) {
                Save();
            }
        }

        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings {
            Formatting = Formatting.Indented,
            ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() }
        };

        private void Load() {
            if (!Directory.Exists(ConfigureFolderPath)) {
                Directory.CreateDirectory(ConfigureFolderPath);
            }
            if (File.Exists(HistoryFilePath)) {
                var content = File.ReadAllText(HistoryFilePath);
                try {
                    _history = JsonConvert.DeserializeObject<HistoryObject>(content, JsonSerializerSettings);
                    return;
                } catch (Exception) { }
            }
            File.WriteAllText(HistoryFilePath, "{}");
            _history ??= new HistoryObject();
        }

        public void Save() {
            var content = JsonConvert.SerializeObject(_history, JsonSerializerSettings);
            File.WriteAllText(HistoryFilePath, content);
        }

        public void AddServerHistory(int port) {
            _history.ServedPorts.Remove(port);
            _history.ServedPorts.Insert(0, port);
        }

        public void AddClientHistory(string address, int port, string name) {
            _history.TargetAddresses.Remove(address);
            _history.TargetAddresses.Insert(0, address);
            _history.TargetPorts.Remove(port);
            _history.TargetPorts.Insert(0, port);
            if (!string.IsNullOrEmpty(name)) {
                _history.ClientNames.Remove(name);
                _history.ClientNames.Insert(0, name);
            }
        }

        public IEnumerable<int> GetServedPorts() {
            return _history.ServedPorts;
        }

        public IEnumerable<string> GetTargetAddresses() {
            return _history.TargetAddresses;
        }

        public IEnumerable<int> GetTargetPorts() {
            return _history.TargetPorts;
        }

        public IEnumerable<string> GetClientNames() {
            return _history.ClientNames;
        }
    }
}
