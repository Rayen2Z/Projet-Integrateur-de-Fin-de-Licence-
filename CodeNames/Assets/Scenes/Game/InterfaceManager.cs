using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InterfaceManager : MonoBehaviour
{
    [SerializeField] public Player player;
    public string LeMot;
    public string LeNombre ;
    public GameObject InputField;
    public GameObject InputFieldNombre;
    public Text AffichageMot;

    public static bool sortPop = true;
    public static bool sortName = true;
    public Text txtbuttonsortname;
    public Text txtbuttonsortPop;
    public static bool Lobby = true;
    public static bool Observer = false;
    public static bool InfoJoueur = false;
    public static bool theme = false;
    public GameObject ThemeList;
    public GameObject DoneButton;
    public GameObject ButtonHist;
    public GameObject ObserverPanel;
    public GameObject InfoJoueurPanel;
    public GameObject ParamHidePanel;
    public GameObject ButtonRandom;
    public TMP_Text ButtonText;
    public TMP_Text topInfo;
    public GameObject top;
    public GameObject ButtonJoin1;
    public GameObject ButtonJoin2;
    public GameObject ButtonJoin3;
    public GameObject ButtonJoin4;

    public GameObject WordGuess;
    public GameObject FooterSpy;
    public GameObject FooterAgent;
    public GridManager m_GridManager;

    public TMP_Text redpoints;
    public TMP_Text bluepoints;
    public AudioSource Victoire;
    public AudioSource Defaite;
    public AudioSource Neutre;
    public AudioSource Bonne;
    public AudioSource Mauvaise;

    public TMP_InputField InputFieldTimer;

    [SerializeField] public ChatManager chatManager;

    Vector3 currentPosition;

    public static Color turnColor = Color.red; 
    public static string turnRole = "Spymaster"; //Spymaster ou Operative

    public static InterfaceManager it;

    // Start is called before the first frame update
    void Start()
    {
        Theme.Start();
        Card.player = GameObject.Find("Player").GetComponent<Player>();
        RawMessage info = Room.infopartie;
        /* todo marche po
        if (info.RoomInfo.TimerValue != 0)
        {
            InputFieldTimer.text = info.RoomInfo.TimerValue.ToString();
        }*/

        //est ce que ça a commencé ? si oui a qui le trour
        if(info.RoomInfo.CardList.Count != 0)
        {
            bool trouved = false;
            int i = 0;

            while (!trouved)
            {
                if (info.RoomInfo.PlayerList[i].PlayerID == MainMenuManager.userid)
                {
                    trouved = true;
                    if(info.RoomInfo.PlayerList[i].IsHisTurn)//si c'est son tour
                    {
                        if(info.RoomInfo.PlayerList[i].Identity == 1)//detective
                        {
                            turnRole = "Spymaster";
                        }
                        else //agent
                        {
                            turnRole = "Operative";
                        }
                    }
                    else //pas son tour
                    {
                        if (info.RoomInfo.PlayerList[i].Identity == 1)//detective
                        {
                            turnRole = "Operative";
                        }
                        else //agent
                        {
                            turnRole = "Spymaster"; 
                        }
                    }
                }
                else
                {
                    i++;
                }
            }
            m_GridManager.Generate(info); // todo averif
            m_GridManager.cherchertourner(info);
            Lobby = false;
            ButtonText.text = "RESET";
            ButtonHist.SetActive(true);
            ButtonRandom.SetActive(false);


            ParamHidePanel.SetActive(true);

            //theme = false;
            //ThemeList.GetComponent<RectTransform>().localScale = new Vector3(0f, 0f, 0f);



            changeTurn();
        }
        
        

        if (info.RoomInfo.ThemeList.Count > 0)
        {
            Debug.Log("go changer les theùmes");
            Theme.UpdateTheme(info);
        }

        it = this;
        theme = true;
        currentPosition = ThemeList.GetComponent<RectTransform>().transform.localPosition;
        ThemeButton();

        InvokeRepeating("updateRes", 0, 1);
        TeamManager.tm.Start();
        Theme.UpdateTheme(Room.infopartie); //todo
        if (MainMenuManager.userid != Room.infopartie.RoomInfo.RoomHostID)
            ParamHidePanel.SetActive(true);

    }

    void updateRes()
    {
        try {
            
            RawMessage update = MainMenuManager.communicator.ReceivePackage();
            Debug.Log("-------------------------------------------------------------");
            //si on a les infos de la room

            Debug.Log(update);
            Debug.Log("-------------------------------------------------------------");
            

            //chat agent
            if (update.RequestCode == 403)
            {
                if (player.getRole() == "Operative")
                {
                    chatManager.AddMessageToChat(update.Action.Msg);
                }
            }

            //chat detective
            else if (update.RequestCode == 404)
            {
                Debug.Log("chat +++++++++++" + update.Action.Msg);
                if (player.getRole() == "Spymaster")
                {
                    chatManager.AddMessageToChat(update.Action.Msg);
                }
            }
            else if (update.RequestCode == 302 && update.ResponseCode == 200)
            {
                QuitGame.QuitRoom();
            }
            //jeu agent
            else if (update.RequestCode == 401 && update.ResponseCode == 302)
            {
                //passer le tour
                int res = GridManager.gm.cherchertourner(update);
                if ( res == 0)
                {
                    if (turnColor.CompareRGB(Color.blue))
                        turnColor = Color.red;
                    else
                        turnColor = Color.blue;

                    turnRole = "Spymaster";

                    //untag toutes les cartes
                    for (int i = 0; i < 5; i++)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            GridManager.gm.tableau[i, j].Untag();
                        }
                    }
                    changeTurn();
                }
                //une carte fu tournadé
                else
                {
                    //Card.tourner(update.Action.CardIndex[0]);
                    
                }
                
            }
            //jeu detective
            else if (update.RequestCode == 402 && update.ResponseCode == 302)
            {
                WordGuess.SetActive(true);
                AffichageMot.text = update.Action.Msg;

                turnRole = "Operative";
                changeTurn();
            }
            //start game
            else if (update.RequestCode == 400 && update.ResponseCode == 301)
            {
                //List<Card> cards = update.RoomInfo.CardList;
                m_GridManager.Generate(update); // todo update 
                Lobby = false;
                ButtonText.text = "RESET";
                ButtonHist.SetActive(true);
                ButtonRandom.SetActive(false);


                ParamHidePanel.SetActive(true);

                theme = false;
                ThemeList.GetComponent<RectTransform>().localScale = new Vector3(0f, 0f, 0f);

                changeTurn();

            }
            //set theme
            else if (update.RequestCode == 308 && update.ResponseCode == 305)
            {
                Debug.Log("go changer les theùmes");
                Theme.UpdateTheme(update);
            }
            //set timer
            else if (update.RequestCode == 304 && update.ResponseCode == 305)
            {
                InputFieldTimer.text = update.RoomInfo.TimerValue.ToString();
            }
            else
            {
                if (update.RoomInfo.PlayerList.Count > 0)
                {
                    Room.infopartie = update;
                    TeamManager.tm.Start();
                }
            }
            
        }
        catch(Exception e) { }
        

    }

    // Update is called once per frame
    void Update()
    {
        // Update is called once per frame
    }


    public void ChangeButton()
    {
        /*
        if(!(
           Player.idblue > 0 
        && Player.idred > 0 
        && Player.idbluespy > 0 
        && Player.idredspy > 0 
        && Theme.activeThemes.Count > 0))
        {
            Debug.Log("Game is not ready to launch");
            return;
        }*/

        if (Lobby == true)
        {
            if (MainMenuManager.userid == Room.infopartie.RoomInfo.RoomHostID)
            {
                RawMessage update = MainMenuManager.communicator.StartGame(MainMenuManager.userid);
                Debug.Log("lancement : --------------------------------------");
                Debug.Log(update);
                if(update.ResponseCode == 301)
                {
                    //Partie commence
                    m_GridManager.Generate(update);
                    Lobby = false;
                    ButtonText.text = "RESET";
                    ButtonHist.SetActive(true);
                    ButtonRandom.SetActive(false);


                    ParamHidePanel.SetActive(true);

                    theme = false;
                    ThemeList.GetComponent<RectTransform>().localScale = new Vector3(0f, 0f, 0f);

                    changeTurn();
                }
            }
            else
            {
                Debug.Log("pas hote");
            }
            changeCompteur();


        }
        else
        {
            //lobby
            if (MainMenuManager.userid == Room.infopartie.RoomInfo.RoomHostID)
            {
                //todo
                //RawMessage update = MainMenuManager.communicator.StartGame(MainMenuManager.userid);
            }
            m_GridManager.delete();
            WordGuess.SetActive(false);
            Lobby = true;
            ButtonText.text = "START";
            ButtonHist.SetActive(false);
            ButtonRandom.SetActive(true);
            
            ButtonJoin1.SetActive(true);
            ButtonJoin2.SetActive(true);
            ButtonJoin3.SetActive(true);
            ButtonJoin4.SetActive(true);

            if (MainMenuManager.userid == Room.infopartie.RoomInfo.RoomHostID)
                ParamHidePanel.SetActive(false);
            else
                ParamHidePanel.SetActive(true);

            DoneButton.SetActive(false);

            FooterSpy.SetActive(false);
            FooterAgent.SetActive(false);

            TeamManager.nbPointsEquipeRouge = 0;
            TeamManager.nbPointsEquipeBleue = 0;
            
        }
    }

    //fonction qui reload l'interface en fonction de turncolor et turnrole
    public void changeTurn()
    {
        //changement top info
        if(turnColor.CompareRGB(Color.blue))
        {
            top.GetComponent<RawImage>().color = GridManager.bleu;
            if(turnRole == "Operative")
            {
                topInfo.text = "Blue Operative guesses";
                
            }
            else
            {
                topInfo.text = "Blue Spymaster's turn";
                WordGuess.SetActive(false);
            }
        }
        else
        {
            top.GetComponent<RawImage>().color = GridManager.rouge;
            if(turnRole == "Operative")
                topInfo.text = "Red Operative guesses";
            else
            {
                topInfo.text = "Red Spymaster's turn";
                WordGuess.SetActive(false);
            }
        }

        //si pas observer on enlève les join
        
        if (this.player.getRole() != null && this.player.getRole() != ""  )
        {
            ButtonJoin1.SetActive(false);
            ButtonJoin2.SetActive(false);
            ButtonJoin3.SetActive(false);
            ButtonJoin4.SetActive(false);
        }
        

        //défini si c'est ton tour et affiche en conséquence
        if (this.player.getRole() == null ||  !turnColor.CompareRGB(this.player.getTeamColor()) || turnRole != this.player.getRole())
            {
                Debug.Log("pas ton tour");
                FooterSpy.SetActive(false);
                FooterAgent.SetActive(false);
            }

            else if (this.player.getRole() == "Operative" )
            {
                Debug.Log("ton tour");
                FooterSpy.SetActive(false);
                FooterAgent.SetActive(true);
            }

            else if (this.player.getRole() == "Spymaster")
            {
                Debug.Log("ton tour");
                FooterSpy.SetActive(true);
                FooterAgent.SetActive(false);
                for (int row = 0; row < 5; row++)
                {
                    for (int col = 0; col < 5; col++)
                    {
                        (m_GridManager.tableau[row,col].getTile().transform.Find("image").gameObject).GetComponent<SpriteRenderer>().color = m_GridManager.tableau[row,col].getCouleur() ;
                    }
                }
            }
    }
    public void ObserverButton()
    {
        if ( Observer == true)
        {
            //fermer observer
            Observer = false;
            ObserverPanel.SetActive(false);
        }
        else
        {
            //ouvrir observer
            Observer = true;
            ObserverPanel.SetActive(true);
        }
    }

    public void InfoButton(int id)
    {
        if (InfoJoueur == false)
            InfoJoueur = true;
        else
            InfoJoueur = false;
        InfoJoueurPanel.SetActive(InfoJoueur);
    }

    public void ThemeButton()
    {
        if (theme == false)
        {
            theme = true;
            ThemeList.GetComponent<RectTransform>().localPosition = currentPosition;
        }
        else
        {
            theme = false;
            ThemeList.GetComponent<RectTransform>().localPosition = new Vector3(1000, 1000);
        }
    }

    public void NameSortButton()
    {
        GameThemeManager.gtm.trierName(sortName);
        if(sortName)
        {
            
            sortName = false;
            txtbuttonsortname.text = "∨";
            
        }
        else
        {
            sortName = true;
            txtbuttonsortname.text = "∧";
        }
    }

    public void PopSortButton()
    {
        GameThemeManager.gtm.trierpop(sortPop);
        if(sortPop)
        {
            
            sortPop = false;
            txtbuttonsortPop.text = "∨";
            
        }
        else
        {
            sortPop = true;
            txtbuttonsortPop.text = "∧";
        }
    }
    // affichage mot a deviner 
    public void affiche()
    {
        TMP_InputField inputFieldCo = InputField.GetComponent<TMP_InputField>();
        TMP_InputField inputFieldCoNombre = InputFieldNombre.GetComponent<TMP_InputField>();
        LeMot = inputFieldCo.text;
        LeNombre = inputFieldCoNombre.text;
        inputFieldCoNombre.text = "";
        inputFieldCo.text = "";

        string numericString = string.Empty;
        foreach (var c in LeNombre)
        {
            if ((c >= '0' && c <= '9'))
            {
                numericString = string.Concat(numericString, c.ToString());
            }
        }

        if (Int32.TryParse(numericString, out int numValue))
        {
            if(numValue > 0 && numValue <= 9)
            {
                
                //todo envoi seveur
                RawMessage update = MainMenuManager.communicator.DetectivePlay(Room.infopartie.RoomInfo.RoomHostID,
                    Room.infopartie.RoomInfo.RoomHostID, LeMot + " - " + LeNombre);
                Debug.Log(update);
                if(update.ResponseCode == 302 && update.RequestCode == 402)
                {
                    WordGuess.SetActive(true);
                    AffichageMot.text = LeMot + " - " + LeNombre;
                    turnRole = "Operative";
                    changeTurn();
                }

                
            }
        }
        else
        {
            Debug.Log("Nombre trop grand");
        }
    }

    public void changeCompteur()
    {
        redpoints.text = (9 - TeamManager.nbPointsEquipeRouge).ToString();
        bluepoints.text = (8 - TeamManager.nbPointsEquipeBleue).ToString();
    }

    //Done button 
    public void Done()
    {
        RawMessage update = MainMenuManager.communicator.AgentPlay(Room.infopartie.RoomInfo.RoomHostID,
                    Room.infopartie.RoomInfo.RoomHostID, 0, true);
        Debug.Log(update);
        if (update.RequestCode == 401 && update.ResponseCode == 302)
        {
            if (turnColor.CompareRGB(Color.blue))
                turnColor = Color.red;
            else
                turnColor = Color.blue;

            turnRole = "Spymaster";

            //untag toutes les cartes
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    GridManager.gm.tableau[i, j].Untag();
                }
            }
            changeTurn();
        }
    }
    public void setTimer()
    {
        MainMenuManager.communicator.SetTimer(MainMenuManager.userid, Convert.ToUInt32(InputFieldTimer.text));
    }
    public void setTheme()
    {
        //JoinGame.communicator.SetThemes(JoinGame.userid, GameThemeManager.);
    }
}
