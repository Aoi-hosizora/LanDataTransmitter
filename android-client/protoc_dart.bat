@echo off

:: `dart pub global activate protoc_plugin` =>
:: D:/Development/flutter/.pub-cache/hosted/pub.flutter-io.cn/analyzer-1.4.0/lib/src/error/best_practices_verifier.dart:1973:14: 
:: Error: A non-null value must be returned since the return type 'String' doesn't allow null.

:: `C:\Users\AoiHosizora\AppData\Local\Pub\Cache\bin\protoc-gen-dart.bat` =>
:: "D:\Development\flutter\bin\cache\dart-sdk\bin\pub.bat" global run protoc_plugin:protoc_plugin %*

:: protoc --dart_out=grpc:./lib/model -I../protos ../protos/transmitter.proto
protoc ^
	--dart_out=grpc:.\lib\model ^
	--proto_path=..\protos ^
	..\protos\transmitter.proto
