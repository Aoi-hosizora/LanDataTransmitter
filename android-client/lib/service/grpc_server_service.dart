import 'package:grpc/grpc.dart';
import 'package:lan_data_transmitter/model/objects.dart';
import 'package:lan_data_transmitter/model/transmitter.dart';
import 'package:lan_data_transmitter/service/global.dart';
import 'package:lan_data_transmitter/util/bichannel.dart';
import 'package:lan_data_transmitter/util/extensions.dart';
import 'package:lan_data_transmitter/util/util.dart' as util;

typedef ConnectStateChangedCallback = Function(ClientObject);
typedef MessageReceivedCallback = Function(MessageRecord);

class GrpcServerService {
  String address;
  int port;

  GrpcServerService(this.address, this.port);

  TransmitterImpl? _serverImpl;
  Server? _server;

  Future<void> serve() async {
    _serverImpl = TransmitterImpl();
    _server = Server(
      [_serverImpl!],
      [],
      CodecRegistry(codecs: [GzipCodec(), IdentityCodec()]),
    );
    try {
      await _server!.serve(address: address, port: port);
      // $ adb forward tcp:10240 tcp:10240
    } on Exception catch (ex) {
      _serverImpl = null;
      _server = null;
      throw Exception('无法监听端口，详细原因：${ex.message()}');
    }
  }

  Future<void> shutdown() async {
    await disconnectAll('服务器已关闭');
    /* await */
    _server?.shutdown(); // <- slow
  }

  Future<void> disconnectAll([String? message]) async {
    message ??= '服务器要求断开连接';
    var futures = <Future<void>>[];
    Global.server!.connectedClients.forEach((key, value) {
      futures.add(Future.microtask(() async {
        var chan = value.pullChannel;
        try {
          var data = PulledDisconnectReply(disconnect: true);
          var reply = PullReply(accepted: true, type: PulledType.DISCONNECT, disconnect: data);
          await chan.sendForward(reply);
          var _ = await chan.receiveBackward(); // ignore exception
          chan.complete(message);
        } on Exception {
          // ignore all exceptions
        }
      }));
    });
    await Future.wait(futures);
  }

  Future<MessageRecord> sendText(String clientId, String text, DateTime time) async {
    var obj = Global.server!.connectedClients[clientId];
    if (obj == null) {
      throw Exception('服务器暂未连接到客户端 $clientId');
    }
    var messageId = util.generateGlobalId();
    var timestamp = util.toTimestamp(time);
    var chan = obj.pullChannel;
    try {
      // !!!!!!
      var data = PulledTextReply(messageId: messageId, timestamp: timestamp, text: text);
      var reply = PullReply(accepted: true, type: PulledType.TEXT, text: data);
      await chan.sendForward(reply); // goto TransmitterImpl.pull
      var ex = await chan.receiveBackward(); // from TransmitterImpl.pull
      if (ex != null) {
        throw ex;
      }
    } on ChannelClosedException {
      // 服务器已关闭 / 服务器要求断开连接 / 客户端主动断开连接
    } on Exception catch (ex) {
      throw Exception(util.checkGrpcException(ex, isServer: true));
    }
    var record = MessageRecord(clientId: clientId, clientName: obj.name, messageId: messageId, timestamp: timestamp, text: text); // C <- S
    return record; // 通过返回值通知调用方：消息成功发送至客户端
  }

  void setupTransmitter({
    required ConnectStateChangedCallback onConnected,
    required ConnectStateChangedCallback onDisconnected,
    required MessageReceivedCallback onReceived,
  }) {
    _serverImpl!.setupCallbacks(
      onConnected: onConnected, // 回调：服务器连接了新客户端
      onDisconnected: onDisconnected, // 回调：服务器取消连接客户端
      onReceived: onReceived, // 回调：服务器收到了来自客户端的消息
    );
  }
} // class GrpcServerService

class TransmitterImpl extends TransmitterServiceBase {
  ConnectStateChangedCallback? _onConnected;
  ConnectStateChangedCallback? _onDisconnected;
  MessageReceivedCallback? _onReceived;

  void setupCallbacks({
    required ConnectStateChangedCallback onConnected,
    required ConnectStateChangedCallback onDisconnected,
    required MessageReceivedCallback onReceived,
  }) {
    _onConnected = onConnected;
    _onDisconnected = onDisconnected;
    _onReceived = onReceived;
  }

  @override
  Future<ConnectReply> connect(ServiceCall call, ConnectRequest request) {
    var clientId = util.generateGlobalId();
    var name = request.clientName; // can be empty
    if (name.isNotEmpty && Global.server!.connectedClients.values.any((v) => v.name == name)) {
      return Future.value(ConnectReply(accepted: false)); // conflict
    }
    var chan = BiChannel<PullReply, Exception>();
    var obj = ClientObject(id: clientId, name: name, connectedTime: DateTime.now(), pulling: false, pullChannel: chan);
    _onConnected?.call(obj);
    return Future.value(ConnectReply(accepted: true, clientId: clientId));
  }

  @override
  Future<DisconnectReply> disconnect(ServiceCall call, DisconnectRequest request) {
    var obj = Global.server!.connectedClients[request.clientId];
    if (obj == null) {
      return Future.value(DisconnectReply(accepted: false)); // not connect yet
    }
    obj.pullChannel.complete('客户端主动断开连接');
    _onDisconnected?.call(obj);
    return Future.value(DisconnectReply(accepted: true));
  }

  @override
  Future<PushTextReply> pushText(ServiceCall call, PushTextRequest request) {
    // C -> S
    var obj = Global.server!.connectedClients[request.clientId];
    if (obj == null) {
      return Future.value(PushTextReply(accepted: false)); // not connect yet
    }
    var messageId = util.generateGlobalId();
    var record = MessageRecord(clientId: obj.id, clientName: obj.name, messageId: messageId, timestamp: request.timestamp, text: request.text); // C -> S
    _onReceived?.call(record); // 通过回调通知调用方：成功收到来自客户端的消息
    return Future.value(PushTextReply(accepted: true, messageId: messageId));
  }

  @override
  Stream<PullReply> pull(ServiceCall call, PullRequest request) async* {
    // C <- S
    var obj = Global.server!.connectedClients[request.clientId];
    if (obj == null || obj.pulling) {
      yield PullReply(accepted: false);
      return;
    }
    obj.pulling = true;
    var chan = obj.pullChannel;
    // !!!!!!
    while (true) {
      try {
        var reply = await chan.receiveForward(); // from GrpcServerService.sendText
        if (reply != null) {
          yield reply; // TODO error???
          await chan.sendBackward(null /* ex */); // goto GrpcServerService.sendText
        }
      } on ChannelClosedException {
        // 服务器已关闭 / 服务器要求断开连接 / 客户端主动断开连接
        return; // close stream also
      } on Exception catch (ex) {
        try {
          await chan.sendBackward(ex);
        } on ChannelClosedException {
          return;
        }
      }
    }
  }
} // class TransmitterImpl
