package com.unistra.codenames.RootServer.Server.Handlers;

import com.unistra.codenames.RootServer.Proto.Build.RawMessageProto;
import io.netty.buffer.ByteBuf;
import io.netty.channel.ChannelHandlerContext;
import io.netty.handler.codec.MessageToByteEncoder;
import io.netty.handler.codec.http.FullHttpResponse;

public class Encoder extends MessageToByteEncoder<Object> {

    @Override
    protected void encode(ChannelHandlerContext ctx, Object msg, ByteBuf out) throws Exception {

        /*
        if (msg instanceof FullHttpResponse) {
            ctx.pipeline().fireChannelRead(msg);
        }
        */

        RawMessageProto.RawMessage message = (RawMessageProto.RawMessage) msg;

        out.writeBytes(message.toByteArray());
    }
}
