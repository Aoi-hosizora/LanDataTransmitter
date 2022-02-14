import 'package:flutter/material.dart';
import 'package:lan_data_transmitter/page/main.dart';
import 'package:lan_data_transmitter/service/global.dart';
import 'package:lan_data_transmitter/service/grpc_client_service.dart';
import 'package:lan_data_transmitter/service/grpc_server_service.dart';
import 'package:lan_data_transmitter/util/extensions.dart';
import 'package:lan_data_transmitter/util/util.dart' as util;

class InitPage extends StatefulWidget {
  const InitPage({Key? key}) : super(key: key);

  @override
  _InitPageState createState() => _InitPageState();
}

class _InitPageState extends State<InitPage> {
  final _serverFormKey = GlobalKey<FormState>();
  final _clientFormKey = GlobalKey<FormState>();
  final _serveAddrController = TextEditingController();
  final _servePortController = TextEditingController();
  final _targetAddrController = TextEditingController();
  final _targetPortController = TextEditingController();
  final _clientNameController = TextEditingController();
  var _behavior = ApplicationBehavior.server;
  var _trying = false;

  @override
  void initState() {
    super.initState();
    _serveAddrController.text = '0.0.0.0';
    _servePortController.text = '10240';
    _targetAddrController.text = '127.0.0.1';
    _targetPortController.text = '10240';
  }

  @override
  void dispose() {
    _serveAddrController.dispose();
    _servePortController.dispose();
    _targetAddrController.dispose();
    _targetPortController.dispose();
    _clientNameController.dispose();
    super.dispose();
  }

  String? _ipValidator(String? r) {
    if (r == null || r.isEmpty) {
      return '网络地址不允许置空';
    }
    return util.validIpv4Address(r) ? null : '网络地址不合法';
  }

  String? _portValidator(String? r) {
    if (r == null || r.isEmpty) {
      return '端口号不允许置空';
    }
    var port = int.tryParse(r);
    if (port == null) {
      return '端口号不合法';
    }
    return (port >= 1024 && port <= 65535) ? null : '端口号仅允许在 [1024, 65535] 范围内';
  }

  void _start() async {
    if (_behavior == ApplicationBehavior.server && !_serverFormKey.currentState!.validate()) return;
    if (_behavior == ApplicationBehavior.client && !_clientFormKey.currentState!.validate()) return;
    _trying = true;
    if (mounted) setState(() {});

    try {
      if (_behavior == ApplicationBehavior.server) {
        var addr = _serveAddrController.text, port = int.tryParse(_servePortController.text)!;
        var service = GrpcServerService(addr, port);
        await service.serve();
        Global.initializeServer(service); // => ApplicationState.Running
      } else {
        var addr = _targetAddrController.text, port = int.tryParse(_targetPortController.text)!, name = _clientNameController.text;
        var service = GrpcClientService(addr, port);
        var id = await service.connect(name);
        Global.initializeClient(service, id, name); // => ApplicationState.Running
      }
      Navigator.of(context).pushAndRemoveUntil(
        MaterialPageRoute(
          builder: (c) => MainPage(),
        ),
        (r) => false,
      );
    } on Exception catch (ex) {
      _trying = false;
      if (mounted) setState(() {});
      if (_behavior == ApplicationBehavior.server) {
        showInfo(title: '监听失败', message: '无法监听指定的地址和端口。\n原因：${ex.message()}');
      } else {
        showInfo(title: '连接失败', message: '无法连接到指定的地址和端口。\n原因：${ex.message()}');
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('LAN Data Transmitter'),
        centerTitle: true,
      ),
      body: Container(
        padding: EdgeInsets.symmetric(horizontal: 12, vertical: 8),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            /// behavior radio
            Text('请选择 Android 端的行为：'),
            SizedBox(height: 4),
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceEvenly,
              children: [
                InkWell(
                  child: Row(
                    children: [
                      Radio<ApplicationBehavior>(
                        value: ApplicationBehavior.server,
                        groupValue: _behavior,
                        onChanged: (_) => mountedSetState(() => _behavior = ApplicationBehavior.server),
                      ),
                      Text('作为服务器'),
                      SizedBox(width: 16),
                    ],
                  ),
                  onTap: () => mountedSetState(() => _behavior = ApplicationBehavior.server),
                ),
                InkWell(
                  child: Row(
                    children: [
                      Radio<ApplicationBehavior>(
                        value: ApplicationBehavior.client,
                        groupValue: _behavior,
                        onChanged: (_) => mountedSetState(() => _behavior = ApplicationBehavior.client),
                      ),
                      Text('作为客户端'),
                      SizedBox(width: 16),
                    ],
                  ),
                  onTap: () => mountedSetState(() => _behavior = ApplicationBehavior.client),
                ),
              ],
            ),

            /// server form
            if (_behavior == ApplicationBehavior.server)
              Form(
                key: _serverFormKey,
                child: Padding(
                  padding: EdgeInsets.only(left: 8, right: 8, top: 4),
                  child: Column(
                    children: [
                      TextFormField(
                        controller: _serveAddrController,
                        enabled: !_trying,
                        readOnly: true,
                        decoration: InputDecoration(
                          contentPadding: EdgeInsets.symmetric(vertical: 6),
                          labelText: '监听地址 (总是监听所有网络接口的地址)',
                          icon: Icon(Icons.public),
                        ),
                        style: !_trying ? null : TextStyle(color: Theme.of(context).hintColor),
                      ),
                      SizedBox(height: 4),
                      TextFormField(
                        controller: _servePortController,
                        enabled: !_trying,
                        keyboardType: TextInputType.number,
                        validator: _portValidator,
                        decoration: InputDecoration(
                          contentPadding: EdgeInsets.symmetric(vertical: 6),
                          labelText: '监听端口',
                          icon: Icon(Icons.grid_3x3),
                        ),
                        style: !_trying ? null : TextStyle(color: Theme.of(context).hintColor),
                      ),
                    ],
                  ),
                ),
              ),

            /// client form
            if (_behavior == ApplicationBehavior.client)
              Form(
                key: _clientFormKey,
                child: Padding(
                  padding: EdgeInsets.only(left: 8, right: 8, top: 4),
                  child: Column(
                    children: [
                      TextFormField(
                        controller: _targetAddrController,
                        enabled: !_trying,
                        validator: _ipValidator,
                        decoration: InputDecoration(
                          contentPadding: EdgeInsets.symmetric(vertical: 6),
                          labelText: '目的地址',
                          icon: Icon(Icons.public),
                        ),
                        style: !_trying ? null : TextStyle(color: Theme.of(context).hintColor),
                      ),
                      SizedBox(height: 4),
                      TextFormField(
                        controller: _targetPortController,
                        enabled: !_trying,
                        keyboardType: TextInputType.number,
                        validator: _portValidator,
                        decoration: InputDecoration(
                          contentPadding: EdgeInsets.symmetric(vertical: 6),
                          labelText: '目的端口',
                          icon: Icon(Icons.grid_3x3),
                        ),
                        style: !_trying ? null : TextStyle(color: Theme.of(context).hintColor),
                      ),
                      SizedBox(height: 4),
                      TextFormField(
                        controller: _clientNameController,
                        enabled: !_trying,
                        decoration: InputDecoration(
                          contentPadding: EdgeInsets.symmetric(vertical: 6),
                          labelText: '客户端名称 (可空)',
                          icon: Icon(Icons.contact_page_outlined),
                        ),
                        style: !_trying ? null : TextStyle(color: Theme.of(context).hintColor),
                      ),
                    ],
                  ),
                ),
              ),

            /// start button
            SizedBox(height: 20),
            Center(
              child: !_trying
                  ? SizedBox(
                      height: 42,
                      width: 100,
                      child: ElevatedButton(
                        child: Text('开始'),
                        onPressed: _start,
                      ),
                    )
                  : Row(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        SizedBox(
                          height: 42,
                          width: 42,
                          child: CircularProgressIndicator(),
                        ),
                        SizedBox(width: 20),
                        Text(_behavior == ApplicationBehavior.server ? '尝试监听...' : '尝试连接...'),
                      ],
                    ),
            ),
          ],
        ),
      ),
    );
  }
}