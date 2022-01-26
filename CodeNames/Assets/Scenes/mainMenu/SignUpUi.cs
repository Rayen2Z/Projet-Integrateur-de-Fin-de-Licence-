using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignUpUi : MonoBehaviour
{
    [SerializeField] private GameObject SignUpCanvas;
    [SerializeField] private Button uiBoutonFermer;

    private void Awake() {
       // uiBoutonFermer.onClick.AddListener(Fermer);
        Fermer();
    }
    public void Ouvrir () {
        SignUpCanvas.SetActive (true);
    }
    
    public void Fermer() {
        SignUpCanvas.SetActive(false);
    }
    private void OnDestroy(){
        //uiBoutonFermer.onClick.RemoveListener(Fermer);
    }
}
