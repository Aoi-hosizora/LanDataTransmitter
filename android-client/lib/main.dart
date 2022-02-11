import 'package:flutter/material.dart';
import 'package:grpc/grpc.dart';
import 'package:lan_data_transmitter/model/helloworld.pbgrpc.dart';

void main() {
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'LanDataTransmitter',
      theme: ThemeData(
        primarySwatch: Colors.indigo,
      ),
      home: HomePage(),
    );
  }
}

class HomePage extends StatefulWidget {
  HomePage({Key key}) : super(key: key);

  @override
  _HomePageState createState() => _HomePageState();
}

class GreeterServiceImpl extends GreeterServiceBase {
  @override
  Future<HelloReply> sayHello(ServiceCall call, HelloRequest request) async {
    return HelloReply()..message = 'Hello, ${request.name}.';
  }
}

class _HomePageState extends State<HomePage> {
  void testServer() async {
    var serverImpl = GreeterServiceImpl();
    var server = Server([serverImpl]);
    await server.serve(port: 10240);
  }

  void testClient() async {
    var channel = ClientChannel('localhost', port: 10240, options: ChannelOptions(credentials: ChannelCredentials.insecure()));
    var client = GreeterClient(channel);
    try {
      final response = await client.sayHello(HelloRequest()..name = "world");
      print(response.message);
    } catch (e) {
      print('Error: $e');
    } finally {
      await channel.shutdown();
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('LanDataTransmitter'),
      ),
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            OutlineButton(
              child: Text('server'),
              onPressed: () => testServer(),
            ),
            OutlineButton(
              child: Text('client'),
              onPressed: () => testClient(),
            ),
          ],
        ),
      ),
    );
  }
}
