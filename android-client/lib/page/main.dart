import 'package:flutter/material.dart';
import 'package:lan_data_transmitter/page/init.dart';
import 'package:lan_data_transmitter/page/view/message_record_line.dart';
import 'package:lan_data_transmitter/service/global.dart';
import 'package:lan_data_transmitter/util/extensions.dart';

class MainPage extends StatefulWidget {
  const MainPage({Key? key}) : super(key: key);

  @override
  _MainPageState createState() => _MainPageState();
}

class _MainPageState extends State<MainPage> {
  final _textController = TextEditingController();
  var _stopping = false;
  var _forceDisconnecting = false;
  String? _selectedClientId;
  var _wantRestart = false;

  @override
  void initState() {
    super.initState();
    _textController.addListener(() {
      if (mounted) setState(() {});
    });
    if (Global.behavior == ApplicationBehavior.asServer) {
      _prepareGrpcServer();
    } else {
      _prepareGrpcClient();
    }
  }

  @override
  void dispose() {
    _textController.dispose();
    super.dispose();
  }

  void _prepareGrpcServer() {
    Global.server!.service.setupTransmitter(
      onConnected: (obj) {
        Global.server!.connectedClients[obj.id] = obj;
        _selectedClientId ??= obj.id;
        if (mounted) setState(() {});
      },
      onDisconnected: (obj) {
        Global.server!.connectedClients.removeWhere((key, value) => key == obj.id);
        if (_selectedClientId == obj.id) {
          _selectedClientId = Global.server!.connectedClients.isEmpty ? null : Global.server!.connectedClients.keys.first;
        }
        if (mounted) setState(() {});
      },
      onReceived: (r) {
        Global.messages!.addCtSMessage(r); // <<<
        if (mounted) setState(() {});
      },
    );
  }

  void _prepareGrpcClient() {
    Future.microtask(() async {
      Exception? err;
      try {
        var closedByClient = await Global.client!.service.startPulling(
          onReceived: (r) {
            Global.messages!.addStCMessage(r); // <<<
            if (mounted) setState(() {});
          },
        );
        if (closedByClient) {
          return;
        }
      } on Exception catch (ex) {
        err = ex;
        // TODO now just to disconnect the client, later will consider retrying strategy
        try {
          await Global.client!.service.disconnect();
        } on Exception {
          // ignore all exceptions
        }
      }
      // <- here connected has been disconnected.
      Global.state = ApplicationState.stopped;
      if (mounted) setState(() {});
      if (err != null) {
        showInfo(
          title: '客户端获取消息失败',
          message: '无法接受来自服务器的推送。\n原因：${err.message()}',
        );
      } else {
        showInfo(
          title: '客户端连接已断开',
          message: '服务器主动断开与当前客户端的连接。',
        );
      }
    });
  }

  Future<bool> _onStopPressed() async {
    _stopping = true;
    if (mounted) setState(() {});
    var ok = await showQuestion(
      title: '结束确认',
      message: Global.behavior == ApplicationBehavior.asServer ? '是否结束监听？' : '是否断开连接？',
      trueText: Global.behavior == ApplicationBehavior.asServer ? '结束' : '断开',
      falseText: '取消',
    );
    if (!ok) {
      _stopping = false;
      if (mounted) setState(() {});
      return false;
    }
    await Future.microtask(() async {
      try {
        if (Global.behavior == ApplicationBehavior.asServer) {
          await Global.server!.service.shutdown();
        } else {
          await Global.client!.service.disconnect();
        }
      } on Exception {
        // ignore all exceptions
      } finally {
        Global.server?.connectedClients.clear();
        Global.state = ApplicationState.stopped;
        _stopping = false;
        if (mounted) setState(() {});
      }
    });
    return true;
  }

  Future<void> _onForceDisconnectPressed() async {
    _forceDisconnecting = true;
    if (mounted) setState(() {});
    var ok = await showQuestion(
      title: '操作确认',
      message: '是否断开所有客户端的连接？',
      trueText: '断开',
      falseText: '取消',
    );
    if (!ok) {
      _forceDisconnecting = false;
      if (mounted) setState(() {});
    }
    await Future.microtask(() async {
      try {
        await Global.server!.service.disconnectAll();
      } on Exception {
        // ignore all exceptions
      } finally {
        Global.server!.connectedClients.clear();
        _forceDisconnecting = false;
        if (mounted) setState(() {});
      }
    });
  }

  Future<void> _onRestartPressed() async {
    if (Global.state != ApplicationState.stopped) {
      return;
    }
    _wantRestart = true;
    Navigator.of(context).pushAndRemoveUntil(
      MaterialPageRoute(
        builder: (c) => InitPage(),
      ),
      (r) => false,
    );
  }

  Future<bool> _onPopping() async {
    if (_wantRestart || Global.state != ApplicationState.running) {
      return true;
    }
    await _onStopPressed();
    return false;
  }

  Future<void> _onSendTextPressed() async {
    var text = _textController.text.trim();
    var now = DateTime.now();
    if (Global.behavior == ApplicationBehavior.asServer) {
      if (_selectedClientId == null) {
        return;
      }
      await Future.microtask(() async {
        try {
          var r = await Global.server!.service.sendText(_selectedClientId!, text, now);
          Global.messages!.addStCMessage(r); // <<<
          _textController.text = '';
          if (mounted) setState(() {});
        } on Exception catch (ex) {
          showInfo(
            title: '发送失败',
            message: '发送文本至客户端失败。原因：${ex.message()}',
          );
        }
      });
    } else {
      await Future.microtask(() async {
        try {
          var r = await Global.client!.service.sendText(text, now);
          Global.messages!.addCtSMessage(r); // <<<
          _textController.text = '';
          if (mounted) setState(() {});
        } on Exception catch (ex) {
          showInfo(
            title: '发送失败',
            message: '发送文本至服务器失败。原因：${ex.message()}',
          );
        }
      });
    }
  }

  void _onSendFilePressed() {
    // TODO
    showInfo(title: '发送文件', message: 'TODO');
  }

  @override
  Widget build(BuildContext context) {
    const padding = EdgeInsets.symmetric(horizontal: 12, vertical: 8);
    return WillPopScope(
      onWillPop: () async => await _onPopping(),
      child: Scaffold(
        appBar: AppBar(
          title: Text('LAN Data Transmitter ' + (Global.behavior == ApplicationBehavior.asServer ? '(服务器)' : '(客户端)')),
          centerTitle: true,
        ),
        body: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            /// above listview
            Container(
              padding: padding,
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  /// server and client's state
                  if (Global.behavior == ApplicationBehavior.asServer)
                    SingleChildScrollView(
                      scrollDirection: Axis.horizontal,
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Text('服务器状态：'),
                          SizedBox(height: 3),
                          if (Global.state == ApplicationState.running) Text('  当前作为服务器，正在监听 ${Global.server!.service.address}:${Global.server!.service.port}'),
                          if (Global.state != ApplicationState.running) Text('  当前作为服务器，服务器的监听已停止'),
                          if (Global.state == ApplicationState.running && Global.server!.connectedClients.isEmpty) Text('  未连接任何客户端'),
                          if (Global.state == ApplicationState.running && Global.server!.connectedClients.isNotEmpty) Text('  已连接 ${Global.server!.connectedClients.length} 个客户端'),
                          if (Global.state != ApplicationState.running) Text('　'),
                        ],
                      ),
                    ),
                  if (Global.behavior == ApplicationBehavior.asClient)
                    SingleChildScrollView(
                      scrollDirection: Axis.horizontal,
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Text('客户端状态：'),
                          SizedBox(height: 3),
                          if (Global.state == ApplicationState.running) Text('  当前作为客户端，已连接到 ${Global.client!.service.address}:${Global.client!.service.port}'),
                          if (Global.state != ApplicationState.running) Text('  当前作为客户端，与服务器的连接已断开'),
                          if (Global.client!.name.isEmpty) Text('  标识：${Global.client!.id}'),
                          if (Global.client!.name.isNotEmpty) Text('  标识：${Global.client!.id}，名称：${Global.client!.name}'),
                        ],
                      ),
                    ),
                  SizedBox(height: 4),

                  /// state buttons
                  Row(
                    mainAxisAlignment: MainAxisAlignment.end,
                    children: [
                      if (Global.state == ApplicationState.running && Global.behavior == ApplicationBehavior.asServer) ...[
                        OutlinedButton(
                          child: Text('断开所有连接'),
                          onPressed: Global.server!.connectedClients.isEmpty || _forceDisconnecting ? null : () => _onForceDisconnectPressed(),
                        ),
                        SizedBox(width: 12),
                        OutlinedButton(
                          child: Text('结束监听'),
                          onPressed: _stopping ? null : () => _onStopPressed(),
                        ),
                      ],
                      if (Global.state == ApplicationState.running && Global.behavior == ApplicationBehavior.asClient)
                        OutlinedButton(
                          child: Text('断开连接'),
                          onPressed: _stopping ? null : () => _onStopPressed(),
                        ),
                      if (Global.state != ApplicationState.running)
                        OutlinedButton(
                          child: Text(Global.behavior == ApplicationBehavior.asServer ? '重新监听' : '重新连接'),
                          onPressed: () => _onRestartPressed(),
                        ),
                    ],
                  ),
                  SizedBox(height: 8),
                  Divider(height: 1, thickness: 1),
                  SizedBox(height: 8),

                  /// message records state
                  SingleChildScrollView(
                    scrollDirection: Axis.horizontal,
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        if (Global.behavior == ApplicationBehavior.asServer)
                          Text(
                            '消息收发记录: (共收到 ${Global.messages!.ctsCount} 条消息，已发送 ${Global.messages!.stcCount} 条消息)',
                          ),
                        if (Global.behavior == ApplicationBehavior.asClient)
                          Text(
                            '消息收发记录: (共收到 ${Global.messages!.stcCount} 条消息，已发送 ${Global.messages!.ctsCount} 条消息)',
                          ),
                      ],
                    ),
                  ),
                ],
              ),
            ),

            /// listview
            Expanded(
              child: Global.messages!.records.isEmpty
                  ? Center(
                      child: Column(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: [
                          Icon(Icons.clear_all, size: 38, color: Colors.black45),
                          SizedBox(height: 5),
                          Text(
                            '暂无消息',
                            style: Theme.of(context).textTheme.subtitle1!.copyWith(color: Colors.black45),
                          ),
                        ],
                      ),
                    )
                  : Scrollbar(
                      child: ListView(
                        padding: EdgeInsets.symmetric(horizontal: padding.left),
                        children: Global.messages!.records
                            .map(
                              (r) => MessageRecordLine(record: r),
                            )
                            .toList(),
                      ),
                    ),
            ),

            /// below listview
            Container(
              padding: padding,
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  /// clients combobox
                  if (Global.behavior == ApplicationBehavior.asServer)
                    Row(
                      crossAxisAlignment: CrossAxisAlignment.center,
                      children: [
                        Text('发送至：'),
                        SizedBox(width: 5),
                        Expanded(
                          child: IgnorePointer(
                            ignoring: !(Global.state == ApplicationState.running && Global.server!.connectedClients.isNotEmpty),
                            child: DropdownButton<String>(
                              isExpanded: true,
                              value: Global.server!.connectedClients.isEmpty ? '暂无' : _selectedClientId,
                              onChanged: (String? newValue) => mountedSetState(() => _selectedClientId = newValue),
                              items: (Global.server!.connectedClients.isEmpty
                                      ? <List<String>>[
                                          ['暂无客户端', '暂无客户端']
                                        ]
                                      : Global.server!.connectedClients.values.map(
                                          (v) => [v.id, v.fullDisplayName],
                                        ))
                                  .map(
                                    (v) => DropdownMenuItem<String>(
                                      value: v[0],
                                      child: Text(
                                        v[1],
                                        maxLines: 1,
                                        overflow: TextOverflow.ellipsis,
                                        style: Theme.of(context).textTheme.bodyText2!.copyWith(
                                              color: Global.state == ApplicationState.running && Global.server!.connectedClients.isNotEmpty ? null : Colors.grey,
                                            ),
                                      ),
                                    ),
                                  )
                                  .toList(),
                            ),
                          ),
                        ),
                      ],
                    ),

                  /// message text
                  TextField(
                    controller: _textController,
                    decoration: InputDecoration(
                      border: OutlineInputBorder(),
                      contentPadding: EdgeInsets.symmetric(vertical: 8, horizontal: 10),
                      hintText: '此处输入需要发送的内容...',
                    ),
                    maxLines: 3,
                    style: Theme.of(context).textTheme.bodyText2,
                  ),
                  SizedBox(height: 4),

                  /// message buttons
                  Row(
                    mainAxisAlignment: MainAxisAlignment.end,
                    children: [
                      OutlinedButton(
                        child: Text('发送'),
                        onPressed: (Global.state != ApplicationState.running || // 1.
                                _selectedClientId == null || // 2.
                                (Global.behavior == ApplicationBehavior.asServer && (Global.server!.connectedClients.isEmpty || _textController.text.isEmpty)) || // 3.
                                (Global.behavior == ApplicationBehavior.asClient && _textController.text.isEmpty)) // 4.
                            ? null
                            : () => _onSendTextPressed(),
                      ),
                      SizedBox(width: 12),
                      OutlinedButton(
                        child: Text('发送文件'),
                        onPressed: (Global.state != ApplicationState.running || // 1.
                                _selectedClientId == null || // 2.
                                (Global.behavior == ApplicationBehavior.asServer && (Global.server!.connectedClients.isEmpty))) // 3.
                            ? null
                            : () => _onSendFilePressed(),
                      ),
                    ],
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}
