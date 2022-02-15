import 'package:flutter/material.dart';
import 'package:lan_data_transmitter/model/objects.dart';
import 'package:lan_data_transmitter/service/global.dart';
import 'package:lan_data_transmitter/util/util.dart' as util;

class MessageRecordLine extends StatefulWidget {
  final MessageRecord record;

  const MessageRecordLine({
    Key? key,
    required this.record,
  }) : super(key: key);

  @override
  _MessageRecordLineState createState() => _MessageRecordLineState();
}

const String newLineSymbol = '↴';

class _MessageRecordLineState extends State<MessageRecordLine> {
  @override
  Widget build(BuildContext context) {
    var time = util.renderTimeForShow(util.fromTimestamp(widget.record.timestamp));
    String infoLine;
    bool isReceived;
    if (Global.behavior == ApplicationBehavior.asServer) {
      infoLine = '${widget.record.clientDisplayName} ($time)';
      isReceived = widget.record.isCtS;
    } else {
      infoLine = 'server ($time)';
      isReceived = widget.record.isStC;
    }
    var line1 = infoLine;
    var line2 = widget.record.text;
    line1 = isReceived ? '→ $line1' : '$line1 ←';
    line2 = line2.replaceAll('\r\n', newLineSymbol).replaceAll('\n', newLineSymbol);
    return InkWell(
      onTap: () {},
      child: Container(
        padding: EdgeInsets.symmetric(horizontal: 8, vertical: 5),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            SizedBox(
              width: double.infinity,
              child: Text(
                line1,
                maxLines: 1,
                overflow: TextOverflow.ellipsis,
                textAlign: isReceived ? TextAlign.left : TextAlign.right,
              ),
            ),
            SizedBox(height: 4),
            SizedBox(
              width: double.infinity,
              child: Text(
                line2,
                maxLines: 1,
                overflow: TextOverflow.ellipsis,
                textAlign: isReceived ? TextAlign.left : TextAlign.right,
              ),
            ),
          ],
        ),
      ),
    );
  }
}
