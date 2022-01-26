package com.unistra.codenames.RootServer.Server.Codes;

import lombok.AllArgsConstructor;
import lombok.Getter;

import java.util.HashMap;
import java.util.Map;

@AllArgsConstructor
@Getter
public enum ResponseCodes {
    SUCCESS(200),
    FAIL(201),
    ASYNC(202),
    LOGIN_REQUESTED(203),
    UNRECOGNIZED_REQUEST(204),
    USER_NOT_EXIST(205),
    WRONG_PASSWORD(206),
    PSEUDO_DUPLICATION(207),
    GAME_START(301),
    IN_GAME(302), // In-game status continue
    GAME_OVER(303),
    USER_LEAVE(304),
    ROOM_SETTING(305),
    VIC_BLUE(306),
    VIC_RED(307);



    private Integer value;
}
