package com.unistra.codenames.RootServer.Server.Handlers;

import com.unistra.codenames.RootServer.DTO.Room;
import com.unistra.codenames.RootServer.Proto.Build.RawMessageProto;
import com.unistra.codenames.RootServer.Server.Codes.RequestCodes;
import com.unistra.codenames.RootServer.Server.Codes.ResponseCodes;
import com.unistra.codenames.RootServer.Server.Controller.InRoomController;
import com.unistra.codenames.RootServer.Server.Controller.LobbyController;
import com.unistra.codenames.RootServer.Server.Service.ThemeManager;
import io.netty.channel.ChannelHandler;
import io.netty.channel.ChannelHandlerContext;
import io.netty.channel.SimpleChannelInboundHandler;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.Collection;

@ChannelHandler.Sharable
@Service
public class LobbyHandler extends SimpleChannelInboundHandler {
    private LobbyController lobbyController;
    private ThemeManager themeManager;
    private InRoomController inRoomController;

    @Autowired
    public  LobbyHandler(LobbyController lobbyController, ThemeManager themeManager, InRoomController inRoomController) {
        this.lobbyController = lobbyController;
        this.themeManager = themeManager;
        this.inRoomController = inRoomController;
    }

    @Override
    protected void channelRead0(ChannelHandlerContext ctx, Object msg) throws Exception {
        System.out.println("Head of Lobby Handler");
        RawMessageProto.RawMessage message = (RawMessageProto.RawMessage) msg;
        Integer requestCode = message.getRequestCode();
        RawMessageProto.RawMessage.Builder builder = RawMessageProto.RawMessage.newBuilder();
        RawMessageProto.RawMessage.RoomInfo.Builder roomInfoBuilder = RawMessageProto.RawMessage.RoomInfo.newBuilder();
        RawMessageProto.RawMessage res;
        RawMessageProto.RawMessage.RoomInfo roomInfo;
        Collection<RawMessageProto.RawMessage.RoomBrief> roomBriefCollection;
        System.out.println("RequestCode: " + requestCode);

        if (requestCode.equals(RequestCodes.REFRESH_ROOM_LIST.getValue()) || requestCode.equals(RequestCodes.GET_ROOM_LIST.getValue())) {
            System.out.println("Refresh Room List");
            roomBriefCollection = lobbyController.getBriefRoomList();

            if (roomBriefCollection != null) {
                res = builder.setRequestCode(message.getRequestCode())
                        .setResponseCode(ResponseCodes.SUCCESS.getValue())
                        .setUserID(message.getUserID())
                        .addAllRoomList(roomBriefCollection)
                        .build();
            } else {
                res = builder.setRequestCode(message.getRequestCode())
                        .setResponseCode(ResponseCodes.SUCCESS.getValue())
                        .setUserID(message.getUserID())
                        .build();
            }

            ctx.writeAndFlush(res);
        } else if (requestCode.equals(RequestCodes.CREATE_ROOM.getValue())) {
            //System.out.println("Create room");
            Room room =lobbyController.createRoom(message.getUserID(), message.getRoomInfo().getRoomName(),
                    message.getRoomInfo().getRoomLanguage(), message.getRoomInfo().getThemeListList(), ctx.channel());

            roomInfo = roomInfoBuilder.setRoomID(room.getRoomID())
                    .setRoomLanguage(room.getLanguage().getValue())
                    .setRoomHostID(room.getHostID())
                    .setRoomName(room.getRoomName())
                    .addAllPlayerList(room.getPlayerInfoMap().values())
                    .build();

            res = builder.setRequestCode(message.getRequestCode())
                    .setResponseCode(ResponseCodes.SUCCESS.getValue())
                    .setUserID(message.getUserID())
                    .setRoomInfo(roomInfo)
                    .build();

            ctx.writeAndFlush(res);
        } else if(requestCode.equals(RequestCodes.ENTER_ROOM.getValue())) {

            /*Enter room, reply to the user and broadcast room status */
            lobbyController.joinRoom(message.getUserID(), message.getRoomInfo().getRoomID(), ctx.channel());

            Room room = lobbyController.getRoomByID(message.getRoomInfo().getRoomID());
            inRoomController.updateStatusToAll(room, message.getUserID(), RequestCodes.ENTER_ROOM, ResponseCodes.SUCCESS);
            /* try uniform status updater*/
            /*
            roomInfo = roomInfoBuilder.setRoomID(room.getRoomID())
                    .setRoomLanguage(room.getLanguage().getValue())
                    .setRoomHostID(room.getHostID())
                    .setRoomName(room.getRoomName())
                    .addAllPlayerList(room.getPlayerInfoMap().values())
                    .build();

            res = builder.setRequestCode(message.getRequestCode())
                    .setResponseCode(ResponseCodes.SUCCESS.getValue())
                    .setUserID(message.getUserID())
                    .setRoomInfo(roomInfo)
                    .build();
            room.roomBroadCast(res);
             */
        } else if(requestCode.equals(RequestCodes.GET_GLOBAL_THEME_LIST.getValue())) {
            roomInfo = roomInfoBuilder
                    .addAllThemeList(themeManager.getThemeList())
                    .build();

            res = builder.setRequestCode(message.getRequestCode())
                    .setResponseCode(ResponseCodes.SUCCESS.getValue())
                    .setUserID(message.getUserID())
                    .setRoomInfo(roomInfo)
                    .build();

            ctx.writeAndFlush(res);
        }

        ctx.fireChannelRead(message);

    }
}
