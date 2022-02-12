import 'package:flutter/material.dart';
import 'package:fluttertoast/fluttertoast.dart';
import 'package:grpc/grpc.dart';
import 'package:lan_data_transmitter/model/transmitter.pbgrpc.dart';
import 'package:lan_data_transmitter/service/global.dart';
import 'package:lan_data_transmitter/service/server.dart';

class InitPage extends StatefulWidget {
  const InitPage({Key? key}) : super(key: key);

  @override
  _InitPageState createState() => _InitPageState();
}

class _InitPageState extends State<InitPage> {
  void _testServer() async {
    var serverImpl = TransmitterImpl();
    var server = Server([serverImpl]);
    Fluttertoast.showToast(msg: 'Serving on 10240...');
    await server.serve(address: '0.0.0.0', port: 10240);
  }

  void _testClient() async {
    var channel = ClientChannel(
      '10.0.3.2',
      port: 10240,
      options: ChannelOptions(credentials: ChannelCredentials.insecure()),
    );
    var client = TransmitterClient(channel);
    try {
      final response = await client.connect(ConnectRequest()..clientName = 'world');
      Fluttertoast.showToast(msg: 'response.clientId: ${response.clientId}');
      print(response.clientId);
    } catch (e) {
      print('Error: $e');
    } finally {
      await channel.shutdown();
    }
  }

  var _behavior = ApplicationBehavior.server;

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
            Text('请选择 Android 端的行为：'),
            Row(
              children: [
                Radio<ApplicationBehavior>(
                  value: ApplicationBehavior.server,
                  groupValue: _behavior,
                  onChanged: (b) {
                    _behavior = b!;
                    if (mounted) setState(() {});
                  },
                ),
                Text('作为服务器'),
              ],
            ),
            Row(
              children: [
                Radio<ApplicationBehavior>(
                  value: ApplicationBehavior.client,
                  groupValue: _behavior,
                  onChanged: (b) {
                    _behavior = b!;
                    if (mounted) setState(() {});
                  },
                ),
                Text('作为客户端'),
              ],
            ),
            if (_behavior == ApplicationBehavior.server)
              Center(
                child: OutlinedButton(
                  child: Text('服务器'),
                  onPressed: () => _testServer(),
                ),
              ),
            if (_behavior == ApplicationBehavior.client)
              Center(
                child: OutlinedButton(
                  child: Text('客户端'),
                  onPressed: () => _testClient(),
                ),
              ),
          ],
        ),
      ),
    );
  }
}
