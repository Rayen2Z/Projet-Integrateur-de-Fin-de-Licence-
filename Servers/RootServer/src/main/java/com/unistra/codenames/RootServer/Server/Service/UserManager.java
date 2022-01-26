package com.unistra.codenames.RootServer.Server.Service;

import com.unistra.codenames.RootServer.DAO.Entities.UserEntity;
import com.unistra.codenames.RootServer.DAO.Repositories.UserRepository;
import com.unistra.codenames.RootServer.DTO.PlayerProperties;
import com.unistra.codenames.RootServer.DTO.Room;
import io.netty.channel.Channel;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.*;

@Service
public class UserManager {
    private UserRepository userRepository;
    private RoomManager roomManager;
    static private Map<Integer, PlayerProperties> authenticatedUsersMap = new HashMap<>();

    @Autowired
    public UserManager(RoomManager roomManager, UserRepository userRepository) {
        this.userRepository = userRepository;
        this.roomManager = roomManager;
    }

    public PlayerProperties getPlayerByID(Integer userID) {
        return authenticatedUsersMap.get(userID);
    }

    public boolean addUser(Integer userID, String pseudo, Integer nb_gagnee, Integer nb_carteNoire, Channel channel) {
        if (authenticatedUsersMap.get(userID) !=null)
            return false;

        PlayerProperties playerProperties = new PlayerProperties(userID, pseudo, nb_gagnee, nb_carteNoire, PlayerProperties.Status.IN_LOBBY, null, null, null, null, false, false,channel);
        authenticatedUsersMap.put(userID, playerProperties);
        return true;
    }

    public Room enterRoom(Integer userID, Integer roomID, Channel userChannel, boolean asHost) {

        PlayerProperties pp = getPlayerByID(userID);
        pp.setIdRoom(roomID);
        pp.setStatus(PlayerProperties.Status.In_ROOM);
        pp.setIsHost(asHost);

        Room toEnter = roomManager.getRoomByID(roomID);
        if (toEnter == null)
            return null;
        pp.setLabel(PlayerProperties.Label.parseFromInteger(roomManager.getRoomByID(roomID).getPlayerQuantity()));
        if (roomManager.joinRoom(pp, roomID, userChannel)) {
            return  roomManager.getRoomByID(roomID);
        } else {
            pp.setIdRoom(0);
            pp.setStatus(PlayerProperties.Status.IN_LOBBY);
            return null;
        }

    }

    public void leaveRoom(int userID) {
        PlayerProperties pp = getPlayerByID(userID);
        pp.setIdRoom(0);
        pp.setStatus(PlayerProperties.Status.IN_LOBBY);
        pp.setIsHost(false);
    }

    public boolean removeUser(Integer userID) {
        if (authenticatedUsersMap.get(userID) ==null)
            return false;
        authenticatedUsersMap.remove(userID);
        return true;
    }

    public boolean isConnected(Integer userID) {
        return authenticatedUsersMap.get(userID) != null;
    }

    public void updateNBVictoryTeam(List<Integer> winnerIDList) {
        Iterable<UserEntity> winnerIter = userRepository.findAllById(winnerIDList);
        winnerIter.forEach(winner->winner.setNb_partie_gagnee(winner.getNb_partie_gagnee()+1));
        userRepository.saveAll(winnerIter);
    }

    public void updateBlackCard(Integer playerID) {
        Optional<UserEntity> loser = userRepository.findById(playerID);
        UserEntity loserFound;
        if (loser.isPresent()) {
            loserFound = loser.get();
            loserFound.setNb_carte_noir(loserFound.getNb_carte_noir() + 1);
            userRepository.save(loserFound);
        }
    }
}
