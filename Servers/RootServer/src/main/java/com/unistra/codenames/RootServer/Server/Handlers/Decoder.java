package com.unistra.codenames.RootServer.Server.Handlers;

import com.google.protobuf.InvalidProtocolBufferException;
import com.unistra.codenames.RootServer.Proto.Build.*;
import io.netty.buffer.ByteBuf;
import io.netty.channel.ChannelHandlerContext;
import io.netty.handler.codec.ByteToMessageDecoder;
import io.netty.handler.codec.http.FullHttpRequest;

import java.util.List;

public class Decoder extends ByteToMessageDecoder {
    @Override
    protected void decode(ChannelHandlerContext ctx, ByteBuf in, List<Object> out) throws Exception {

        if (in.readableBytes() >= 1) {

           /*
            if (in instanceof FullHttpRequest) {
                System.out.println("Is http");
                ctx.fireChannelRead(in);
                return;
            } else {
                System.out.println("Not http");
                ctx.pipeline().remove("httpCoders");
                ctx.pipeline().remove("httpHandler");
            }
            */

            RawMessageProto.RawMessage rawMessage;
            try {
                byte[] tmp = new byte[in.readableBytes()];
                in.readBytes(tmp);
                rawMessage = RawMessageProto.RawMessage.parseFrom(tmp);
                out.add(rawMessage);
            } catch (InvalidProtocolBufferException ie) {
                System.out.println("Decoder: InvalidProtocolBufferException");
                System.out.println(ie.getLocalizedMessage());
            }
        }
    }
}
