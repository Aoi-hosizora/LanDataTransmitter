@echo off

:: Package: Google.Protobuf + Grpc + Grpc.Code + Grpc.Code.Api + Grpc.Tools
set tools_path=..\windows-client\packages\Grpc.Tools.2.42.0\tools\windows_x86
set include_path=%tools_path%\..\..\build\native\include

%tools_path%\protoc.exe ^
	--proto_path=. ^
	--proto_path=%include_path% ^
	--plugin=protoc-gen-grpc=%tools_path%\grpc_csharp_plugin.exe ^
	--csharp_out=..\windows-client\Model ^
	--grpc_out=..\windows-client\Model ^
	.\transmitter.proto
