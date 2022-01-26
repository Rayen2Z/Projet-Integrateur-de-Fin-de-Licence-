using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JoinButton : MonoBehaviour
{
    [SerializeField] private Color teamColor;
    [SerializeField] private string role;
    [SerializeField] public Player player;

    [SerializeField] private Transform list;

    public static JoinButton jb;

    // Start is called before the first frame update
    void Start()
    {
        jb = this;
    }
    public static void init()
    {
        jb.player.changePseudo(TeamManager.players[0].getPseudo());
        jb.player.changeTeamColor(TeamManager.players[0].getTeamColor());
        jb.player.changeTagColor(TeamManager.players[0].getTagColor());
        jb.player.changeRole(TeamManager.players[0].getRole());
    }
    public void updatePlayer() {
        //vérification joueur max
        Debug.Log("UpdateJoin : "+player.getTeamColor() + "-" + player.getRole());
        if(this.teamColor == Color.red) {
            if(this.role == "Operative")
            {
                if(Player.idred >= 3)
                {
                    return;
                } else {
                    //Verification des precedents teamcolor et role du joueur
                    if(!player.getTeamColor().Equals(Color.red) || (player.getRole() == "Spymaster" && player.getTeamColor().Equals(Color.red))) {
                        if(player.getRole() == "Spymaster" && player.getTeamColor().Equals(Color.red)) {
                            Player.idredspy--;
                        } else if(player.getRole() == "Spymaster" && player.getTeamColor().Equals(Color.blue)) {
                            Player.idbluespy--;
                        } else if(player.getRole() == "Operative" && player.getTeamColor().Equals(Color.blue)) {
                            Player.idblue--;
                        }
                        player.changeTagColor(Player.tabcouleur[Player.idred]);
                        //todo RawMessage list = MainMenuManager.communicator.(JoinGame.userid, room.);
                        
                        Player.idred++;
                    }
                }
            }
            else
            {
                if(Player.idredspy >= 2)
                {
                    return;
                } else {
                    //Verification des precedents teamcolor et role du joueur
                    if(!player.getTeamColor().Equals(Color.red) || (player.getRole() == "Operative" && player.getTeamColor().Equals(Color.red))) {
                        if(player.getRole() == "Operative" && player.getTeamColor().Equals(Color.red)) {
                            Player.idred--;
                        } else if(player.getRole() == "Spymaster" && player.getTeamColor().Equals(Color.blue)) {
                            Player.idbluespy--;
                        } else if(player.getRole() == "Operative" && player.getTeamColor().Equals(Color.blue)) {
                            Player.idblue--;
                        } 
                        player.changeTagColor(Color.clear);
                        Player.idredspy++;
                    }
                }
            }
        }
        else if(this.teamColor == Color.blue) {
            if(this.role == "Operative")
            {
                if(Player.idblue >= 3)
                {
                    return;
                } else {
                    //Verification des precedents teamcolor et role du joueur
                    if(!player.getTeamColor().Equals(Color.blue) || (player.getRole() == "Spymaster" && player.getTeamColor().Equals(Color.blue))) {
                        if(player.getRole() == "Spymaster" && player.getTeamColor().Equals(Color.blue)) {
                            Player.idbluespy--;
                        } else if(player.getRole() == "Spymaster" && player.getTeamColor().Equals(Color.red)) {
                            Player.idredspy--;
                        } else if(player.getRole() == "Operative" && player.getTeamColor().Equals(Color.red)) {
                            Player.idred--;
                        }
                        player.changeTagColor(Player.tabcouleur[Player.idblue]);
                        Player.idblue++;
                    }
                }
            }
            else
            {
                if(Player.idbluespy >= 2)
                {
                    return;
                } else {
                    //Verification des precedents teamcolor et role du joueur
                    if(!player.getTeamColor().Equals(Color.blue) || (player.getRole() == "Operative" && player.getTeamColor().Equals(Color.blue))) {
                        if(player.getRole() == "Operative" && player.getTeamColor().Equals(Color.blue)) {
                            Player.idblue--;
                        } else if(player.getRole() == "Spymaster" && player.getTeamColor().Equals(Color.red)) {
                            Player.idredspy--;
                        } else if(player.getRole() == "Operative" && player.getTeamColor().Equals(Color.red)) {
                            Player.idred--;
                        }
                        player.changeTagColor(Color.clear);
                        Player.idbluespy++;
                    }
                }
            }
        }

        GameObject oldList = null;
        if(this.player.getRole() == "Operative" && (this.player.getTeamColor().Equals(Color.red) || this.player.getTeamColor().Equals(Color.blue))) {
            oldList = TeamManager.listgo[this.player.getId()].transform.parent.gameObject;
        }

        this.player.changeTeamColor(this.teamColor);
        this.player.changeRole(this.role);

        TeamManager.listgo[this.player.getId()].transform.SetParent(list);  

        if(oldList != null && oldList.transform.childCount > 0) {
            for(int i=0; i<oldList.transform.childCount; i++) {
                GameObject child = oldList.transform.GetChild(i).gameObject;
                GameObject button = child.transform.Find("Button").gameObject;
                TeamManager.getPlayerFromPseudo(button.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text).changeTagColor(Player.tabcouleur[i]);
            }
        }

        //quand le joueur rejoint en cours de partie il faut lui faire commencer le tour pour afficher l'interface 
        if (InterfaceManager.Lobby == false)
        {
            InterfaceManager.it.changeTurn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
