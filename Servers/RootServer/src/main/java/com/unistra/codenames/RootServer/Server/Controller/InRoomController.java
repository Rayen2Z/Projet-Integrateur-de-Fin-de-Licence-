package com.unistra.codenames.RootServer.Server.Controller;

import com.google.protobuf.Message;
import com.unistra.codenames.RootServer.DTO.Card;
import com.unistra.codenames.RootServer.DTO.PlayerProperties;
import com.unistra.codenames.RootServer.DTO.Room;
import com.unistra.codenames.RootServer.Proto.Build.RawMessageProto;
import com.unistra.codenames.RootServer.Server.Codes.RequestCodes;
import com.unistra.codenames.RootServer.Server.Codes.ResponseCodes;
import com.unistra.codenames.RootServer.Server.Service.CardManager;
import com.unistra.codenames.RootServer.Server.Service.RoomManager;
import com.unistra.codenames.RootServer.Server.Service.ThemeManager;
import com.unistra.codenames.RootServer.Server.Service.UserManager;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;

import java.util.ArrayList;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;

@Controller
public class InRoomController {
    RoomManager roomManager;
    UserManager userManager;
    CardManager cardManager;
    ThemeManager themeManager;

    @Autowired
    InRoomController(RoomManager roomManager, UserManager userManager, CardManager cardManager, ThemeManager themeManager) {
        this.roomManager = roomManager;
        this.userManager = userManager;
        this.cardManager = cardManager;
        this.themeManager = themeManager;
    }

    /* TODO */
    public void modifySettings(Integer playerID, RawMessageProto.RawMessage.RoomInfo roomInfo) {

    }

    public Room startGameAsHost(Integer playerID) {

        final int totalCards = 25;
        PlayerProperties player = userManager.getPlayerByID(playerID);
        roomManager.updateRoomBriefStart(player.getIdRoom());
        Room room = roomManager.getRoomByID(player.getIdRoom());
        room.clearPlayerOrder();
        if (!room.getHostID().equals(playerID) || !player.getIsHost()) {
            return null;
        }

        /* Check team info */
        if (room.getPlayerQuantity() < 4) {
         return null;
        }

        Integer languageID = room.getLanguage().getValue();
        // List<Card> cardList= room.getCards();
        List<Card> cardList= new ArrayList<>(room.getCards());
        List<RawMessageProto.RawMessage.Card> cardInfoList = new ArrayList<>(room.getCardInfoList());
        List<String> themeList = new LinkedList<>(room.getThemes());

        /* Test in room player names */
        /*
        System.out.println("PlayerInfo Map:");
        System.out.println(room.getPlayerInfoMap().toString());

        System.out.println("PLayers list:");
        System.out.println(room.getPlayers().toString());
         */
        /**/
        //System.out.println("Starting Game With Themes: " + themeList.toString());
        int themeQuantity = themeList.size();
        //System.out.println("Initial Theme Size:" + themeQuantity);
        List<Integer> themeDistribution = new LinkedList<>();
        int rest = totalCards % themeQuantity;
        int avg = totalCards / themeQuantity;
        themeDistribution.add(avg + rest);
        themeQuantity--;
        while (themeQuantity > 0) {
            themeDistribution.add(avg);
            themeQuantity--;
        }
        //System.out.println("Theme distribution for each theme: " + themeDistribution.toString());
        ArrayList<String> wordList = new ArrayList<>();
        while (themeList.size() > 0) {
            //System.out.println("Theme List Size: " + themeList.size());
            String theme = themeList.remove(0);
            int cardQuantity = themeDistribution.remove(0);
            wordList.addAll(cardManager.getRandomCards(cardManager.getThemeID(theme), languageID, cardQuantity));
            //System.out.println("Latest Words: " + wordList.toString());
            themeManager.addPopularity(theme);
        }
        RawMessageProto.RawMessage.Card.Builder cardBuilder = RawMessageProto.RawMessage.Card.newBuilder();
        List<PlayerProperties> playerList = room.getPlayers();
        Map<Integer, RawMessageProto.RawMessage.PlayerInfo> playerInfoMap = room.getPlayerInfoMap();
        Card.Property nineProperties;
        Card.Property eightProperties;
        int i;

        PlayerProperties.Team startTeam =playerList.get(0).getTeam();

        for (i = 0; i < playerList.size(); i++){
            PlayerProperties pp = playerList.get(i);
            if (findNextDetective(pp, playerInfoMap, startTeam)) break;
        }

        if (startTeam == PlayerProperties.Team.BLUE) {
            nineProperties = Card.Property.BLUE;
            eightProperties = Card.Property.RED;
        } else {
            nineProperties = Card.Property.RED;
            eightProperties = Card.Property.BLUE;
        }
        /*
        for (i = 0; i < 9; i++) {
            cardList.add(new Card(wordList.get(wordList.size()-1), nineProperties, false));
            cardInfoList.add(cardBuilder.setWord(wordList.remove(wordList.size()-1)).setProperty(nineProperties.getValue()).setTurnedUp(false).build());
        }
        for (i = 0; i < 8; i++) {
            cardList.add(new Card(wordList.get(wordList.size()-1), eightProperties, false));
            cardInfoList.add(cardBuilder.setWord(wordList.remove(wordList.size()-1)).setProperty(eightProperties.getValue()).setTurnedUp(false).build());
        }
        for (i = 0; i < 7; i++) {
            cardList.add(new Card(wordList.get(wordList.size()-1), Card.Property.ANONYMOUS, false));
            cardInfoList.add(cardBuilder.setWord(wordList.remove(wordList.size()-1)).setProperty(Card.Property.ANONYMOUS.getValue()).setTurnedUp(false).build());
        }

        cardList.add(new Card(wordList.get(wordList.size()-1), Card.Property.BLACK, false));
        cardInfoList.add(cardBuilder.setWord(wordList.remove(wordList.size()-1)).setProperty(Card.Property.BLACK.getValue()).setTurnedUp(false).build());
         */


        /*************************Fake random***************************/
        cardList.add(new Card(wordList.get(wordList.size()-1), nineProperties, false));
        cardInfoList.add(cardBuilder.setWord(wordList.remove(wordList.size()-1)).setProperty(nineProperties.getValue()).setTurnedUp(false).build());
        cardList.add(new Card(wordList.get(wordList.size()-1), nineProperties, false));
        cardInfoList.add(cardBuilder.setWord(wordList.remove(wordList.size()-1)).setProperty(nineProperties.getValue()).setTurnedUp(false).build());
        for (i = 0; i < 7; i++) {
            cardList.add(new Card(wordList.get(wordList.size()-1), eightProperties, false));
            cardInfoList.add(cardBuilder.setWord(wordList.remove(wordList.size()-1)).setProperty(eightProperties.getValue()).setTurnedUp(false).build());

            cardList.add(new Card(wordList.get(wordList.size()-1), Card.Property.ANONYMOUS, false));
            cardInfoList.add(cardBuilder.setWord(wordList.remove(wordList.size()-1)).setProperty(Card.Property.ANONYMOUS.getValue()).setTurnedUp(false).build());

            cardList.add(new Card(wordList.get(wordList.size()-1), nineProperties, false));
            cardInfoList.add(cardBuilder.setWord(wordList.remove(wordList.size()-1)).setProperty(nineProperties.getValue()).setTurnedUp(false).build());
        }

        cardList.add(new Card(wordList.get(wordList.size()-1), Card.Property.BLACK, false));
        cardInfoList.add(cardBuilder.setWord(wordList.remove(wordList.size()-1)).setProperty(Card.Property.BLACK.getValue()).setTurnedUp(false).build());

        cardList.add(new Card(wordList.get(wordList.size()-1), eightProperties, false));
        cardInfoList.add(cardBuilder.setWord(wordList.remove(wordList.size()-1)).setProperty(eightProperties.getValue()).setTurnedUp(false).build());

        /***************************************************************************************************/

        playerList.forEach((p)-> p.setStatus(PlayerProperties.Status.IN_GAME));
        room.setCards(cardList);
        room.setCardInfoList(cardInfoList);
        return room;
    }

    public void updateStatusToAll(Room room, Integer callerID, RequestCodes requestCode, ResponseCodes responseCode) {
        RawMessageProto.RawMessage message;
        RawMessageProto.RawMessage.RoomInfo roomInfo;
        RawMessageProto.RawMessage.RoomInfo.Builder roomInfoBuilder = RawMessageProto.RawMessage.RoomInfo.newBuilder();
        roomInfo = roomInfoBuilder.setRoomID(room.getRoomID())
                .setRoomHostID(room.getHostID())
                .setRoomName(room.getRoomName())
                .setRoomLanguage(room.getLanguage().getValue())
                .setTimerValue(room.getTimerValue())
                .addAllPlayerList(room.getPlayerInfoMap().values())
                .addAllCardList(room.getCardInfoList())
                .addAllThemeList(room.getThemes())
                .build();

        RawMessageProto.RawMessage.Builder messageBuilder = RawMessageProto.RawMessage.newBuilder();
        message = messageBuilder.setRoomInfo(roomInfo)
                .setResponseCode(responseCode.getValue())
                .setRequestCode(requestCode.getValue())
                .setUserID(callerID)
                .build();

        room.roomBroadCast(message);
    }

    public void agentPlay(Integer playerID, RawMessageProto.RawMessage.Action action) {
        Room room = roomManager.getRoomByID(action.getRoomID());
        PlayerProperties pp = userManager.getPlayerByID(playerID);
        List<Card> cardList = room.getCards();
        List<RawMessageProto.RawMessage.Card> cardInfoList = room.getCardInfoList();
        RawMessageProto.RawMessage.Card.Builder cardBuilder = RawMessageProto.RawMessage.Card.newBuilder();
        RawMessageProto.RawMessage.PlayerInfo.Builder playerInfoBuilder = RawMessageProto.RawMessage.PlayerInfo.newBuilder();
        RawMessageProto.RawMessage.PlayerInfo playerInfo;
        int quantityCardTurned = action.getCardQuantity();
        System.out.println("Quantity of cards turned: " + quantityCardTurned);
        // List<Integer> turnedCardIndexes = action.getCardIndexList();
        // if (turnedCardIndexes == null)
        List<Integer> turnedCardIndexes = new LinkedList<>(action.getCardIndexList());
        List<PlayerProperties> playerList = room.getPlayers();
        Map<Integer, RawMessageProto.RawMessage.PlayerInfo> playerInfoMap = room.getPlayerInfoMap();

        ResponseCodes responseCodes = ResponseCodes.IN_GAME;
        PlayerProperties.Team winnerTeam = PlayerProperties.Team.BLUE;
        Boolean cardIsBlack = false;

        boolean keepPlaying = true;
        int playerIndex = playerList.indexOf(pp);
        PlayerProperties.Team team= playerList.get(playerIndex).getTeam();

        while (turnedCardIndexes.size() > 0 && quantityCardTurned > 0) {
            quantityCardTurned--;
            int cardIndex = turnedCardIndexes.remove(turnedCardIndexes.size()-1);

            Card card = room.getCards().get(cardIndex);
            card.setTurnedUp(true);
            RawMessageProto.RawMessage.Card cardInfo = room.getCardInfoList().get(cardIndex);
            room.getCardInfoList().set(cardIndex, cardBuilder.mergeFrom(cardInfo).setTurnedUp(true).build());

            Card.Property cardProperty = card.getProperty();

            // Black Card
            if (cardProperty.equals(Card.Property.BLACK)) {
                responseCodes = ResponseCodes.GAME_OVER;
                cardIsBlack = true;
                if (pp.getTeam().equals(PlayerProperties.Team.BLUE)) {
                    winnerTeam = PlayerProperties.Team.RED;
                } else {
                    winnerTeam = PlayerProperties.Team.BLUE;
                }
            }

            // Wrong card but not black
            if (!cardProperty.getValue().equals(team.getValue())) {
                keepPlaying = false;
            }
        }

        // Change team (Wrong card but not black)
        if (!keepPlaying && responseCodes.equals(ResponseCodes.IN_GAME)) {
            /* Set current player to not his turn */
            playerList.get(playerIndex).setIsHisTurn(false);
            playerInfo =playerInfoMap.get(pp.getIdUser());
            playerInfo = playerInfoBuilder.mergeFrom(playerInfo)
                    .setIsHisTurn(false)
                    .build();
            playerInfoMap.replace(pp.getIdUser(), playerInfo);

            PlayerProperties.Team nextTeam = PlayerProperties.Team.BLUE;
            int checkIndex = room.getBlueLastDetective();
            if (room.getBlueDetectiveCount() < 2)
                checkIndex = -1;
            if (team.equals(PlayerProperties.Team.BLUE)) {
                nextTeam = PlayerProperties.Team.RED;
                checkIndex = room.getRedLastDetective();
                if (room.getRedDetectiveCount() < 2)
                    checkIndex = -1;
            }

            for (int i = 0; i < playerList.size(); i++){
                if (i == checkIndex)
                    continue;
                pp = playerList.get(i);
                if (findNextDetective(pp, playerInfoMap, nextTeam)) break;
            }

        }

        // Correct card -> Check his team's card number
        final Integer[] cardQuantity = {0};
        room.getCards().forEach((c)->{
            if(!c.getTurnedUp() && c.getProperty().getValue().equals(team.getValue()))
                cardQuantity[0]++;
        });
        if (cardQuantity[0].equals(0)) {
            winnerTeam = pp.getTeam();
            responseCodes = ResponseCodes.GAME_OVER;
        }

        // Update db if "GameOver", set "game over" to room brief list
        if (responseCodes.equals(ResponseCodes.GAME_OVER)) {
            roomManager.updateRoomBriefOver(room.getRoomID());
            List<Integer> winnerIDList = new LinkedList<Integer>();
            PlayerProperties.Team finalWinnerTeam = winnerTeam;

            if (finalWinnerTeam.equals(PlayerProperties.Team.BLUE)) {
                responseCodes = ResponseCodes.VIC_BLUE;
            } else {
                responseCodes = ResponseCodes.VIC_RED;
            }

            playerList.forEach(p->{
                if (p.getTeam().equals(finalWinnerTeam)) {
                    winnerIDList.add(p.getIdUser());
                    p.setNb_partie_gagnee(p.getNb_partie_gagnee() + 1);
                }

            });

            winnerIDList.forEach(id->{
                RawMessageProto.RawMessage.PlayerInfo old = playerInfoMap.get(id);
                playerInfoMap.replace(id, playerInfoBuilder.mergeFrom(old).setNbVictory(old.getNbVictory()+1).build());
            });

            userManager.updateNBVictoryTeam(winnerIDList);

            if (cardIsBlack) {
                userManager.updateBlackCard(pp.getIdUser());
                pp.setNb_carte_noir(pp.getNb_carte_noir()+1);
                RawMessageProto.RawMessage.PlayerInfo old = playerInfoMap.get(pp.getIdUser());
                playerInfoMap.replace(pp.getIdUser(), playerInfoBuilder.mergeFrom(old).setNbBlackCard(old.getNbBlackCard()+1).build());
            }
        }

        updateStatusToAll(room, playerID, RequestCodes.PLAY_AGENT, responseCodes);
    }

    public void detectivePlay(RawMessageProto.RawMessage message) {
        RawMessageProto.RawMessage.Action action = message.getAction();

        int playerID = message.getUserID();
        int roomID = message.getAction().getRoomID();
        PlayerProperties pp = userManager.getPlayerByID(playerID);
        Room room = roomManager.getRoomByID(roomID);
        List<PlayerProperties> playerList = room.getPlayers();
        Map<Integer, RawMessageProto.RawMessage.PlayerInfo> playerInfoMap = room.getPlayerInfoMap();
        /* TODO: set plerINFO isHisTurn */

        int playerIndex = playerList.indexOf(pp);
        PlayerProperties.Team team= playerList.get(playerIndex).getTeam();

        /* Set current player to not his turn */
        playerList.get(playerIndex).setIsHisTurn(false);
        RawMessageProto.RawMessage.PlayerInfo playerInfo =playerInfoMap.get(playerID);
        RawMessageProto.RawMessage.PlayerInfo.Builder playerInfoBuilder = RawMessageProto.RawMessage.PlayerInfo.newBuilder();
        playerInfo = playerInfoBuilder.mergeFrom(playerInfo)
                .setIsHisTurn(false)
                .build();
        playerInfoMap.replace(pp.getIdUser(), playerInfo);

        PlayerProperties.Team nextTeam = PlayerProperties.Team.BLUE;
        int checkIndex = room.getBlueLastAgent();
        if ((room.getBlueTeamCount() - room.getBlueDetectiveCount()) < 2)
            checkIndex = -1;
        if (!team.equals(PlayerProperties.Team.BLUE)) {
            nextTeam = PlayerProperties.Team.RED;
            checkIndex = room.getRedLastAgent();
            if ((room.getRedTeamCount() - room.getRedDetectiveCount()) < 2)
                checkIndex = -1;
        }

        for (int i = 0; i < playerList.size(); i++){
            if (i == checkIndex)
                continue;
            pp = playerList.get(i);
            if (findNextAgent(pp, playerInfoMap, nextTeam)) break;
        }


        RawMessageProto.RawMessage.RoomInfo.Builder roomInfoBuilder = RawMessageProto.RawMessage.RoomInfo.newBuilder();
        RawMessageProto.RawMessage.RoomInfo roomInfo = roomInfoBuilder.setRoomID(room.getRoomID())
                .setRoomHostID(room.getHostID())
                .setRoomName(room.getRoomName())
                .setRoomLanguage(room.getLanguage().getValue())
                .setTimerValue(room.getTimerValue())
                .addAllPlayerList(room.getPlayerInfoMap().values())
                .addAllCardList(room.getCardInfoList())
                .addAllThemeList(room.getThemes())
                .build();

        RawMessageProto.RawMessage.Builder  msgBuilder = RawMessageProto.RawMessage.newBuilder();
        RawMessageProto.RawMessage latestMessage = msgBuilder.mergeFrom(message)
                .setResponseCode(ResponseCodes.IN_GAME.getValue())
                .setRoomInfo(roomInfo)
                .build();

        room.roomBroadCast(latestMessage);
    }

    public void agentChat(RawMessageProto.RawMessage message) {
        Room room = roomManager.getRoomByID(message.getAction().getRoomID());
        room.roomBroadCast(message);
    }

    public void detectiveChat(RawMessageProto.RawMessage message) {
        Room room = roomManager.getRoomByID(message.getAction().getRoomID());
        room.detectiveBroadCast(message);
    }

    public boolean quitRoom(Integer playerID) {
        PlayerProperties pp = userManager.getPlayerByID(playerID);
        Integer roomID = pp.getIdRoom();
        Room room = roomManager.getRoomByID(roomID);
        if (room == null)
            return false;
        if (room.getPlayerQuantity() == 2) {
            roomManager.destroyRoom(roomID);
        } else {
            room.removeUser(pp);
            updateStatusToAll(room, playerID, RequestCodes.QUIT_ROOM, ResponseCodes.USER_LEAVE);
        }
        userManager.leaveRoom(playerID);
        return true;
    }

    public RawMessageProto.RawMessage.RoomInfo getThemes() {
        RawMessageProto.RawMessage.RoomInfo.Builder roomInfoBuilder = RawMessageProto.RawMessage.RoomInfo.newBuilder();
        return roomInfoBuilder.addAllThemeList(themeManager.getThemeList()).build();

    }

    public boolean setThemes(int playerID, List<String> themes) {
        PlayerProperties pp = userManager.getPlayerByID(playerID);
        Integer roomID = pp.getIdRoom();
        Room room = roomManager.getRoomByID(roomID);
        if (room == null)
            return false;
        room.setThemes(themes);
        updateStatusToAll(room, playerID, RequestCodes.SET_THEMES, ResponseCodes.ROOM_SETTING);

        return true;
    }

    public boolean setTimer(int playerID, int timeValue) {
        PlayerProperties pp = userManager.getPlayerByID(playerID);
        Integer roomID = pp.getIdRoom();
        Room room = roomManager.getRoomByID(roomID);
        if (room == null)
            return false;
        room.setTimerValue(timeValue);
        updateStatusToAll(room, playerID, RequestCodes.SET_THEMES, ResponseCodes.ROOM_SETTING);

        return true;
    }

    private boolean findNextDetective(PlayerProperties pp, Map<Integer, RawMessageProto.RawMessage.PlayerInfo> playerInfoMap, PlayerProperties.Team nextTeam) {
        if (pp.getTeam().equals(nextTeam) && pp.getRole().equals(PlayerProperties.Role.DETECTIVE)) {
            pp.setIsHisTurn(Boolean.TRUE);
            RawMessageProto.RawMessage.PlayerInfo playerInfo =playerInfoMap.get(pp.getIdUser());
            RawMessageProto.RawMessage.PlayerInfo.Builder playerInfoBuilder = RawMessageProto.RawMessage.PlayerInfo.newBuilder();
            playerInfo = playerInfoBuilder.mergeFrom(playerInfo)
                    .setIsHisTurn(true)
                    .build();
            playerInfoMap.replace(pp.getIdUser(), playerInfo);
            return true;
        }
        return false;
    }

    private boolean findNextAgent(PlayerProperties pp, Map<Integer, RawMessageProto.RawMessage.PlayerInfo> playerInfoMap, PlayerProperties.Team nextTeam) {
        if (pp.getTeam().equals(nextTeam) && pp.getRole().equals(PlayerProperties.Role.AGENT)) {
            pp.setIsHisTurn(Boolean.TRUE);
            RawMessageProto.RawMessage.PlayerInfo playerInfo =playerInfoMap.get(pp.getIdUser());
            RawMessageProto.RawMessage.PlayerInfo.Builder playerInfoBuilder = RawMessageProto.RawMessage.PlayerInfo.newBuilder();
            playerInfo = playerInfoBuilder.mergeFrom(playerInfo)
                    .setIsHisTurn(true)
                    .build();
            playerInfoMap.replace(pp.getIdUser(), playerInfo);
            return true;
        }
        return false;
    }
    
    public boolean switchTeam(int userID) {
        PlayerProperties pp = userManager.getPlayerByID(userID);
        Room room = roomManager.getRoomByID(pp.getIdRoom());

        int oldTeam = pp.getTeam().getValue();
        int newTeam;
        // Message.Builder playerInfoBuilder = RawMessageProto.RawMessage.PlayerInfo.newBuilder();
        int max = 5;
        int maxAgent = 3;
        int maxDetective = 2;

        int oldRole = pp.getRole().getValue();
        int newRole;
        int nbDetective;
        int nbAgent;

        if (oldTeam == 0) {
            if(room.getRedTeamCount() < max) {
                newTeam = 1;
                room.setRedTeamCount(room.getRedTeamCount() +1 );
                room.setBlueTeamCount(room.getBlueTeamCount() -1 );
                nbDetective = room.getRedLastDetective();
                nbAgent = room.getRedTeamCount() - nbDetective;
                if(oldRole == 0){
                    if(nbAgent >= maxAgent)
                    {
                        newRole=1;
                        room.getDetectiveChannelGroup().add(pp.getChannel());
                        room.setRedDetectiveCount(room.getRedDetectiveCount() +1);
                    }else {
                        newRole=0;
                    }
                }else{
                    room.setBlueDetectiveCount(room.getBlueDetectiveCount() -1);
                    if(nbDetective >= maxDetective)
                    {
                        newRole=0;
                        room.getDetectiveChannelGroup().remove(pp.getChannel());
                    }else {
                        newRole=1;
                        room.setRedDetectiveCount(room.getRedDetectiveCount() +1);
                    }
                }
            }else{
                return  false;
            }
        } else {
            if(room.getBlueTeamCount() < max) {
                newTeam = 0;
                room.setRedTeamCount(room.getRedTeamCount() -1 );
                room.setBlueTeamCount(room.getBlueTeamCount() +1 );
                nbDetective = room.getBlueDetectiveCount();
                nbAgent = room.getBlueTeamCount() - nbDetective;
                if(oldRole == 0){
                    if(nbAgent >= maxAgent)
                    {
                        newRole=1;
                        room.getDetectiveChannelGroup().add(pp.getChannel());
                        room.setBlueDetectiveCount(room.getBlueDetectiveCount()+1);
                    }else {
                        newRole=0;
                    }
                }else{
                    room.setRedDetectiveCount(room.getRedDetectiveCount()-1);
                if(nbDetective >= maxDetective)
                    {
                        newRole=0;
                        room.getDetectiveChannelGroup().remove(pp.getChannel());
                    }else {
                        newRole=1;
                        room.setBlueDetectiveCount(room.getBlueDetectiveCount()+1);
                    }
                }
            }else {
                return false;
            }
        }

        if (newTeam == 0) {
            pp.setTeam(PlayerProperties.Team.BLUE);
        } else {
            pp.setTeam(PlayerProperties.Team.RED);
        }

        if (newRole == 0) {
            pp.setRole(PlayerProperties.Role.AGENT);
        } else {
            pp.setRole(PlayerProperties.Role.DETECTIVE);
        }
        //room.addUser(pp);

        Map<Integer, RawMessageProto.RawMessage.PlayerInfo> map = room.getPlayerInfoMap();

        /* oldPlayerInfo*/
        RawMessageProto.RawMessage.PlayerInfo oldPlayerInfo = map.get(userID);

        /* Builder */
        RawMessageProto.RawMessage.PlayerInfo.Builder playerInfoBuilder = RawMessageProto.RawMessage.PlayerInfo.newBuilder();

        /* Build a new playerInfo by merging from an old instance of PLayerInfo*/
        RawMessageProto.RawMessage.PlayerInfo updatePlayerInfo = playerInfoBuilder.mergeFrom(oldPlayerInfo)
                .setTeam(newTeam)
                .setIdentity(newRole)
                .build();
        map.replace(userID, updatePlayerInfo);

        updateStatusToAll(room, userID, RequestCodes.CHANGE_TEAM, ResponseCodes.SUCCESS);
        return true;
    }

    public boolean changeIdentity(int userID){
        PlayerProperties pp = userManager.getPlayerByID(userID);
        Room room = roomManager.getRoomByID(pp.getIdRoom());

        int oldRole = pp.getRole().getValue();
        int newRole;
        int oldTeam = pp.getTeam().getValue();
        int maxAgent = 3;
        int maxDetective = 2;
        int nbDetective;
        int nbAgent;
        if(oldRole == 0){
            if(oldTeam == 0){
                nbDetective = room.getBlueDetectiveCount();
                if(nbDetective >= maxDetective){
                    return false;
                }
                room.setBlueDetectiveCount(room.getBlueDetectiveCount()+1);
            }else {
                nbDetective = room.getRedLastDetective();
                if(nbDetective >= maxDetective){
                    return false;
                }
                room.setRedDetectiveCount(room.getRedDetectiveCount()+1);
            }
            room.getDetectiveChannelGroup().add(pp.getChannel());
            newRole=1;
        }else{
            if(oldTeam == 0){
                nbDetective = room.getBlueDetectiveCount();
                nbAgent = room.getBlueTeamCount() - nbDetective;
                if(nbAgent >= maxAgent){
                    return false;
                }
                room.setBlueDetectiveCount(room.getBlueDetectiveCount()-1);
            }else {
                nbDetective = room.getRedLastDetective();
                nbAgent = room.getRedTeamCount() - nbDetective;
                if(nbAgent >= maxAgent){
                    return false;
                }
                room.setRedDetectiveCount(room.getRedDetectiveCount()-1);
            }
            room.getDetectiveChannelGroup().remove(pp.getChannel());
            newRole=0;
        }

        //room.removeUser(pp);
        if (newRole == 0) {
            pp.setRole(PlayerProperties.Role.DETECTIVE);
        } else {
            pp.setRole(PlayerProperties.Role.AGENT);
        }
        //room.addUser(pp);

        Map<Integer, RawMessageProto.RawMessage.PlayerInfo> map = room.getPlayerInfoMap();

        /* oldPlayerInfo*/
        RawMessageProto.RawMessage.PlayerInfo oldPlayerInfo = map.get(userID);

        /* Builder */
        RawMessageProto.RawMessage.PlayerInfo.Builder playerInfoBuilder = RawMessageProto.RawMessage.PlayerInfo.newBuilder();

        RawMessageProto.RawMessage.PlayerInfo updatePlayerInfo = playerInfoBuilder.mergeFrom(oldPlayerInfo)
                .setIdentity(newRole)
                .build();
        map.replace(userID, updatePlayerInfo);

        updateStatusToAll(room, userID, RequestCodes.CHANGE_IDENTITY, ResponseCodes.SUCCESS);
        return true;
    }

}
