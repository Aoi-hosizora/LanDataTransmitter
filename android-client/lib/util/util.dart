import 'package:uuid/uuid.dart';

String generateGlobalId() {
  var uuid = Uuid();
  return uuid.v4();
}
