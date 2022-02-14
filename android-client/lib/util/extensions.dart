import 'package:flutter/material.dart';

extension PageExtension<T extends StatefulWidget> on State<T> {
  void mountedSetState(VoidCallback f) {
    // ignore: invalid_use_of_protected_member
    if (mounted) setState(f);
  }

  void showInfo({required String title, required String message}) {
    showDialog(
      context: context,
      builder: (c) => AlertDialog(
        title: Text(title),
        content: Text(message),
        actions: [
          TextButton(
            child: Text('确定'),
            onPressed: () => Navigator.of(c).pop(),
          ),
        ],
      ),
    );
  }
}

extension ExceptionExtension on Exception {
  String message() {
    const prefix = 'Exception: ';
    const prefixLength = prefix.length;
    var m = toString();
    if (m.length >= prefixLength && m.substring(0, prefixLength) == prefix) {
      m = m.substring(prefixLength);
    }
    return m;
  }
}
