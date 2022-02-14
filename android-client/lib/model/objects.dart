import 'package:fixnum/fixnum.dart';
import 'package:lan_data_transmitter/model/transmitter.dart';
import 'package:lan_data_transmitter/util/bichannel.dart';
import 'package:lan_data_transmitter/util/util.dart' as util;

class ClientObject {
  String id;
  String name;
  DateTime connectedTime;
  bool pulling;
  BiChannel<PullReply, Exception> pullChannel;

  String get fullDisplayName => name == '' ? id : '$id ($name)';

  ClientObject({
    required this.id,
    required this.name,
    required this.connectedTime,
    required this.pulling,
    required this.pullChannel,
  });

  static util.Tuple<String, String> extractIdAndName(String s) {
    var sp = s.split(' ');
    return util.Tuple(sp.isEmpty ? '' : sp[0], sp.length <= 1 ? '' : sp[1]);
  }
} // class ClientObject

class MessageRecord {
  // client
  bool isCtS = false; // this field will be set when AddCtSMessage or AddStCMessage invoked
  String clientId;
  String clientName;

  bool get isStC => !isCtS;

  String get clientDisplayName => clientName == '' ? clientId : clientName;

  String get clientFullDisplayName => clientName == '' ? clientId : '$clientId ($clientName)';

  // message
  String messageId;
  Int64 timestamp;
  String text;

  MessageRecord({
    required this.clientId,
    required this.clientName,
    required this.messageId,
    required this.timestamp,
    required this.text,
  });
} // class MessageRecord

class MessageRepository {
  List<MessageRecord> records;
  int ctsCount;

  int get stcCount => records.length - ctsCount;

  MessageRepository()
      : records = <MessageRecord>[],
        ctsCount = 0;

  void addCtSMessage(MessageRecord r) {
    r.isCtS = true;
    records.add(r);
    ctsCount++;
  }

  void addStCMessage(MessageRecord r) {
    r.isCtS = false;
    records.add(r);
  }
} // class MessageRepository
