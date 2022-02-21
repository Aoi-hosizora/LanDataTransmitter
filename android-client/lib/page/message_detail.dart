import 'package:flutter/material.dart';
import 'package:lan_data_transmitter/model/objects.dart';
import 'package:lan_data_transmitter/page/view/kv_table_view.dart';
import 'package:lan_data_transmitter/service/global.dart';
import 'package:lan_data_transmitter/util/extensions.dart';
import 'package:lan_data_transmitter/util/util.dart' as util;

class MessageDetailPage extends StatefulWidget {
  final MessageRecord record;

  const MessageDetailPage({
    Key? key,
    required this.record,
  }) : super(key: key);

  @override
  _MessageDetailPageState createState() => _MessageDetailPageState();
}

class _MessageDetailPageState extends State<MessageDetailPage> {
  var _data = <util.Tuple<String, String>>[];

  @override
  void initState() {
    super.initState();
    var r = widget.record;

    var client = r.clientFullDisplayName;
    var server = 'server';
    String sender, receiver;
    if (Global.behavior == ApplicationBehavior.asServer) {
      server += ' (me)';
      var connected = Global.server!.connectedClients.containsKey(r.clientId);
      if (!connected) {
        client += ' (disconnected)';
      }
      if (r.isCtS) {
        // received
        sender = client;
        receiver = server;
      } else {
        // sent
        sender = server;
        receiver = client;
      }
    } else {
      client += ' (me)';
      if (r.isStC) {
        // received
        sender = server;
        receiver = client;
      } else {
        // sent
        sender = client;
        receiver = server;
      }
    }
    _data = [
      util.Tuple('消息ID', r.messageId),
      util.Tuple('发送方', sender),
      util.Tuple('接收方', receiver),
      util.Tuple('发送时间', util.fromTimestamp(r.timestamp).format('yyyy-MM-dd HH:mm:ss')),
      util.Tuple('详细内容', r.text),
    ];
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('消息详情'),
        centerTitle: true,
      ),
      body: Scrollbar(
        child: SingleChildScrollView(
          scrollDirection: Axis.vertical,
          padding: EdgeInsets.symmetric(horizontal: 20, vertical: 20),
          child: KvTableView(
            data: _data,
            onTap: (t, _) => util.copyText(t.item2, showToast: true),
          ),
        ),
      ),
    );
  }
}
