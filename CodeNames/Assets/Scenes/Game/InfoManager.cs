using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoManager : MonoBehaviour
{

    public static bool Player = false;
    public GameObject PlayerInfoUI;
 
    public void PlayerInfoButton()
        {
        if (Player == false)
            Player = true;
        else
            Player = false;
        PlayerInfoUI.SetActive(Player);
        }

}


