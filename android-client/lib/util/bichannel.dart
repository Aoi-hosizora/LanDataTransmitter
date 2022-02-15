import 'dart:async';
import 'dart:collection';
import 'dart:math';

import 'package:lan_data_transmitter/util/util.dart';

class BiChannel<TForward, TBackward> {
  final BlockingChannel<TForward> _forwardChannel;
  final BlockingChannel<TBackward> _backwardChannel;

  BiChannel()
      : _forwardChannel = BlockingChannel<TForward>(),
        _backwardChannel = BlockingChannel<TBackward>();

  Future<void> sendForward(TForward? data) async {
    await _forwardChannel.send(data);
  }

  Future<void> sendBackward(TBackward? data) async {
    await _backwardChannel.send(data);
  }

  Future<TForward?> receiveForward() async {
    var data = await _forwardChannel.receive();
    return data;
  }

  Future<TBackward?> receiveBackward() async {
    var data = await _backwardChannel.receive();
    return data;
  }

  void complete([String? message]) {
    message ??= 'channel is completed';
    try {
      _forwardChannel.complete(message);
      _backwardChannel.complete(message);
    } catch (_) {}
  }
}

class ChannelClosedException implements Exception {
  final String message;

  const ChannelClosedException(this.message);
}

class BlockingChannel<T> {
  final _sendQueue = Queue<Tuple<T?, Completer<void>>>();
  final _recvQueue = Queue<Completer<Tuple<T?, bool>>>();

  var _isClosed = false;
  var _closedMessage = 'channel is completed';

  bool get isClosed => _isClosed;

  Future<void> send(T? value) {
    if (_isClosed) {
      throw ChannelClosedException(_closedMessage);
    }
    final si = Tuple(value, Completer<void>());
    _sendQueue.add(si);
    _notifyReceivers();
    return si.item2.future;
  }

  void _notifyReceivers() {
    if (_sendQueue.isEmpty || _recvQueue.isEmpty) {
      return;
    }
    final count = min(_sendQueue.length, _recvQueue.length);
    for (var i = 0; i < count; i++) {
      final si = _sendQueue.removeFirst();
      final ri = _recvQueue.removeFirst();
      ri.complete(Tuple(si.item1, false));
      si.item2.complete();
    }
  }

  Future<T?> receive() async {
    // 1.  closed &&  empty sendQueue &&  empty recvQueue => exception
    // 2.  closed &&  empty sendQueue && !empty recvQueue => exception (completer in recvQueue need to complete)
    // 3.  closed && !empty sendQueue &&  empty recvQueue => pop and return data directly
    // 4.  closed && !empty sendQueue && !empty recvQueue => create completer and push
    // 5. !closed &&  empty sendQueue &&  empty recvQueue => same with 4
    // 6. !closed &&  empty sendQueue && !empty recvQueue => same with 4
    // 7. !closed && !empty sendQueue &&  empty recvQueue => same with 3
    // 8. !closed && !empty sendQueue && !empty recvQueue => same with 4

    if (_isClosed && _sendQueue.isEmpty) {
      // 1 / 2
      throw ChannelClosedException(_closedMessage);
    }

    if (_sendQueue.isNotEmpty && _recvQueue.isEmpty) {
      // 3 / 7
      var si = _sendQueue.removeFirst();
      si.item2.complete();
      return si.item1;
    }

    // 4 / 5 / 6 / 8
    final ri = Completer<Tuple<T?, bool>>();
    _recvQueue.add(ri);
    final pair = await ri.future;
    if (pair.item2 /* closed */) {
      // 2
      throw ChannelClosedException(_closedMessage);
    }
    return pair.item1;
  }

  void complete([String? message]) {
    _isClosed = true;
    if (message != null) {
      _closedMessage = message;
    }

    _notifyReceivers();
    while (_recvQueue.isNotEmpty) {
      final ri = _recvQueue.removeFirst();
      ri.complete(Tuple(null, true /* closed */));
    }
  }
}
