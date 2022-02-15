@echo off

.\packages\Grpc.Tools.2.42.0\tools\windows_x86\protoc.exe ^
	--plugin=protoc-gen-grpc=.\packages\Grpc.Tools.2.42.0\tools\windows_x86\grpc_csharp_plugin.exe ^
	--csharp_out=.\Model ^
	--grpc_out=.\Model ^
	--proto_path=.\packages\Grpc.Tools.2.42.0\build\native\include ^
	--proto_path=..\protos ^
	..\protos\transmitter.proto
