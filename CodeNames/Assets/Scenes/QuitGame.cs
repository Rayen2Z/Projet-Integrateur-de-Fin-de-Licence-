using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        if (activeScene.name=="SampleScene"){
            bool quit = MainMenuManager.communicator.QuitRoom(MainMenuManager.userid);
            Debug.Log(quit);
            if(quit)
            {
                Debug.Log("browser");
                Loader.LoadBrowser();
            }
        }
        else {
            Debug.Log("Main");
            Loader.LoadMain();
        }
    }

    public static void QuitRoom()
    {
        Debug.Log("browser");
        Loader.LoadBrowser();
    }

    public void QuitApp()
    {
        MainMenuManager.communicator.ShutDown();
        Debug.Log("Quit !");
        Application.Quit();
        
    }

   
}
