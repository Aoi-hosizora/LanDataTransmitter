import 'package:flutter/material.dart';
import 'package:lan_data_transmitter/page/init.dart';
import 'package:lan_data_transmitter/service/global.dart';

class MainPage extends StatefulWidget {
  const MainPage({Key? key}) : super(key: key);

  @override
  _MainPageState createState() => _MainPageState();
}

class _MainPageState extends State<MainPage> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('LAN Data Transmitter ' + (Global.behavior == ApplicationBehavior.server ? '(服务器)' : '(客户端)')),
        centerTitle: true,
      ),
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            if (Global.behavior == ApplicationBehavior.client) Text(Global.client!.id),
            if (Global.behavior == ApplicationBehavior.client) Text(Global.client!.name),
            OutlinedButton(
              child: Text('Close me'),
              onPressed: () {
                Navigator.of(context).pushAndRemoveUntil(
                  MaterialPageRoute(
                    builder: (c) => InitPage(),
                  ),
                  (r) => false,
                );
              },
            ),
          ],
        ),
      ),
    );
  }
}
