namespace ClientNetworkModule.Codes
{
    public enum RequestCode: uint
    {
        LOGIN = 100,
        REGISTER =101,

        GET_ROOM_LIST = 200,
        REFRESH_ROOM_LIST = 201,
        GET_GLOBAL_THEME_LIST = 202,

        ENTER_ROOM = 300,
        CREATE_ROOM = 301,
        QUIT_ROOM = 302,
        REFRESH_ROOM = 303,
        SET_TIMER = 304,
        CHANGE_IDENTITY = 305, // Change identity (Detective/Agent)
        CHANGE_TEAM = 306, // Change team
        GET_THEMES = 307,
        SET_THEMES = 308,


        START_GAME = 400,
        PLAY_AGENT = 401,
        PLAY_DETECTIVE = 402,
        MSG_AGENT = 403,
        MSG_DETECTIVE = 404,
        REFRESH_GAME = 405,
        GAME_OVER = 406
    }
}