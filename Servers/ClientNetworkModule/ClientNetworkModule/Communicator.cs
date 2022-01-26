using System;
using System.Collections.Generic;
using System.Net.Sockets;
using ClientNetworkModule.Codes;
using Google.Protobuf;

namespace ClientNetworkModule
{
    public class Communicator
    {
        public TcpClient client;
        public NetworkStream stream;

        public Communicator(string hostname, int port)
        {
            this.client = new TcpClient(hostname, port);
            this.stream = this.client.GetStream();
            this.stream.ReadTimeout = 5000;
        }

        
        /// <param name="pseudo"> The pseudo of new user </param>
        /// <param name="password"> The password of new user </param>
        /// <returns>A response which is an instance of the ProtoMessage class <c>RewMessage</c></returns>
        /// Check "userID" and "responseCode" .
        public RawMessage Register(string pseudo, string password)
        {
            RawMessage registerMessage = new RawMessage
            {
                RequestCode = (uint)RequestCode.REGISTER,
                Pseudo = pseudo,
                Password = password
            };
            
            SendPackage(registerMessage);
            RawMessage response = ReceivePackage();

            return response;
        }
        
        /// <param name="pseudo"> The pseudo of new user </param>
        /// <param name="password"> The password of new user </param>
        /// <returns>A response which is an instance of the class <c>RewMessage</c></returns>
        /// Check userID, roomList, responseCode, requestCode(if you need).
        /// Take care, roomList might be empty or even null. Check if it is empty before extracting messages.
        /// roomList is a list of RoomBrief objects. Check each element to get corresponding information about each existing room.
        public RawMessage Login(string pseudo, string password)
        {
            RawMessage loginMessage = new RawMessage
            {
                RequestCode = (uint)RequestCode.LOGIN,
                Pseudo = pseudo,
                Password = password
            };
           
            SendPackage(loginMessage);
            RawMessage response = ReceivePackage();

            return response;
        }
        
        /// <param name="userID"> The userID </param>
        /// <returns>A response which is an instance of the class <c>RewMessage</c></returns>
        /// Check userID, roomList, responseCode, requestCode(if you need).
        /// Take care, roomList might be empty or even null. Check if it is empty before extracting messages.
        /// roomList is a list of RoomBrief objects. Check each element to get corresponding information about each existing room.
        public RawMessage RefreshRoomList(uint userID)
        {
            RawMessage rrlMessage = new RawMessage
            {
                RequestCode = (uint) RequestCode.REFRESH_ROOM_LIST,
                UserID = userID
            };
            SendPackage(rrlMessage);
            return ReceivePackage();
        }

        /// <param name="userID"> The userID </param>
        /// <param name="roomName"> The name of the room </param>
        /// <param name="themes"> A string list containing the themes </param>
        /// <returns>A response which is an instance of the class <c>RewMessage</c></returns>
        /// Check userID, roomInfo, responseCode, requestCode(if you need).
        /// roomInfo sub-message(sub-class) defined in RawMessage.proto(.cs). If the room is not in game,
        /// its component "cardList" should be empty or even null. So, take care.
        /// If you don't wanna specify themes when creating the room, use an empty String list. Normally, it works just fine.
        public RawMessage CreateRoom(uint userID, String roomName, List<String> themes)
        {
            RawMessage crMessage = new RawMessage
            {
                RequestCode = (uint)RequestCode.CREATE_ROOM,
                RoomInfo = new RawMessage.Types.RoomInfo
                {
                    RoomName = roomName,
                    ThemeList = {themes}
                },
                UserID = userID
            };
           
            SendPackage(crMessage);
            RawMessage response = ReceivePackage();

            return response;
        }
        
        /// <param name="userID"> The userID </param>
        /// <param name="roomNID"> The roomID </param>
        /// <returns>A response which is an instance of the class <c>RewMessage</c></returns>
        /// Check userID, roomInfo, responseCode, requestCode(if you need).
        /// roomInfo sub-message(sub-class) defined in RawMessage.proto(.cs). If the room is not in game,
        /// its component "cardList" should be empty or even null. So, take care.
        public RawMessage EnterRoom(uint userID, uint roomID)
        {
            RawMessage erMessage = new RawMessage
            {
                RequestCode = (uint)RequestCode.ENTER_ROOM,
                RoomInfo = new RawMessage.Types.RoomInfo
                {
                    RoomID = roomID,
                },
                UserID = userID
            };
           
            SendPackage(erMessage);
            RawMessage response = ReceivePackage();

            return response;
        }

        /// <param name="userID"> The userID </param>
        /// <returns>A list of string containing all themes <c>RewMessage</c></returns>
        /// Unless something is wrong, this list should never be empty 
        public List<String> GetGlobalThemeList(uint userID)
        {
            RawMessage ggtMessage = new RawMessage
            {
                RequestCode = (uint)RequestCode.GET_GLOBAL_THEME_LIST,
                UserID = userID
            };
           
            SendPackage(ggtMessage);
            RawMessage response = ReceivePackage();
            return new List<String>(response.RoomInfo.ThemeList);
        }
        
        /// <param name="userID"> The host's userID </param>
        /// <returns>A response which is an instance of the class <c>RewMessage</c></returns>
        /// Check userID, roomInfo, responseCode.
        /// roomInfo sub-message(sub-class) defined in RawMessage.proto(.cs).
        /// If succeed, a message will be sent to each player containing:
        ///     Request Code: START_GAME, Response Code:  GAME_START, userID: Host's userID, roomInfo: latest roomInfo
        ///     which is the current status of the room
        /// This function should be called only by a host, or it always fails on the serverside by responding a
        /// response code: FAIL
        /// Check userID, roomInfo, responseCode, requestCode.
        /// !!! Important !!!
        /// In roomInfo.playerList , you can find all players' info list. That info includes If it's a very player's turn to play.
        public RawMessage StartGame(uint userID)
        {
            RawMessage sgMessage = new RawMessage
            {
                RequestCode = (uint)RequestCode.START_GAME,
                UserID = userID
            };
           
            SendPackage(sgMessage);
            RawMessage response = ReceivePackage();

            return response;
        }

        /// <param name="userID"> The host's userID </param>
        /// <param name="themes"> A string list containing the themes </param>
        /// <returns>A response which is an instance of the class <c>RewMessage</c></returns>
        /// Check userID, roomInfo, responseCode.
        /// roomInfo sub-message(sub-class) defined in RawMessage.proto(.cs).
        /// If succeed, a message will be sent to each player containing:
        ///     Request Code: SET_THEMES, Response Code:  ROOM_SETTING, userID: Host's userID, roomInfo: latest roomInfo
        ///     which is the current status of the room
        /// This function should be called only by a host, or it always fails on the serverside by responding a
        /// response code: FAIL
        /// Check userID, roomInfo, responseCode, requestCode.
        public RawMessage SetThemes(uint userID, List<String> themes)
        {
            RawMessage crMessage = new RawMessage
            {
                RequestCode = (uint)RequestCode.SET_THEMES,
                RoomInfo = new RawMessage.Types.RoomInfo
                {
                    ThemeList = {themes}
                },
                UserID = userID
            };
           
            SendPackage(crMessage);
            RawMessage response = ReceivePackage();

            return response;
        }
        
        /// <param name="userID"> The host's userID </param>
        /// <param name="themes"> A string list containing the themes </param>
        /// <returns>A response which is an instance of the class <c>RewMessage</c></returns>
        /// Check userID, roomInfo, responseCode.
        /// roomInfo sub-message(sub-class) defined in RawMessage.proto(.cs).
        /// If succeed, a message will be sent to each player containing:
        ///     Request Code: SET_TIMER, Response Code:  ROOM_SETTING, userID: Host's userID, roomInfo: latest roomInfo
        ///     which is the current status of the room
        /// This function should be called only by a host, or it always fails on the serverside by responding a
        /// response code: FAIL
        /// Check userID, roomInfo, responseCode, requestCode.
        public RawMessage SetTimer(uint userID, uint timerValue)
        {
            RawMessage stMessage = new RawMessage
            {
                RequestCode = (uint)RequestCode.SET_TIMER,
                RoomInfo = new RawMessage.Types.RoomInfo
                {
                    TimerValue = timerValue
                },
                UserID = userID
            };
           
            SendPackage(stMessage);
            RawMessage response = ReceivePackage();

            return response;
        }

        /// <param name="userID"> The host's userID </param>
        /// <param name="roomID"> The roomID </param>
        /// <param name="cardIndex"> The index of card </param>
        /// <param name="pass"> If the player passes and turns none card (If pass, fill any positive int as cardIndex is fine </param>
        /// <returns>A response which is an instance of the class <c>RewMessage</c></returns>
        /// Check userID, roomInfo, responseCode.
        /// roomInfo sub-message(sub-class) defined in RawMessage.proto(.cs).
        /// If succeed, a message will be sent to each player containing:
        ///     Request Code: PLAY_AGENT, Response Code:  {???} , userID: Host's userID, roomInfo: latest roomInfo
        ///     which is the current status of the room, action: the agent's action
        /// This function should be called only by an agent, or it always fails on the serverside by responding a
        /// response code: FAIL
        /// Check userID, roomInfo, responseCode, requestCode.
        /// !!! Important !!!
        /// Response Code could be : IN_GAME or GAME_OVER make sure you do check it
        /// !!! Important !!!
        /// In roomInfo.playerList , you can find all players' info list. That info includes If it's a very player's turn to play.
        public RawMessage AgentPlay(uint userID, uint roomID, uint cardIndex, bool pass)
        {
            uint quantity = 1;
            if (pass)
                quantity = 0;
            
            RawMessage stMessage = new RawMessage
            {
                RequestCode = (uint)RequestCode.PLAY_AGENT,
                UserID = userID,
                Action = new RawMessage.Types.Action
                {
                    RoomID = roomID,
                    CardQuantity = quantity,
                    CardIndex = {cardIndex}
                }
            };
           
            SendPackage(stMessage);
            RawMessage response = ReceivePackage();

            return response;
        }

        /// <param name="userID"> The host's userID </param>
        /// <param name="roomID"> The roomID </param>
        /// <param name="msg"> The detective's hint </param>
        /// <returns>A response which is an instance of the class <c>RewMessage</c></returns>
        /// Check userID, action.msg, action.roomID, requestCode, responseCode, roomInfo.
        /// This function updates the latest roomInfo and sends the detective's action.message to each player
        /// Check roomInfo.playList to determine the next player
        /// roomInfo sub-message(sub-class) defined in RawMessage.proto(.cs).
        public RawMessage DetectivePlay(uint userID, uint roomID, String msg)
        {
            RawMessage stMessage = new RawMessage
            {
                RequestCode = (uint)RequestCode.PLAY_DETECTIVE,
                UserID = userID,
                Action = new RawMessage.Types.Action
                {
                    RoomID = roomID,
                    Msg = msg
                }
            };
           
            SendPackage(stMessage);
            RawMessage response = ReceivePackage();

            return response;
        }

        /// <param name="userID"> The host's userID </param>
        /// <param name="roomID"> The roomID </param>
        /// <param name="msg"> The agent's chatting message </param>
        /// <returns>A response which is an instance of the class <c>RewMessage</c></returns>
        /// Check userID, action.msg, action.roomID, requestCode, responseCode
        /// This function simply sends the detective's action.message to each player
        /// It won't change any status of the game
        public RawMessage ChatAgent(uint userID, uint roomID, String msg)
        {
            RawMessage stMessage = new RawMessage
            {
                RequestCode = (uint)RequestCode.MSG_AGENT,
                UserID = userID,
                Action = new RawMessage.Types.Action
                {
                    RoomID = roomID,
                    Msg = msg
                }
            };
           
            SendPackage(stMessage);
            RawMessage response = ReceivePackage();

            return response;
        }
        
        /// <param name="userID"> The host's userID </param>
        /// <param name="roomID"> The roomID </param>
        /// <param name="msg"> The detective's chatting message </param>
        /// <returns>A response which is an instance of the class <c>RewMessage</c></returns>
        /// Check userID, action.msg, action.roomID, requestCode, responseCode
        /// This function simply sends the detective's action.message to each detective(none agents will receive this message)
        /// It won't change any status of the game
        public RawMessage ChatDetective(uint userID, uint roomID, String msg)
        {
            RawMessage stMessage = new RawMessage
            {
                RequestCode = (uint)RequestCode.MSG_DETECTIVE,
                UserID = userID,
                Action = new RawMessage.Types.Action
                {
                    RoomID = roomID,
                    Msg = msg
                }
            };
           
            SendPackage(stMessage);
            RawMessage response = ReceivePackage();

            return response;
        }

        /// <param name="userID"> The host's userID </param>
        /// <returns>Boolean: True if operation succeeds, else, False  <c>RewMessage</c></returns>
        public bool QuitRoom(uint userID)
        {
            RawMessage qrMessage = new RawMessage
            {
                RequestCode = (uint)RequestCode.QUIT_ROOM,
                UserID = userID
            };
           
            SendPackage(qrMessage);
            RawMessage response = ReceivePackage();

            return (response.ResponseCode == (uint)ResponseCode.SUCCESS);
        }
        
        /// <summary>
        /// It shuts down the whole connection. Call this function before shutting down the program or logging out.
        /// </summary>
        public void ShutDown()
        {
            stream.Close();
            client.Close();
        }
        
        public RawMessage ReceivePackage()
        {
            Byte[] data = new Byte[512];
            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = stream.Read(data, 0, data.Length);
            RawMessage message = RawMessage.Parser.ParseFrom(data, 0, bytes);
            return message;
        }
        
        /* Private methods */
        private void SendPackage(RawMessage msg)
        {
            Byte[] data = msg.ToByteArray();
            stream.Write(data, 0, data.Length);
        }
        
    }
}