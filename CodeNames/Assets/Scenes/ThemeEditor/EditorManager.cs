using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorManager : MonoBehaviour
{
    public GameObject NewThemePanel;
    public GameObject BrowserPanel;
    public GameObject MyBrowserPanel;
    public static bool browser = true;

    // Start is called before the first frame update
    

    public void ShowNewThemePanel()
    {
        if ( browser == true)
        {
            //fermer observer
            browser = false;
            NewThemePanel.SetActive(true);
            BrowserPanel.SetActive(false);
            MyBrowserPanel.SetActive(false);
        }
        else
        {
            //ouvrir observer
            browser = true;
            NewThemePanel.SetActive(false);
            BrowserPanel.SetActive(true);
            MyBrowserPanel.SetActive(true);
        }
    }
}
