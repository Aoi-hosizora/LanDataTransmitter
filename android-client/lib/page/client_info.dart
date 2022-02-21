import 'package:flutter/material.dart';
import 'package:lan_data_transmitter/page/view/kv_table_view.dart';
import 'package:lan_data_transmitter/service/global.dart';
import 'package:lan_data_transmitter/util/extensions.dart';
import 'package:lan_data_transmitter/util/util.dart' as util;

class ClientInfoPage extends StatefulWidget {
  const ClientInfoPage({Key? key}) : super(key: key);

  @override
  _ClientInfoPageState createState() => _ClientInfoPageState();
}

class _ClientInfoPageState extends State<ClientInfoPage> {
  var _ok = false;
  var _data = <util.Tuple<String, String>>[];

  @override
  void initState() {
    super.initState();
    if (Global.behavior == ApplicationBehavior.asServer) {
      showInfo(title: '错误', message: '错误操作').then((_) {
        Navigator.of(context).pop();
      });
      return;
    }
    _ok = true;
    _data = [
      util.Tuple('客户端标识', Global.client!.obj.id),
      util.Tuple('客户端名称', Global.client!.obj.name),
      util.Tuple('连接时间', util.fromTimestamp(Global.client!.obj.connectedTimestamp).format('yyyy-MM-dd HH:mm:ss')),
      util.Tuple('服务器地址', '${Global.client!.service.address}:${Global.client!.service.port}'),
    ];
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
              child: SingleChildScrollView(
                scrollDirection: Axis.vertical,
                padding: EdgeInsets.symmetric(horizontal: 20, vertical: 20),
                child: KvTableView(
                  data: _data,
                  keyColumnWidthFraction: 0.25,
                  onTap: (t, _) => util.copyText(t.item2, showToast: true),
                ),
              ),
            ),
    );
  }
}
