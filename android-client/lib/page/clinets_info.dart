import 'package:flutter/material.dart';
import 'package:lan_data_transmitter/page/view/kv_table_view.dart';
import 'package:lan_data_transmitter/service/global.dart';
import 'package:lan_data_transmitter/util/extensions.dart';
import 'package:lan_data_transmitter/util/util.dart' as util;

class ClientsInfoPage extends StatefulWidget {
  const ClientsInfoPage({Key? key}) : super(key: key);

  @override
  _ClientsInfoPageState createState() => _ClientsInfoPageState();
}

class _ClientsInfoPageState extends State<ClientsInfoPage> {
  var _ok = false;
  var _data = <util.Tuple<String, String>>[];
  late String _selectedClientId;

  @override
  void initState() {
    super.initState();
    if (Global.behavior == ApplicationBehavior.asClient || Global.server!.connectedClients.isEmpty) {
      showInfo(title: '错误', message: '错误操作').then((_) {
        Navigator.of(context).pop();
      });
      return;
    }
    _changeSelectTo(Global.server!.connectedClients.values.first.id);
    _ok = true;
  }

  void _changeSelectTo(String id) {
    _selectedClientId = id;
    var obj = Global.server!.connectedClients[id]!;
    _data = [
      util.Tuple('服务器监听地址', '${Global.server!.service.address}:${Global.server!.service.port}'),
      util.Tuple('客户端标识', obj.id),
      util.Tuple('客户端名称', obj.name),
      util.Tuple('连接时间', util.fromTimestamp(obj.connectedTimestamp).format('yyyy-MM-dd HH:mm:ss')),
    ];
    if (mounted) setState(() {});
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('客户端信息'),
        centerTitle: true,
      ),
      body: !_ok
          ? Container()
          : Scrollbar(
              child: ListView(
                padding: EdgeInsets.symmetric(horizontal: 20, vertical: 20),
                children: [
                  Text('当前已连接到服务器的所有客户端：'),
                  SizedBox(width: 5),
                  DropdownButton<String>(
                    isExpanded: true,
                    value: Global.server!.connectedClients[_selectedClientId]!.id,
                    onChanged: (newValue) => newValue == null ? {} : _changeSelectTo(newValue),
                    style: TextStyle(color: Colors.black),
                    items: [
                      for (var v in Global.server!.connectedClients.values)
                        DropdownMenuItem<String>(
                          value: v.id,
                          child: Text(
                            v.fullDisplayName,
                            maxLines: 1,
                            overflow: TextOverflow.ellipsis,
                            style: Theme.of(context).textTheme.bodyText2,
                          ),
                        )
                    ],
                  ),
                  SizedBox(width: 5),
                  KvTableView(
                    data: _data,
                    keyColumnWidthFraction: 0.3,
                    onTap: (t, _) => util.copyText(t.item2, showToast: true),
                  ),
                ],
              ),
            ),
    );
  }
}
