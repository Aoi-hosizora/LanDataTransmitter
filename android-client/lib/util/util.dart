import 'package:uuid/uuid.dart';

String generateGlobalId() {
  var uuid = Uuid();
  return uuid.v4();
}

int toTimestamp(DateTime time) {
  return time.millisecondsSinceEpoch ~/ 1000;
}

DateTime fromTimestamp(int timestamp) {
  return DateTime.fromMillisecondsSinceEpoch(timestamp * 1000);
}

class Tuple<T1, T2> {
  T1 item1;
  T2 item2;

  Tuple(this.item1, this.item2);
}
