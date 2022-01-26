package com.unistra.codenames.RootServer.DTO;

import io.netty.channel.Channel;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.Getter;
import lombok.Setter;

import java.security.PublicKey;
import java.util.HashMap;
import java.util.Map;

@AllArgsConstructor
@Getter
@Setter
public class PlayerProperties {

    @AllArgsConstructor
    @Getter
    public enum Status {
        IN_GAME(0),
        IN_LOBBY(1),
        In_ROOM(2);

        private Integer value;
    }

    @AllArgsConstructor
    @Getter
    public enum Role {
        AGENT(0),
        DETECTIVE(1);

        private Integer value;
    }

    @AllArgsConstructor
    @Getter
    public enum Team {
        BLUE(0),
        RED(1);

        private Integer value;
    }

    @AllArgsConstructor
    @Getter
    enum RoomHost {
        FALSE(0),
        TRUE(1);

        private Integer value;
    }

    @AllArgsConstructor
    @Getter
    public enum Label {
        RED(0),
        YELLOW(1),
        MARRON(2),
        BLUE(3),
        GREEN(4),
        PURPLE(5),
        PINK(6),
        BROWN(7),
        GREY(8),
        BLACK(9);

        private Integer value;
        private static Map map = new HashMap<>();
        static {
            for (PlayerProperties.Label item : PlayerProperties.Label.values()) {
                map.put(item.getValue(), item);
            }
        }
        public static PlayerProperties.Label parseFromInteger(int integer) {
            return (PlayerProperties.Label) map.get(integer);
        }
    }

    private Integer idUser;
    private String pseudo;
    private Integer nb_partie_gagnee;
    private Integer nb_carte_noir;

    private Status status;
    private Integer idRoom;

    private Role role;
    private Team team;
    private Label label = Label.BLUE;
    private Boolean isHisTurn = false;
    private Boolean isHost = false;
    private Channel channel;
}
