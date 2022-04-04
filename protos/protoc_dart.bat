@echo off

:: Install: `dart pub global activate protoc_plugin`
:: Plugin path: `C:\Users\AoiHosizora\AppData\Local\Pub\Cache\bin\protoc-gen-dart.bat`
:: An error:
::   D:/Development/flutter/.pub-cache/hosted/pub.flutter-io.cn/analyzer-1.4.0/lib/src/error/best_practices_verifier.dart:1973:14:
::   Error: A non-null value must be returned since the return type 'String' doesn't allow null.

protoc ^
	--proto_path=. ^
	--dart_out=..\android-client\lib\model ^
	--dart_opt=grpc ^
	.\transmitter.proto
