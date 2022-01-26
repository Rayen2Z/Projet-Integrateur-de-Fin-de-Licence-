using System;

namespace ClientNetworkModule
{
    class Program
    {
        static void Main(string[] args)
        {
            string hostname = "127.0.0.1";
            int port = 9999;
            Communicator communicator = new Communicator(hostname, port);
            

            RawMessage loginMessage = new RawMessage
            {
                RequestCode = 100,
                Pseudo = "testRegister",
                Password = "password"
            };

            RawMessage registerMessage = new RawMessage
            {
                RequestCode = 101,
                Pseudo = "testRegister",
                Password = "password"
            };
            
            /* For any request other than register/login, a validate userID is necessary */
            RawMessage refreshRoomListMessage = new RawMessage
            {
                RequestCode = 201, // Change request code to 200 for "getRoomList" request
                Pseudo = "testRegister",
                UserID = 4
            };
            
            /* For any request other than register/login, a validate userID is necessary */
            RawMessage createRoomMessage = new RawMessage
            {
                RequestCode = 301, // Change request code to 200 for "getRoomList" request
                Pseudo = "testRegister",
                UserID = 4
            };
            /*
            communication.sendPackage(loginMessage);
            RawMessage rtn = communication.receivePackage();
            
            Console.WriteLine("Rtn user id: " + rtn.UserID + " reqCode: " + rtn.RequestCode + " resCode: " + rtn.ResponseCode);
            Console.WriteLine("Room list: " + rtn.RoomList.ToString());
            
            communication.sendPackage(refreshRoomListMessage);
            rtn = communication.receivePackage();
            Console.WriteLine("Rtn user id: " + rtn.UserID + " reqCode: " + rtn.RequestCode + " resCode: " + rtn.ResponseCode);
            Console.WriteLine("Room list: " + rtn.RoomList.ToString());
            
            communication.sendPackage(createRoomMessage);
            rtn = communication.receivePackage();
            Console.WriteLine("Rtn user id: " + rtn.UserID + " reqCode: " + rtn.RequestCode + " resCode: " + rtn.ResponseCode);

            Console.WriteLine("Room Info " + rtn.RoomInfo.ToString());
            
            communication.shutDown();
            
            */

            Console.WriteLine(communicator.Register("a_new_user", "azerty"));
        }
    }
}