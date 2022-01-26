using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Image SonON;
    [SerializeField] Image SonOFF;
    private bool sourdine = false;
    void Start()
    {
        if(!PlayerPrefs.HasKey("sourdine"))
        {
            PlayerPrefs.SetInt("sourdine", 0);
            Chargement();
        }
        else
        {
            Chargement();
        }
        
        ButtonIcon();
        AudioListener.pause = sourdine;
    }

    //button cliqué
    public void ButtonON()
    {
        if(sourdine == false)
        {
            sourdine = true;
            AudioListener.pause = true;
        }
        else 
        {
            sourdine = false;
            AudioListener.pause = false;
        }
        Sauvegarder();
        ButtonIcon();
    }

    //chargement bool
    private void Chargement()
    {
        sourdine = PlayerPrefs.GetInt("sourdine") == 1;
    }

    //sauvegarde d'etat
    private void Sauvegarder()
    {
        PlayerPrefs.SetInt("sourdine", sourdine ? 1 :0);
    }
    
    //alternance icon
    private void ButtonIcon()
    {
        if(sourdine == false )
        {
            SonON.enabled = true;
            SonOFF.enabled = false;
        }
        else 
        {
            SonON.enabled = false;
            SonOFF.enabled = true;

        }
    }
}
