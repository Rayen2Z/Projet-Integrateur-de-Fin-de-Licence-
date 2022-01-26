using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    private int nbJoueurs;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform blueop;
    public static List<GameObject> listgo = new List<GameObject>();
    public static List<Player> players = new List<Player>();
    public static int nbPointsEquipeRouge = 0;
    public static int nbPointsEquipeBleue = 0;
    public static TeamManager tm;

    [SerializeField]
    public Transform listUnActivated;

    [SerializeField] private GameObject prefabtheme;
    // Start is called before the first frame update

    string [,] tab = new string[,]  {
        { "25","moi",  "red", "Operative"},
        { "365","ngcool",  "", ""},
        { "6777","bgdu",  "red", "Operative"},
        { "7","Xxx_SnIpEr",  "red", "Spymaster"},
        { "42","jean",  "blue", "Operative"},
        { "69","le_fou_",  "blue", "Spymaster"},
    };

    public void Start()
    {
        //éviter le bug theme
        GameThemeManager.gtmstart(listUnActivated,prefabtheme);


        RawMessage info = Room.infopartie;
        for (int j = 0; j < listgo.Count; j++)
        {
            Destroy(listgo[j]);
        }
        TeamManager.tm = this;
        TeamManager.listgo = new List<GameObject>();
        TeamManager.players = new List<Player>();

        //on cherche ici à placer notre joueur en premier dans la liste pour que ce soit celui qu'on controle dans le jeu
        int i = 0;
        while(info.RoomInfo.PlayerList[i].PlayerID != MainMenuManager.userid)
        {
            i++;
        }
        RawMessage.Types.PlayerInfo Moi = info.RoomInfo.PlayerList[i];
        info.RoomInfo.PlayerList.RemoveAt(i);

        info.RoomInfo.PlayerList.Insert(0,Moi);
        
        for (int id = 0; id < info.RoomInfo.PlayerList.Count; id++)
        {
            //reception liste joueurs
            listgo.Add(Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity, GameObject.Find("redspy").transform));
            if(info.RoomInfo.PlayerList[id].Identity == null)
            {
                players.Add(new Player(id, info.RoomInfo.PlayerList[id].PlayerID, info.RoomInfo.PlayerList[id].Pseudo, info.RoomInfo.PlayerList[id].Team, ""));
            }
            else if (info.RoomInfo.PlayerList[id].Identity == 0)
            { //agent = 0
                players.Add(new Player(id, info.RoomInfo.PlayerList[id].PlayerID, info.RoomInfo.PlayerList[id].Pseudo, info.RoomInfo.PlayerList[id].Team, "Operative"));
            }
            else if(info.RoomInfo.PlayerList[id].Identity == 1)
            {
                players.Add(new Player(id, info.RoomInfo.PlayerList[id].PlayerID, info.RoomInfo.PlayerList[id].Pseudo, info.RoomInfo.PlayerList[id].Team, "Spymaster"));
            }
        }
        nbJoueurs = tab.GetLength(0);
        JoinButton.init(); // on init le joueur avec players[0]
    }

    public static Player getPlayerFromPseudo(string _pseudo) {
        for(int i=0; i<players.Count; i++) {
            if(players[i].getPseudo() == _pseudo) {
                return players[i];
            }
        }
        return null;
    }

    

    // Fonction pour afficher les noms sur le HUD

    // Update is called once per frame
    void Update()
    {
        
    }
}
