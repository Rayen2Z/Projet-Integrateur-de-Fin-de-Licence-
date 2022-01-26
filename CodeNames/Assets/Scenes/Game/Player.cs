using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    private int id;
    private uint id_serv;
    private string pseudo;
    private Color teamColor;
    private string role; // Operative or Spymaster
    private Color tagColor;
    private GameObject PlayerObject;
    [SerializeField] private Transform obs;
    [SerializeField] public GameObject PlayerPrefab;

    [SerializeField] private Transform blueop;
    [SerializeField] private Transform bluespy;
    [SerializeField] private Transform redop;
    [SerializeField] private Transform redspy;
    public GameObject obsparent; 

    public static Color[] tabcouleur = {Color.magenta,Color.cyan,Color.yellow};
    private Button button;
    private GameObject go;
    public static int idblue = 0;
    public static int idbluespy = 0;
    public static int idredspy = 0;
    public static int idred = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    public Player(int _id, uint _id_serv, string _pseudo, uint _color, string _role)
    {
        this.id_serv = _id_serv;
        this.id = _id;
        this.pseudo = _pseudo;
        this.role = _role;
        //0 blue
        if(_color == 0){
            this.teamColor = Color.blue;
            if(role == "Spymaster") {
                TeamManager.listgo[id].transform.SetParent(GameObject.Find("bluespy").transform);
                idbluespy++; 
            }
            else {
                TeamManager.listgo[id].transform.SetParent(GameObject.Find("blueop").transform);
                this.tagColor = tabcouleur[idblue];
                idblue++;
            }
        }
        //1 red
        else if(_color == 1){    
            this.teamColor = Color.red;
            if(role == "Spymaster") {
                TeamManager.listgo[id].transform.SetParent(GameObject.Find("redspy").transform);
                idredspy++;
            }
            else {
                TeamManager.listgo[id].transform.SetParent(GameObject.Find("redop").transform);
                this.tagColor = tabcouleur[idred];
                idred++;
            }
        }
        else
        {
            GameObject filenamefld = null;
            Transform[] trans = GameObject.Find("Observers").GetComponentsInChildren<Transform>(true);
            foreach (Transform t in trans) {
                if (t.gameObject.name == "listobs1") {
                    filenamefld = t.gameObject;
                }
            }
            TeamManager.listgo[id].transform.SetParent(filenamefld.transform);
        }
        
        
        GameObject buttonPseudoJoueur = TeamManager.listgo[id].transform.GetChild(0).GetChild(0).gameObject;
        buttonPseudoJoueur.GetComponent<TextMeshProUGUI>().text = this.pseudo;
        
        this.go = TeamManager.listgo[_id];
        this.button = this.go.transform.GetChild(0).GetComponent<Button>();
        this.button.onClick.AddListener(delegate{ClickPlayer(_id);});

        GameObject couleurJoueur = TeamManager.listgo[id].transform.Find("Image").gameObject;
        couleurJoueur.GetComponent<Image>().color = this.tagColor;
    }
    
    public GameObject getPlayerObject() {
        return this.PlayerObject;
    }

    public string getPseudo() {
        return this.pseudo;
    }

    public int getId() {
        return this.id;
    }

    public Color getTeamColor() {
        return this.teamColor;
    }

    public string getRole() {
        return this.role;
    }

    public Color getTagColor() {
        return this.tagColor;
    }

    public void changePseudo(string _pseudo) {
        this.pseudo = _pseudo;
        Debug.Log("Pseudo modifié : " + this.pseudo);
    }

    public void changeTeamColor(Color _teamColor) {
        this.teamColor = _teamColor;
        Debug.Log("Couleur d'équipe modifiée : " + this.teamColor);
    }

    public void changeRole(string _role) {
        this.role = _role;
        Debug.Log("Rôle modifié : " + this.role);
    }

    public void changeTagColor(Color _tagColor) {
        this.tagColor = _tagColor;
        GameObject couleurJoueur = TeamManager.listgo[id].transform.Find("Image").gameObject;
        couleurJoueur.GetComponent<Image>().color = this.tagColor;
        Debug.Log("Couleur de tag modifiée : " + this.tagColor);
    }
    public static void ClickPlayer(int id)
    {
        Debug.Log("click " + id);
        InterfaceManager.it.InfoButton(id);
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
