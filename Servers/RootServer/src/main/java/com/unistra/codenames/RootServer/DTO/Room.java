package com.unistra.codenames.RootServer.DTO;

import com.unistra.codenames.RootServer.Proto.Build.RawMessageProto;
import com.unistra.codenames.RootServer.Server.Codes.ResponseCodes;
import io.netty.channel.group.ChannelGroup;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.Setter;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

@AllArgsConstructor
@Getter
@Setter
public class Room {
    @AllArgsConstructor
    @Getter
    public enum Language {
        FRENCH(0),
        ENGLISH(1);

        Integer value;

        private static Map<Integer, Language> map = new HashMap<>();
        static {
            for (Language item : Language.values()) {
                map.put(item.getValue(), item);
            }
        }
        public static Language parseFromInteger(int integer) {
            return map.get(integer);
        }
    }

    private Integer roomID;
    private String roomName;
    private Boolean inGame;
    private Integer hostID;
    private Language language;
    private int timerValue;

    private Integer blueTeamCount;
    private Integer blueDetectiveCount;
    private Integer redTeamCount;
    private Integer redDetectiveCount;

    private Integer blueLastDetective;
    private Integer redLastDetective;
    private Integer blueLastAgent;
    private Integer redLastAgent;

    private List<PlayerProperties> players;
    private List<String> themes;
    private List<Card> cards;
    private List<RawMessageProto.RawMessage.Card> cardInfoList;
    private Map<Integer, RawMessageProto.RawMessage.PlayerInfo> playerInfoMap;

    private ChannelGroup channelGroup;
    private ChannelGroup detectiveChannelGroup;

    public void clearPlayerOrder() {
        this.blueLastAgent = -1;
        this.redLastAgent = -1;
        this.blueLastDetective = -1;
        this.redDetectiveCount = -1;
    }

    public void addUser(PlayerProperties pp)
    {
        RawMessageProto.RawMessage.PlayerInfo.Builder ppBuilder = RawMessageProto.RawMessage.PlayerInfo.newBuilder();
        RawMessageProto.RawMessage.PlayerInfo playerInfo = ppBuilder.setPlayerID(pp.getIdUser())
                .setPseudo(pp.getPseudo())
                .setNbVictory(pp.getNb_partie_gagnee())
                .setNbBlackCard(pp.getNb_carte_noir())
                .setIdentity(pp.getRole().getValue())
                .setIsHisTurn(pp.getIsHisTurn())
                .setIsHost(pp.getIsHost())
                .setLabel(pp.getLabel().getValue())
                .setTeam(pp.getTeam().getValue())
                .build();
        playerInfoMap.put(playerInfo.getPlayerID(), playerInfo);
        players.add(pp);
        channelGroup.add(pp.getChannel());
        if (pp.getTeam() == PlayerProperties.Team.BLUE) {
            this.blueTeamCount++;
        } else {
            this.redTeamCount++;
        }
        if (pp.getRole().equals(PlayerProperties.Role.DETECTIVE)) {
            detectiveChannelGroup.add(pp.getChannel());
            if (pp.getTeam() == PlayerProperties.Team.BLUE) {
                this.blueDetectiveCount++;
            } else {
                this.redDetectiveCount++;
            }
        }
    }

    public Integer getPlayerQuantity() {
        return players.size();
    }

    public void removeUser(PlayerProperties pp) {
        boolean isHost = pp.getIsHost();
        channelGroup.remove(pp.getChannel());
        if (pp.getRole().equals(PlayerProperties.Role.DETECTIVE))
            detectiveChannelGroup.remove(pp.getChannel());
        playerInfoMap.remove(pp.getIdUser());
        players.remove(pp);

        if (pp.getTeam().equals(PlayerProperties.Team.BLUE)) {
            if (pp.getRole().equals(PlayerProperties.Role.DETECTIVE))
                this.blueDetectiveCount--;
            this.blueTeamCount--;
        } else {
            if (pp.getRole().equals(PlayerProperties.Role.DETECTIVE))
                this.redDetectiveCount--;
            this.redTeamCount--;
        }


        if (isHost && this.getPlayerQuantity()>=1) {
            RawMessageProto.RawMessage.PlayerInfo.Builder pBuilder= RawMessageProto.RawMessage.PlayerInfo.newBuilder();
            players.get(0).setIsHost(true);
            int nextHostID = players.get(0).getIdUser();
            playerInfoMap.replace(nextHostID, pBuilder.mergeFrom(playerInfoMap.get(nextHostID)).setIsHost(true).build());
        }
    }

    public ResponseCodes roomBroadCast(RawMessageProto.RawMessage message) {
        try {
            channelGroup.forEach(ch -> ch.writeAndFlush(message));
        } catch (Exception e) {
            e.printStackTrace();
            return ResponseCodes.FAIL;
        }
        return ResponseCodes.SUCCESS;
    }

    public ResponseCodes detectiveBroadCast(RawMessageProto.RawMessage message) {
        try {
            detectiveChannelGroup.forEach(ch -> ch.writeAndFlush(message));
        } catch (Exception e) {
            e.printStackTrace();
            return ResponseCodes.FAIL;
        }
        return ResponseCodes.SUCCESS;
    }
}
