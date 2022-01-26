using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class JoinGame : MonoBehaviour
{

    [SerializeField] private Text status;
    [SerializeField] private GameObject roomListItemPrefab;
    [SerializeField] private Transform roomListParent;
    [SerializeField] private Text statuspassword;

    [SerializeField] private InputField InputField;

    [SerializeField] private TMP_InputField NewGameName;

    public static List<GameObject> listgo = new List<GameObject>();
    public static List<Room> listroom = new List<Room>();
    
    

    public void Start()
    {
        status.text = "Loading ...";
        
        for (int i = 0; i< listgo.Count;i++)
        {
            Destroy(listgo[i]);
        }



        //RawMessage response = communicator.Register("juba","123");
        //Debug.Log(response.ToString());

        
        RawMessage list = MainMenuManager.communicator.RefreshRoomList(MainMenuManager.userid);
        int nblist = list.RoomList.Count;

        JoinGame.listroom = new List<Room>();
        JoinGame.listgo = new List<GameObject>();
        for (int i = 0; i < nblist; i++)
        {
            listgo.Add(Instantiate(roomListItemPrefab, new Vector3(0, 0, 0), Quaternion.identity, roomListParent));
            

            Room room = new Room(list.RoomList[i].RoomName, list.RoomList[i].RoomLanguage,false, false, (int)list.RoomList[i].CountPlayer, list.RoomList[i].RoomID, i);
            listroom.Add(room);

            
        }
        if(nblist == 0)
        {
            status.text = "No room found, please refresh or create a new game";
        }
        else
        {
            status.text = "";
        }
    }

    public void CreateNewGame()
    {
        List<string> theme = new List<string>();
        Debug.Log("le nom de la partie " + NewGameName.text);
        RawMessage list = MainMenuManager.communicator.CreateRoom(MainMenuManager.userid,NewGameName.text,theme);
        Debug.Log("réponse : " + list.ToString());
        if(list.RoomInfo != null && list.RoomInfo.PlayerList.Count != 0)
        {
            Room.infopartie = list;
            Loader.LoadGame();
        }
    }

    public void testPassword()
    {
        //RawMessage response = communicator.EnterRoom(Loader.iduser,); 
        if(InputField.text == "123")
        {
            InputField.text = "";
            statuspassword.text = "";
            
            Loader.LoadGame();
        }
        else
        {
            statuspassword.text = "Wrong Password";
            InputField.text = "";
        }
    }



}
