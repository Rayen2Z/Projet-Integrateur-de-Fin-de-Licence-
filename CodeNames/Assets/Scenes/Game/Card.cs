using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
    [SerializeField] private GameObject mot;
    [SerializeField] private GameObject tile;
    uint id;
    private Color couleur;
    public bool retournee;
    private string valeurMot;
    [SerializeField] public static Player player; // Pour avoir la couleur du tag
    private bool canTag;
    [SerializeField] private CardConfirmButton cardConfirmButton;
    [SerializeField] private GridManager gridManager;

    

    public Card()
    {

    }

    public Color getCouleur()
    {
        return couleur;
    }

    public void setTile(GameObject tile)
    {
        this.tile = tile;
    }

    public GameObject getTile()
    {
        return this.tile;
    }

    public void setId(int id)
    {
        this.id = (uint)id;
    }

    public uint getId()
    {
        return this.id;
    }

    public void setCanTag(bool _canTag) {
        this.canTag = _canTag;
    }

    public void setGridManager(GridManager _gridManager) {
        this.gridManager = _gridManager;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.tile = gameObject;
        this.tile.AddComponent<BoxCollider>();
        this.retournee = false;
        player = GameObject.Find("Player").GetComponent<Player>();
        this.canTag = true;

        // Bouton CONFIRM
        GameObject canvas = this.tile.transform.Find("Canvas").gameObject;
        GameObject button = canvas.transform.Find("Button").gameObject;
        button.AddComponent<CardConfirmButton>();
        this.cardConfirmButton = button.GetComponent<CardConfirmButton>();
        this.cardConfirmButton.changeCard(this as Card);
        button.GetComponent<Button>().onClick.AddListener(this.cardConfirmButton.retournerCardFromButton);
        button.SetActive(false);

        // Cercles (tags) transparents au début
        for(int i=1; i<4; i++) {
            GameObject cercle = this.tile.transform.Find("Circle" + i).gameObject;
            cercle.GetComponent<SpriteRenderer>().color = Color.clear;
        }
    }
    public static void tourner(uint id)
    {
        Card carte = GridManager.gm.tableau[id/5,id%5];
        carte.retournee = true;
        (carte.tile.transform.Find("image").gameObject).GetComponent<SpriteRenderer>().color = carte.couleur;
        for (int i = 1; i < 4; i++)
        {
            GameObject cercle = carte.tile.transform.Find("Circle" + i).gameObject;
            cercle.GetComponent<SpriteRenderer>().color = Color.clear;
        }

        //Vérification de la couleur de la carte sélectionnée
        if (carte.couleur.Equals(GridManager.noir))
        {
            Debug.Log("Carte noire retournée : DEFAITE !");
            carte.StartCoroutine(carte.deleteDelayed(1.5f)); // à garder ?
            InterfaceManager.it.Defaite.Play();
        }
        else if (carte.couleur.Equals(GridManager.rouge))
        {
            Debug.Log("Carte rouge retournée : 1 point à l'équipe rouge !");
            TeamManager.nbPointsEquipeRouge++;
            if (InterfaceManager.turnColor.Equals(Color.blue))
            {
                Debug.Log("C'est une carte de l'équipe adverse : on passe le tour !");
                InterfaceManager.it.Mauvaise.Play();
                InterfaceManager.turnColor = Color.red;
                InterfaceManager.turnRole = "Spymaster";
                InterfaceManager.it.changeTurn();
            }
            else
            {
                InterfaceManager.it.Bonne.Play();
            }
        }
        else if (carte.couleur.Equals(GridManager.bleu))
        {
            Debug.Log("Carte bleue retournée : 1 point à l'équipe bleue !");
            TeamManager.nbPointsEquipeBleue++;
            if (InterfaceManager.turnColor.Equals(Color.red))
            {
                Debug.Log("C'est une carte de l'équipe adverse : on passe le tour !");
                InterfaceManager.it.Mauvaise.Play();
                InterfaceManager.turnColor = Color.blue;
                InterfaceManager.turnRole = "Spymaster";
                InterfaceManager.it.changeTurn();
            }
            else
            {
                InterfaceManager.it.Bonne.Play();
            }
        }
        else if (carte.couleur.Equals(GridManager.gris))
        {
            Debug.Log("Carte grise (neutre) retournée : on passe le tour !");
            InterfaceManager.it.Neutre.Play();
            if (InterfaceManager.turnColor.Equals(Color.red))
            {
                InterfaceManager.turnColor = Color.blue;
                InterfaceManager.turnRole = "Spymaster";
                InterfaceManager.it.changeTurn();
            }
            else if (InterfaceManager.turnColor.Equals(Color.blue))
            {
                InterfaceManager.turnColor = Color.red;
                InterfaceManager.turnRole = "Spymaster";
                InterfaceManager.it.changeTurn();
            }
        }
        InterfaceManager.it.changeCompteur();
        //Vérification des conditions de victoire
        if (player.getTeamColor().Equals(Color.red) && TeamManager.nbPointsEquipeRouge == 9)
        { // ou 9 en fonction de qui commence : à compléter
            Debug.Log("Vous avez retourné toutes vos cartes : Victoire !");
            InterfaceManager.it.Victoire.Play();
        }
        else if (player.getTeamColor().Equals(Color.blue) && TeamManager.nbPointsEquipeBleue == 8)
        { // ou 8 en fonction de qui commence : à compléter
            Debug.Log("Vous avez retourné toutes vos cartes : Victoire !");
            InterfaceManager.it.Victoire.Play();
        }
    }

    public void retournerCard(){
        if(player.getRole() != "Spymaster" && !this.retournee){
            this.retournee = true;
            Debug.Log("Carte avec le mot " + this.valeurMot + " cliquée !");

            RawMessage update = MainMenuManager.communicator.AgentPlay(Room.infopartie.RoomInfo.RoomHostID,
                Room.infopartie.RoomInfo.RoomID,this.id,false);
            Debug.Log(update.ToString());
            if(update.ResponseCode == 302)
            {
                tourner(this.id);
            }
            
        }
    }

    IEnumerator deleteDelayed(float _temps) {
        yield return new WaitForSeconds(_temps);
        this.gridManager.delete(); // A garder ou non
    }

    public void ChangeCouleur(Color _couleur){
        this.couleur = _couleur;
    }

    public void ChangeMot(GameObject tile, string _mot) {
        this.mot = tile.transform.GetChild(0).GetChild(0).gameObject;
        this.mot.GetComponent<TextMeshProUGUI>().text = _mot;
        this.valeurMot = _mot;
    }

    public void Untag()
    {
        for(int i=1; i<4; i++) {
            GameObject cercle = this.tile.transform.Find("Circle" + i).gameObject;
            if (!cercle.GetComponent<SpriteRenderer>().color.Equals(Color.clear)) {
                cercle.GetComponent<SpriteRenderer>().color = Color.clear;
                Debug.Log("Untag Carte");
            }
        }
    }

    public void OnMouseDown()
    {
        if (this.canTag && player.getRole() != "Spymaster" && !this.retournee 
        && InterfaceManager.turnColor.CompareRGB(player.getTeamColor()) 
        && InterfaceManager.turnRole == player.getRole() 
        ) {
            // Tag
            int nbTags = 0;
            int numCercle = 0;
            bool playerColorFound = false;
            for(int i=1; i<4; i++) {
                GameObject cercle = this.tile.transform.Find("Circle" + i).gameObject;
                if (!playerColorFound && (cercle.GetComponent<SpriteRenderer>().color.Equals(Color.clear) || cercle.GetComponent<SpriteRenderer>().color.Equals(player.getTagColor()))) {
                    if (numCercle == 0) {
                        numCercle = i;
                    }
                    if (cercle.GetComponent<SpriteRenderer>().color.Equals(player.getTagColor())) {
                        playerColorFound = true;
                        numCercle = i;
                    }
                }
                if (!cercle.GetComponent<SpriteRenderer>().color.Equals(Color.clear)) {
                    nbTags++;
                }
            }
            if (numCercle>0) {
                GameObject cercle = this.tile.transform.Find("Circle" + numCercle).gameObject;
                if (cercle.GetComponent<SpriteRenderer>().color.Equals(Color.clear)) {
                    cercle.GetComponent<SpriteRenderer>().color = player.getTagColor();
                    nbTags++;
                    Debug.Log("Tag Carte");
                } else {
                    cercle.GetComponent<SpriteRenderer>().color = Color.clear;
                    nbTags--;
                    Debug.Log("Untag Carte");
                }
                GameObject canvas = this.tile.transform.Find("Canvas").gameObject;
                GameObject button = canvas.transform.Find("Button").gameObject;

                if (Color.blue.CompareRGB(player.getTeamColor())) {
                    if(nbTags >=1){// Player.idblue ){ todo
                        button.SetActive(true);
                    }
                    else {
                        button.SetActive(false);
                    }
                }
                else if (Color.red.CompareRGB(player.getTeamColor())) { //dynamique
                    if(nbTags >= 1){// Player.idred ){ todo
                        button.SetActive(true);
                    }
                    else {
                        button.SetActive(false);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
