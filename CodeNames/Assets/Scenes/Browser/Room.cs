using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    private string title;
    private uint language;
    private bool isplaying;
    private bool password;
    private int playerNumber;
    private int id;
    private uint idserv;
    private GameObject go;
    private Button button;  
    private static GameObject PanelPassword;
    public static RawMessage infopartie;


    public static List<int> activeThemes = new List<int>();
    //[SerializeField] public static Text NumThemes;
    
   void Start()
    {
        //NumThemes = GameObject.Find("NumThemes").GetComponent<Text>();
        PanelPassword = GameObject.Find("PanelPassword");
        PanelPassword.SetActive(false);
    }

    
    public Room(string _title, uint _language, bool _isplaying, bool _password, int _playerNumber, uint _idserv, int _id)
    {
        this.title = _title;
        this.language = _language;
        this.isplaying = _isplaying;
        this.password = _password;
        this.playerNumber = _playerNumber;
        this.id = _id;
        this.idserv = _idserv;
        this.go = JoinGame.listgo[_id];

        this.button = this.go.GetComponent<Button>();
        this.button.onClick.AddListener(delegate{ClickTheme(_id);} );

        Text[] obj = this.go.GetComponentsInChildren<Text>();
        obj[0].text = title;
        if(_isplaying)
            obj[1].text = "Playing";
        else
            obj[1].text = "Waiting";
        obj[2].text = "(" + _playerNumber.ToString() + "/10)";
        
        Image[] img = this.go.GetComponentsInChildren<Image>();
        if(!password)
        {
            
            img[1].enabled = false;
        }
        if(language == 1)
        {
            img[3].enabled = false;
            obj[3].text = "English";
        }
        else if(language == 0 )
        {
            img[2].enabled = false;
            obj[3].text = "French";
        }
        
 // get button text component in children and set the text property
        

        
    }
    public void setTitle(string _name)
    {
        this.title = _name;
    }


    public GameObject getGo()
    {
        return this.go;
    }

    public string getTitle()
    {
        return this.title;
    }

    public static void ClickTheme(int id)
    {
        Debug.Log("click " + id);
        Room room = JoinGame.listroom[id];
        if (room.password)
        {
            PanelPassword.SetActive(true);
            Text[] txt = PanelPassword.GetComponentsInChildren<Text>();
            txt[0].text = room.title;
            txt[3].text = "Enter Password";
        }
        else
        {
            
            RawMessage list = MainMenuManager.communicator.EnterRoom(MainMenuManager.userid, room.idserv);
            if(list.ResponseCode == 200 && list.RoomInfo != null && list.RoomInfo.PlayerList.Count != 0)
            {
                Debug.Log("rejoind la partie : " + list.ToString());
                infopartie = list;
                Loader.LoadGame();
            }
            
        }
        /*
        GameObject go = GameThemeManager.listgo[id];
        Theme theme = GameThemeManager.listtheme[id];
        if(theme.activated == true) {

            activeThemes.Remove(id);
            NumThemes.text = activeThemes.Count.ToString();
            theme.activated = false;
            go.transform.SetParent(GameObject.Find("themeUnActivated").transform);
            RectTransform rt = (RectTransform)GameThemeManager.listgo[id].transform;
            //rt.sizeDelta = new Vector2(5f, 0f);
            rt.SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal, 326.14f);
            rt.SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical, 27.62598f);
        }
        else {
            if(activeThemes.Count < 3)
            {
                activeThemes.Add(id);
                NumThemes.text = activeThemes.Count.ToString();
                theme.activated = true;
                go.transform.SetParent(GameObject.Find("themeActivated").transform);
                RectTransform rt = (RectTransform)GameThemeManager.listgo[id].transform;
                //rt.sizeDelta = new Vector2(-15f, 0f);
                rt.SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal, 112.23f);
                rt.SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical, 21.8f);
            }
        }
        */

        
        
    }

    public void clickClear()
    {
        List<int> cpy = new List<int>(activeThemes);
        foreach(int th in cpy) {
            ClickTheme(th);
        }
    }
}
