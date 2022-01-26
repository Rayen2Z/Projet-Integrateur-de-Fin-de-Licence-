
* Play:
    * Request message components:  
      *userID, requestCode, action*
    * possible requestCode:
      ```
        PLAY_AGENT(401),PLAY_DETECTIVE(402), MSG_AGENT(403), MSG_DETECTIVE(404);
      ```
    * Response method:  
      *inRoomController.updateStatusToAll()*
    * Respond msg components:  
      *RoomInfo(full), requestCode, responseCode, userID(action user)* 
    * possible resCode: 
      ```
        IN_GAME(302), WIN_GAME(303), LOSE_GAME(304);
      ```
    * to-do for the client:  
      check reqCode and resCode, if "IN_GAME", then check whose turn it is, update client's in-game status
      