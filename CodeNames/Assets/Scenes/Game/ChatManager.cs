using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    [SerializeField] private Player player;
    public int maxMessages = 10000;
    public GameObject chatPanel, textObject;
    public InputField chatBox;
    
    [SerializeField] List<Message> messageList = new List<Message>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
         if (chatBox?.text != "")
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                SendMessageToChat(this.player.GetComponent<Player>().getPseudo() + ": " + chatBox.text, Message.MessageType.playerMessage);
                chatBox.text = "";
            }
        }

        
        else 
        {
            if(!chatBox.isFocused && Input.GetKeyDown(KeyCode.Return))
            chatBox.ActivateInputField();
        }
        if (!chatBox.isFocused)
            {   
                if (Input.GetKeyDown(KeyCode.Space))
                     {
                     SendMessageToChat("",Message.MessageType.info); 
                    Debug.Log("Space");   
                    }

            }
          
    }   

    public void SendMessageToChat(string text, Message.MessageType messageType){
        if (messageList.Count >= maxMessages)
        {
             Destroy(messageList[0].textObject.gameObject);
             messageList.Remove(messageList[0]);
        }   
        Message newMessage = new Message();
        newMessage.text = text;
        
        

        //
        // --- Je ne sais pas si ça marche, non testé !
        //
        RawMessage rep = null;
        if(player.getRole() == "Spymaster") {
            rep = MainMenuManager.communicator.ChatDetective(MainMenuManager.userid, Room.infopartie.RoomInfo.RoomID, text);
        } else if(player.getRole() == "Operative") {
            rep = MainMenuManager.communicator.ChatAgent(MainMenuManager.userid, Room.infopartie.RoomInfo.RoomID, text);
        }
        Debug.Log(rep);
        if(rep != null)
        {
            GameObject newText = Instantiate(textObject, chatPanel.transform);
            newMessage.textObject = newText.GetComponent<Text>();
            newMessage.textObject.text = newMessage.text;
        }
        //
        // ---
        //

        messageList.Add(newMessage);
    }

    public void AddMessageToChat(string text){
        
        if (messageList.Count >= maxMessages)
        {
             Destroy(messageList[0].textObject.gameObject);
             messageList.Remove(messageList[0]);
        }  
        Message newMessage = new Message();
        newMessage.text = text;
        
        GameObject newText = Instantiate(textObject, chatPanel.transform); 
        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = newMessage.text;

        messageList.Add(newMessage);
    }
}

[System.Serializable]
public class Message{

        public string text;
        public Text textObject;
        public MessageType messageType;

        public enum MessageType
        {
            playerMessage,
            info
        }
}