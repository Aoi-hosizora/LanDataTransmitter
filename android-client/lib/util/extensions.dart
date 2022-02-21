import 'dart:async';

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

  Future<bool> showQuestion({required String title, required String message, required String trueText, required String falseText}) async {
    var completed = Completer<bool>();
    await showDialog(
      context: context,
      barrierDismissible: false,
      builder: (c) => AlertDialog(
        title: Text(title),
        content: Text(message),
        actions: [
          TextButton(
            child: Text(trueText),
            onPressed: () {
              completed.complete(true);
              Navigator.of(context).pop();
            },
          ),
          TextButton(
            child: Text(falseText),
            onPressed: () {
              completed.complete(false);
              Navigator.of(context).pop();
            },
          ),
        ],
      ),
    );
    if (!completed.isCompleted) {
      completed.complete(false);
    }
    return completed.future;
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

extension StringException on String {
  String unifyToCrlf() {
    var re = RegExp('(?<!\r)\n');
    return replaceAll(re, '\r\n');
  }

  String unifyToLf() {
    var re = RegExp('(?<=\r)\n');
    return replaceAll(re, '\n');
  }
}

extension ScrollControllerExtension on ScrollController {
  void scrollToBottom() {
    if (hasClients) {
      // ScrollController not attached to any scroll views.
      animateTo(
        position.maxScrollExtent,
        duration: Duration(milliseconds: 500),
        curve: Curves.easeInOutQuint,
      );
    }
  }
}
