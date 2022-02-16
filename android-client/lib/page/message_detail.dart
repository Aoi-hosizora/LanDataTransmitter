import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:lan_data_transmitter/model/objects.dart';
import 'package:lan_data_transmitter/service/global.dart';
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
    String sender, receiver;
    if (Global.behavior == ApplicationBehavior.asServer) {
      if (r.isCtS) {
        // received
        sender = r.clientFullDisplayName;
        receiver = 'server (me)';
      } else {
        // sent
        sender = 'server (me)';
        receiver = r.clientFullDisplayName;
      }
    } else {
      if (r.isStC) {
        // received
        sender = 'server';
        receiver = r.clientFullDisplayName + ' (me)';
      } else {
        // sent
        sender = r.clientFullDisplayName + ' (me)';
        receiver = 'server';
      }
    }
    _data = [
      util.Tuple('消息ID', r.messageId),
      util.Tuple('发送方', sender),
      util.Tuple('接收方', receiver),
      util.Tuple('发送时间', DateFormat('yyyy-MM-dd HH:mm:ss').format(util.fromTimestamp(r.timestamp))),
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
        child: ListView(
          padding: EdgeInsets.symmetric(horizontal: 20, vertical: 20),
          children: [
            Table(
              columnWidths: const {
                0: FractionColumnWidth(0.2),
              },
              border: TableBorder(
                horizontalInside: BorderSide(
                  width: 1,
                  color: Colors.grey,
                  style: BorderStyle.solid,
                ),
              ),
              children: [
                for (var data in _data)
                  TableRow(
                    children: [
                      TableRowInkWell(
                        child: Padding(
                          padding: EdgeInsets.symmetric(horizontal: 8, vertical: 5),
                          child: Text('${data.item1}　'),
                        ),
                        onTap: () => util.copyText(data.item2),
                      ),
                      TableRowInkWell(
                        child: Padding(
                          padding: EdgeInsets.symmetric(horizontal: 8, vertical: 5),
                          child: Text('${data.item2}　'),
                        ),
                        onTap: () => util.copyText(data.item2),
                      ),
                    ],
                  ),
              ],
            )
          ],
        ),
      ),
    );
  }
}
