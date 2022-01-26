package com.unistra.codenames.RootServer.Server.Service;

import com.unistra.codenames.RootServer.DTO.Card;
import com.unistra.codenames.RootServer.DTO.PlayerProperties;
import com.unistra.codenames.RootServer.DTO.Room;
import com.unistra.codenames.RootServer.Proto.Build.RawMessageProto;
import io.netty.channel.Channel;
import io.netty.channel.group.DefaultChannelGroup;
import io.netty.util.concurrent.GlobalEventExecutor;
import org.springframework.stereotype.Service;

import java.util.*;

@Service
public class RoomManager {
    private static final Integer MaxDetective = 2;
    private static final Integer maxPlayer = 10;
    private static Map<Integer, Room> roomsMap = new HashMap<Integer, Room>();
    private static Map<Integer, RawMessageProto.RawMessage.RoomBrief> roomBriefMap = new HashMap<Integer, RawMessageProto.RawMessage.RoomBrief>();

    public Collection<RawMessageProto.RawMessage.RoomBrief> getRoomBriefList() {
        return roomBriefMap.values();
    }

    public Room createRoom(PlayerProperties host,String roomName, Room.Language language, List<String> themes) {
        Integer newRoomID = host.getIdUser();

        while (roomsMap.get(newRoomID) != null) {
            newRoomID = newRoomID << 2;
            if (newRoomID%1000 == 0)
                newRoomID+=1;
        }

        host.setIdRoom(newRoomID);
        host.setRole(PlayerProperties.Role.DETECTIVE);
        host.setLabel(PlayerProperties.Label.parseFromInteger(0));
        host.setTeam(PlayerProperties.Team.BLUE);
        LinkedList<String> themeList = new LinkedList<String>(themes);

        Room newRoom = new Room(newRoomID, roomName, false, host.getIdRoom(), language,30,0,
                0, 0, 0, -1, -1, -1, -1,
                new LinkedList<PlayerProperties>(), themeList, new ArrayList<Card>(),
                new ArrayList<RawMessageProto.RawMessage.Card>(), new HashMap<Integer, RawMessageProto.RawMessage.PlayerInfo>(),
                new DefaultChannelGroup(GlobalEventExecutor.INSTANCE),
                new DefaultChannelGroup(GlobalEventExecutor.INSTANCE));

        newRoom.addUser(host);
        roomsMap.put(host.getIdRoom(), newRoom);

        RawMessageProto.RawMessage.RoomBrief.Builder roomBriefBuilder = RawMessageProto.RawMessage.RoomBrief.newBuilder();
        RawMessageProto.RawMessage.RoomBrief roomBrief = roomBriefBuilder.setRoomID(host.getIdRoom())
                .setRoomHostID(host.getIdUser())
                .setRoomName(roomName)
                .setRoomLanguage(language.getValue())
                .setIsPlaying(false)
                .build();

        roomBriefMap.put(host.getIdRoom(), roomBrief);

        return newRoom;
    }

    public  Room getRoomByID(Integer roomID) {
        return roomsMap.get(roomID);
    }

    public boolean joinRoom(PlayerProperties player, Integer roomID, Channel userChannel) {
        RawMessageProto.RawMessage.RoomBrief.Builder roomBriefBuilder = RawMessageProto.RawMessage.RoomBrief.newBuilder();
        RawMessageProto.RawMessage.RoomBrief roomBrief;
        Room rm = roomsMap.get(roomID);
        if (rm.getPlayerQuantity() < maxPlayer) {

            if(rm.getBlueTeamCount() < rm.getRedTeamCount()) {
                player.setTeam(PlayerProperties.Team.BLUE);
                rm.setBlueTeamCount(rm.getBlueTeamCount() +1 );
                if (rm.getBlueDetectiveCount() < MaxDetective) {
                    player.setRole(PlayerProperties.Role.DETECTIVE);
                    rm.setBlueDetectiveCount(rm.getBlueDetectiveCount() +1);
                } else {
                    player.setRole(PlayerProperties.Role.AGENT);
                }
            } else {
                player.setTeam(PlayerProperties.Team.RED);
                rm.setRedTeamCount(rm.getRedTeamCount() + 1);
                if (rm.getRedDetectiveCount() < MaxDetective) {
                    player.setRole((PlayerProperties.Role.DETECTIVE));
                    rm.setRedDetectiveCount(rm.getRedDetectiveCount() + 1);
                } else {
                    player.setRole(PlayerProperties.Role.AGENT);
                }
            }
            player.setIsHisTurn(false);
            player.setIsHost(rm.getHostID().equals(player.getIdUser()));
            rm.addUser(player);
            rm.getChannelGroup().add(userChannel);
            roomBrief = roomBriefMap.get(roomID);
            roomBriefMap.replace(roomID, roomBriefBuilder.mergeFrom(roomBrief).setCountPlayer(rm.getPlayerQuantity()).build());
            return true;
        } else return false;
    }

    public void destroyRoom(Integer roomID) {
        roomsMap.remove(roomID);
        roomBriefMap.remove(roomID);
    }

    public void updateRoomBriefStart(Integer idRoom) {
        RawMessageProto.RawMessage.RoomBrief.Builder roomBriefBuilder = RawMessageProto.RawMessage.RoomBrief.newBuilder();
        RawMessageProto.RawMessage.RoomBrief roomBrief = roomBriefMap.get(idRoom);
        roomBriefMap.replace(idRoom, roomBriefBuilder.mergeFrom(roomBrief).setIsPlaying(true).build());
    }

    public void updateRoomBriefOver(Integer idRoom) {
        RawMessageProto.RawMessage.RoomBrief.Builder roomBriefBuilder = RawMessageProto.RawMessage.RoomBrief.newBuilder();
        RawMessageProto.RawMessage.RoomBrief roomBrief = roomBriefMap.get(idRoom);
        roomBriefMap.replace(idRoom, roomBriefBuilder.mergeFrom(roomBrief).setIsPlaying(false).build());
    }

}
