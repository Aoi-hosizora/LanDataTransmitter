import 'package:flutter/material.dart';
import 'package:lan_data_transmitter/util/util.dart' as util;

class KvTableView extends StatelessWidget {
  final List<util.Tuple<String, String>> data;
  final double keyColumnWidthFraction;
  final void Function(util.Tuple<String, String>, bool keyPressed)? onTap;

  const KvTableView({
    Key? key,
    required this.data,
    this.keyColumnWidthFraction = 0.2,
    this.onTap,
  })  : assert(data != const <util.Tuple<String, String>>[]),
        super(key: key);

  @override
  Widget build(BuildContext context) {
    return Table(
      columnWidths: {
        0: FractionColumnWidth(keyColumnWidthFraction),
      },
      border: TableBorder(
        horizontalInside: BorderSide(
          width: 1,
          color: Colors.grey,
          style: BorderStyle.solid,
        ),
      ),
      children: [
        for (var line in data)
          TableRow(
            children: [
              TableRowInkWell(
                child: Padding(
                  padding: EdgeInsets.symmetric(horizontal: 8, vertical: 5),
                  child: Text('${line.item1}　'),
                ),
                onTap: onTap == null ? null : () => onTap!(line, true),
              ),
              TableRowInkWell(
                child: Padding(
                  padding: EdgeInsets.symmetric(horizontal: 8, vertical: 5),
                  child: Text('${line.item2}　'),
                ),
                onTap: onTap == null ? null : () => onTap!(line, false),
              ),
            ],
          ),
      ],
    );
  }
}
