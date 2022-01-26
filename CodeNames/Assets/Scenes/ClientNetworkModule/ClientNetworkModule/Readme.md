dotnet add package Google.Protobuf --version 3.14.0


protoc -I=$SRC_DIR --csharp_out=$DST_DIR $SRC_DIR/fileName.proto