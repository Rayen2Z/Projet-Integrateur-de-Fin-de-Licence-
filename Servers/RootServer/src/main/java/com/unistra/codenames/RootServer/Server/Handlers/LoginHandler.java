package com.unistra.codenames.RootServer.Server.Handlers;

import com.unistra.codenames.RootServer.Proto.Build.RawMessageProto;
import com.unistra.codenames.RootServer.Server.Codes.RequestCodes;
import com.unistra.codenames.RootServer.Server.Codes.ResponseCodes;
import com.unistra.codenames.RootServer.Server.Controller.LobbyController;
import com.unistra.codenames.RootServer.Server.Controller.LoginController;
import io.netty.channel.ChannelHandler;
import io.netty.channel.ChannelHandlerContext;
import io.netty.channel.SimpleChannelInboundHandler;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.Collection;


@ChannelHandler.Sharable
@Service
public class LoginHandler extends SimpleChannelInboundHandler<RawMessageProto.RawMessage> {

    private LobbyController lobbyController;
    private LoginController loginController;

    @Autowired
    public LoginHandler(LobbyController lobbyController, LoginController loginController) {
        this.lobbyController = lobbyController;
        this.loginController = loginController;
    }


    @Override
    protected void channelRead0(ChannelHandlerContext ctx, RawMessageProto.RawMessage message) throws Exception {
        Integer requestCode = message.getRequestCode();
        RawMessageProto.RawMessage.Builder builder = RawMessageProto.RawMessage.newBuilder();
        RawMessageProto.RawMessage res;
        Collection<RawMessageProto.RawMessage.RoomBrief> roomBriefCollection = null;
        Integer userID = 0;

        if (requestCode.equals(RequestCodes.REGISTER.getValue())) {
            ResponseCodes registerResult = loginController.register(message.getPseudo(), message.getPassword());

            if (registerResult == ResponseCodes.SUCCESS) {
                userID = loginController.getUserIDByPseudo(message.getPseudo());
            }

            res = builder.setRequestCode(message.getRequestCode())
                    .setResponseCode(registerResult.getValue())
                    .setUserID(userID)
                    .buildPartial();

            ctx.writeAndFlush(res);
        } else if (requestCode.equals(RequestCodes.LOGIN.getValue())) {
            ResponseCodes loginResult = loginController.login(message.getPseudo(), message.getPassword(), ctx.channel());

            if (loginResult == ResponseCodes.SUCCESS) {
                userID = loginController.getUserIDByPseudo(message.getPseudo());
                roomBriefCollection = lobbyController.getBriefRoomList();
            }

            if (roomBriefCollection!=null) {
                res = builder.setRequestCode(message.getRequestCode())
                        .setResponseCode(loginResult.getValue())
                        .setUserID(userID)
                        .addAllRoomList(roomBriefCollection)
                        .build();
            } else {
                res = builder.setRequestCode(message.getRequestCode())
                        .setResponseCode(loginResult.getValue())
                        .setUserID(userID)
                        .build();
            }

            ctx.writeAndFlush(res);
        } else if (loginController.isConnected(message.getUserID())) {
            System.out.println("Connected");
            ctx.fireChannelRead(message);
        } else {
            res = builder.setRequestCode(message.getRequestCode())
                    .setResponseCode(ResponseCodes.LOGIN_REQUESTED.getValue())
                    .setUserID(message.getUserID())
                    .build();

            ctx.writeAndFlush(res);
        }
        System.out.println("End of Login Handler");
    }
}
