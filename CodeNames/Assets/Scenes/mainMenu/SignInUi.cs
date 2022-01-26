using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignInUi : MonoBehaviour
{

    [SerializeField] private GameObject SignInCanvas;
    [SerializeField] private Button uiBoutonFermer;

    private void Awake() {
        //uiBoutonFermer.onClick.AddListener(Fermer);
        Fermer();
    }
    public void Ouvrir () {
        SignInCanvas.SetActive (true);
    }
    
    public void Fermer() {
        SignInCanvas.SetActive(false);
    }
    private void OnDestroy(){
        //uiBoutonFermer.onClick.RemoveListener(Fermer);
    }
}


