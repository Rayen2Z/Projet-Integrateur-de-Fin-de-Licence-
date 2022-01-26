using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public static int iduser;
    public static void LoadMain(){
        SceneManager.LoadScene("main");
    }

    public static void LoadBrowser(){
        SceneManager.LoadScene("Browser");
    }

    public static void LoadGame(){
        Player.idblue = 0;
        Player.idbluespy = 0;
        Player.idredspy = 0;
        Player.idred = 0;
        foreach( GameObject go in TeamManager.listgo)
        {
            Destroy(go);
        }
        TeamManager.listgo = new List<GameObject>();
        TeamManager.players = new List<Player>();
        
        SceneManager.LoadScene("SampleScene");
    }

    public static void LoadThemeEditor()
    {
        SceneManager.LoadScene("ThemeEditor");
    }
}
