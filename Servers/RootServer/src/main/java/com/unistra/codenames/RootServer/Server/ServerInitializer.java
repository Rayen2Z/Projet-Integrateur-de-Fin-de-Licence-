package com.unistra.codenames.RootServer.Server;

import com.unistra.codenames.RootServer.Server.Handlers.*;
import io.netty.channel.Channel;
import io.netty.channel.ChannelInitializer;
import io.netty.channel.ChannelPipeline;
import io.netty.handler.codec.http.HttpServerCodec;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

@Service
public class ServerInitializer extends ChannelInitializer {

    private LoginHandler loginHandler;
    private LobbyHandler lobbyHandler;
    private RoomHandler roomHandler;

    @Autowired
    public ServerInitializer(LoginHandler loginHandler, LobbyHandler lobbyHandler, RoomHandler roomHandler) {
        this.loginHandler = loginHandler;
        this.lobbyHandler = lobbyHandler;
        this.roomHandler = roomHandler;
    }
    @Override
    protected void initChannel(Channel ch) {
        ChannelPipeline pipeline = ch.pipeline();
        pipeline.addLast("tcpDecoder", new Decoder());
        // pipeline.addLast("httpCoders", new HttpServerCodec());
        pipeline.addLast("tcpEncoder", new Encoder());
        // pipeline.addLast("httpHandler", new HttpRequestHandler());

        pipeline.addLast(loginHandler);
        pipeline.addLast(lobbyHandler);
        pipeline.addLast(roomHandler);
    }
}
