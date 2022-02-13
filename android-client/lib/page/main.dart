import 'package:flutter/material.dart';
import 'package:lan_data_transmitter/page/init.dart';

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
        title: Text('MainPage'),
        centerTitle: true,
      ),
      body: Center(
        child: OutlinedButton(
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
      ),
    );
  }
}
