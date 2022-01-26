using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class browserManager : MonoBehaviour
{
    public GameObject NewGamePanel;
    public GameObject BrowserPanel;
    public GameObject PasswordPanel;
    public static bool browser = true;

    [SerializeField] private InputField InputField;
    [SerializeField] public Text statuspassword;
    // Start is called before the first frame update
    

    public void ShowNewGamePanel()
    {
        if ( browser == true)
        {
            //fermer observer
            browser = false;
            NewGamePanel.SetActive(true);
            BrowserPanel.SetActive(false);
        }
        else
        {
            //ouvrir observer
            browser = true;
            NewGamePanel.SetActive(false);
            BrowserPanel.SetActive(true);
        }
    }

    public void hidePasswordPanel() {
        InputField.text = "";
        statuspassword.text = "";
        PasswordPanel.SetActive(false);
    }

    

}
