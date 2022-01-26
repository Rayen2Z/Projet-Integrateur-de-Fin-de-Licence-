package com.unistra.codenames.RootServer.Server.Handlers;

import com.unistra.codenames.RootServer.DTO.Room;
import com.unistra.codenames.RootServer.Proto.Build.RawMessageProto;
import com.unistra.codenames.RootServer.Server.Codes.RequestCodes;
import com.unistra.codenames.RootServer.Server.Codes.ResponseCodes;
import com.unistra.codenames.RootServer.Server.Controller.InRoomController;
import com.unistra.codenames.RootServer.Server.Service.RoomManager;
import io.netty.channel.ChannelHandler;
import io.netty.channel.ChannelHandlerContext;
import io.netty.channel.SimpleChannelInboundHandler;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

@ChannelHandler.Sharable
@Service
public class RoomHandler extends SimpleChannelInboundHandler<RawMessageProto.RawMessage> {
    private InRoomController inRoomController;
    private RoomManager roomManager;


    @Autowired
    public RoomHandler(InRoomController inRoomController, RoomManager roomManager) {
        this.inRoomController = inRoomController;
        this.roomManager = roomManager;
    }

    @Override
    protected void channelRead0(ChannelHandlerContext ctx, RawMessageProto.RawMessage message) throws Exception {
        int requestCode = message.getRequestCode();
        RawMessageProto.RawMessage.Builder builder = RawMessageProto.RawMessage.newBuilder();
        RawMessageProto.RawMessage res;

        /* Start Game */
        if (requestCode == RequestCodes.START_GAME.getValue()) {
            Room room =inRoomController.startGameAsHost(message.getUserID());
            if (room == null) {
                res = builder.setRequestCode(message.getRequestCode())
                        .setUserID(message.getUserID())
                        .setResponseCode(ResponseCodes.FAIL.getValue())
                        .build();
                ctx.writeAndFlush(res);
            } else {

                inRoomController.updateStatusToAll(room, room.getHostID(), RequestCodes.START_GAME, ResponseCodes.GAME_START);
            }

        } else if (requestCode == RequestCodes.CHANGE_TEAM.getValue()) {
            /* TODO */
            int userID = message.getUserID();
            if(!inRoomController.switchTeam(userID)){
                res = builder.setUserID(userID)
                        .setRequestCode(RequestCodes.CHANGE_TEAM.getValue())
                        .setResponseCode(ResponseCodes.FAIL.getValue())
                        .build();
                ctx.writeAndFlush(res);
            }
        } else if (requestCode == RequestCodes.CHANGE_IDENTITY.getValue()) {
            /* TODO */
            int userID = message.getUserID();
            if(!inRoomController.changeIdentity(userID)) {
                res = builder.setUserID(userID)
                        .setRequestCode(RequestCodes.CHANGE_IDENTITY.getValue())
                        .setResponseCode(ResponseCodes.FAIL.getValue())
                        .build();
                ctx.writeAndFlush(res);
            }
        } else if (requestCode == RequestCodes.SET_TIMER.getValue()) {
            if (!inRoomController.setTimer(message.getUserID(), message.getRoomInfo().getTimerValue())){
                res = builder.setRequestCode(message.getRequestCode())
                        .setUserID(message.getUserID())
                        .setResponseCode(ResponseCodes.FAIL.getValue())
                        .build();
                ctx.writeAndFlush(res);
            }
        } else if (requestCode == RequestCodes.GET_THEMES.getValue()) {
            res = builder.mergeFrom(message)
                    .setRoomInfo(inRoomController.getThemes())
                    .build();
            ctx.writeAndFlush(res);
        } else if (requestCode == RequestCodes.SET_THEMES.getValue()) {
            if (!inRoomController.setThemes(message.getUserID(), message.getRoomInfo().getThemeListList())) {
                res = builder.setRequestCode(message.getRequestCode())
                        .setUserID(message.getUserID())
                        .setResponseCode(ResponseCodes.FAIL.getValue())
                        .build();
                ctx.writeAndFlush(res);
            }
        } else if (requestCode == RequestCodes.PLAY_AGENT.getValue()) {
            inRoomController.agentPlay(message.getUserID(), message.getAction());
        } else if (requestCode == RequestCodes.PLAY_DETECTIVE.getValue()) {
            inRoomController.detectivePlay(message);
        } else if (requestCode == RequestCodes.MSG_AGENT.getValue()) {
            inRoomController.agentChat(message);
        } else if (requestCode == RequestCodes.MSG_DETECTIVE.getValue()) {
            inRoomController.detectiveChat(message);

        } else if (requestCode == RequestCodes.QUIT_ROOM.getValue()) {
            int code = ResponseCodes.FAIL.getValue();
            boolean result = inRoomController.quitRoom(message.getUserID());
            if (result) {
                code = ResponseCodes.SUCCESS.getValue();
            }
            res = builder.setRequestCode(message.getRequestCode())
                    .setUserID(message.getUserID())
                    .setResponseCode(code)
                    .build();
            ctx.writeAndFlush(res);
        }
    }
}
