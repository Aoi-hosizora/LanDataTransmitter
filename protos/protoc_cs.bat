@echo off

set tools_path=..\windows-client\packages\Grpc.Tools.2.42.0\tools\windows_x86

%tools_path%\protoc.exe ^
	--plugin=protoc-gen-grpc=%tools_path%\grpc_csharp_plugin.exe ^
	--csharp_out=..\windows-client\Model ^
	--grpc_out=..\windows-client\Model ^
	--proto_path=%tools_path%\..\..\build\native\include ^
	--proto_path=. ^
	.\transmitter.proto
