import 'package:shared_preferences/shared_preferences.dart';

class _HistoryObject {
  List<int> servedPorts = [];
  List<String> targetAddresses = [];
  List<int> targetPorts = [];
  List<String> clientNames = [];
}

const _servedPortsKey = 'served_ports';
const _targetAddressesKey = 'target_addresses';
const _targetPortsKey = 'target_ports';
const _clientNamesKey = 'client_names';

class History {
  History._();

  final _history = _HistoryObject();

  static Future<History> create() async {
    var h = History._();

    // load
    await h._load();

    // validate, set to default value if history is empty
    var changed = false;
    if (h._history.servedPorts.isEmpty) {
      h._history.servedPorts = [10240];
      changed = true;
    }
    if (h._history.targetAddresses.isEmpty) {
      h._history.targetAddresses = ['127.0.0.1'];
      changed = true;
    }
    if (h._history.targetPorts.isEmpty) {
      h._history.targetPorts = [10240];
      changed = true;
    }
    if (h._history.clientNames.isEmpty) {
      h._history.clientNames = [];
      changed = true;
    }
    if (changed) {
      h.save();
    }

    return h;
  }

  Future<void> _load() async {
    List<int> expandFn(String s) {
      var i = int.tryParse(s);
      return i == null ? <int>[] : [int.tryParse(s)!];
    }

    final prefs = await SharedPreferences.getInstance();
    _history.servedPorts = prefs.getStringList(_servedPortsKey)?.expand(expandFn).toList() ?? [];
    _history.targetAddresses = prefs.getStringList(_targetAddressesKey) ?? [];
    _history.targetPorts = prefs.getStringList(_targetPortsKey)?.expand(expandFn).toList() ?? [];
    _history.clientNames = prefs.getStringList(_clientNamesKey) ?? [];
  }

  Future<void> save() async {
    final prefs = await SharedPreferences.getInstance();
    await prefs.setStringList(_servedPortsKey, _history.servedPorts.map((e) => e.toString()).toList());
    await prefs.setStringList(_targetAddressesKey, _history.targetAddresses);
    await prefs.setStringList(_targetPortsKey, _history.targetPorts.map((e) => e.toString()).toList());
    await prefs.setStringList(_clientNamesKey, _history.clientNames);
  }

  void addServerHistory(int port) {
    _history.servedPorts.remove(port);
    _history.servedPorts.insert(0, port);
  }

  void addClientHistory(String address, int port, String name) {
    _history.targetAddresses.remove(address);
    _history.targetAddresses.insert(0, address);
    _history.targetPorts.remove(port);
    _history.targetPorts.insert(0, port);
    name = name.trim();
    if (name.isNotEmpty) {
      _history.clientNames.remove(name);
      _history.clientNames.insert(0, name);
    }
  }

  void removeHistory({String? servedPort, String? targetAddresses, String? targetPort, String? clientName}) {
    if (servedPort != null) {
      var s = int.tryParse(servedPort);
      if (s != null) {
        _history.servedPorts.remove(s);
      }
    }
    if (targetAddresses != null) {
      _history.targetAddresses.remove(targetAddresses);
    }
    if (targetPort != null) {
      var s = int.tryParse(targetPort);
      if (s != null) {
        _history.targetPorts.remove(s);
      }
    }
    if (clientName != null) {
      _history.clientNames.remove(clientName);
    }
    save();
  }

  List<String> getServedPorts() {
    return _history.servedPorts.map((e) => e.toString()).toList();
  }

  List<String> getTargetAddresses() {
    return _history.targetAddresses;
  }

  List<String> getTargetPorts() {
    return _history.targetPorts.map((e) => e.toString()).toList();
  }

  List<String> getClientNames() {
    return _history.clientNames;
  }
}
