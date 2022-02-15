import 'package:fixnum/fixnum.dart';
import 'package:intl/intl.dart';
import 'package:lan_data_transmitter/util/extensions.dart';
import 'package:regexed_validator/regexed_validator.dart';
import 'package:uuid/uuid.dart';

class Tuple<T1, T2> {
  T1 item1;
  T2 item2;

  Tuple(this.item1, this.item2);
}

String generateGlobalId() {
  var uuid = Uuid();
  return uuid.v4();
}

bool validIpv4Address(String address) {
  return validator.ip(address);
}

Int64 toTimestamp(DateTime time) {
  int timestamp = time.millisecondsSinceEpoch ~/ 1000;
  return Int64.fromInts(timestamp >> 32, timestamp & 0xffffffff);
}

DateTime fromTimestamp(Int64 timestamp) {
  var ms = timestamp.toInt() * 1000;
  return DateTime.fromMillisecondsSinceEpoch(ms);
}

String renderTimeForShow(DateTime time) {
  var now = DateTime.now();
  var sameDay = now.year == time.year && now.month == time.month && now.day == time.day;
  var fmt = DateFormat(sameDay ? 'HH:mm:ss' : 'MM-dd HH:mm:ss');
  return fmt.format(time);
}

String checkGrpcException(Exception ex, {required bool isServer}) {
  var err = ex.message();
  if (err.contains('Network is unreachable')) {
    return '当前无网络连接';
  }
  if (isServer) {
    return '无法连接到客户端，详细原因：${ex.message()}';
  }
  return '无法连接到服务器，详细原因：${ex.message()}';
}
