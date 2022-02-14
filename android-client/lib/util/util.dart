import 'package:fixnum/fixnum.dart';
import 'package:regexed_validator/regexed_validator.dart';
import 'package:uuid/uuid.dart';

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

class Tuple<T1, T2> {
  T1 item1;
  T2 item2;

  Tuple(this.item1, this.item2);
}
