package com.unistra.codenames.RootServer.Server.Controller;

import com.unistra.codenames.RootServer.DTO.PlayerProperties;
import com.unistra.codenames.RootServer.DTO.Room;
import com.unistra.codenames.RootServer.Proto.Build.RawMessageProto;
import com.unistra.codenames.RootServer.Server.Codes.ResponseCodes;
import com.unistra.codenames.RootServer.Server.Service.RoomManager;
import com.unistra.codenames.RootServer.Server.Service.UserManager;
import io.netty.channel.Channel;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;

import java.util.Collection;
import java.util.List;

@Controller
public class LobbyController {

    RoomManager roomManager;
    UserManager userManager;

    @Autowired
    public LobbyController(RoomManager roomManager, UserManager userManager) {
        this.roomManager = roomManager;
        this.userManager = userManager;
    }

    public Collection<RawMessageProto.RawMessage.RoomBrief> getBriefRoomList() {
        return roomManager.getRoomBriefList();
    }

    public Room createRoom(Integer userID, String roomName, Integer language, List<String> themes, Channel userChannel) {
        PlayerProperties host = userManager.getPlayerByID(userID);

        if (roomName == null)
            roomName = "Room of " + host.getPseudo();
        if (language == null)
            language = 0;
        Room newRoom = roomManager.createRoom(host, roomName, Room.Language.parseFromInteger(language), themes);
        return  userManager.enterRoom(userID, newRoom.getRoomID(), userChannel, true);
    }

    public Room joinRoom(Integer userID, Integer roomID, Channel userChannel) {
        return userManager.enterRoom(userID, roomID, userChannel, false);
    }

    public Room getRoomByID(Integer roomID) {
        return roomManager.getRoomByID(roomID);
    }
}
