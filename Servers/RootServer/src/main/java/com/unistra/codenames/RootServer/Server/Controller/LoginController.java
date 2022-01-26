package com.unistra.codenames.RootServer.Server.Controller;

import com.unistra.codenames.RootServer.DAO.Entities.UserEntity;
import com.unistra.codenames.RootServer.DAO.Repositories.UserRepository;
import com.unistra.codenames.RootServer.Server.Codes.ResponseCodes;
import com.unistra.codenames.RootServer.Server.Service.UserManager;
import io.netty.channel.Channel;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.Optional;

@Service
public class LoginController {
    private UserManager userManager;
    private UserRepository userRepository;
    @Autowired
    public LoginController(UserManager userManager, UserRepository userRepository) {
        this.userManager = userManager;
        this.userRepository = userRepository;
    }

    public Integer getUserIDByPseudo(String pseudo) {
        Optional<UserEntity> readUser = userRepository.findByPseudo(pseudo);
        if (readUser.isPresent())
            return readUser.get().getId_utilisateur();
        else
            return 0;
    }

    public ResponseCodes login(String pseudo, String password, Channel channel) {
        Optional<UserEntity> readUser = userRepository.findByPseudo(pseudo);
        if (readUser.isEmpty())
            return ResponseCodes.USER_NOT_EXIST;
        UserEntity user = readUser.get();
        if (user.getMdp().equals(password)) {
            userManager.addUser(user.getId_utilisateur(), pseudo, user.getNb_partie_gagnee(), user.getNb_carte_noir(), channel);
            return ResponseCodes.SUCCESS;
        }

        else
            return ResponseCodes.WRONG_PASSWORD;
    }

    public ResponseCodes register(String pseudo, String password) {
        Optional<UserEntity> oldUser = userRepository.findByPseudo(pseudo);
        if (oldUser.isPresent())
            return ResponseCodes.PSEUDO_DUPLICATION;
        UserEntity user = new UserEntity();
        user.setPseudo(pseudo);
        user.setMdp(password);
        user.setTyp('A');
        user.setNb_carte_noir(0);
        user.setNb_partie_gagnee(0);
        userRepository.save(user);
        return ResponseCodes.SUCCESS;
    }

    public boolean logOut(Integer userID) {
        return userManager.removeUser(userID);
    }

    public boolean isConnected(Integer userID) {
        return userManager.isConnected(userID);
    }
}
