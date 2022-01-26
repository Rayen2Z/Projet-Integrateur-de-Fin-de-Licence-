using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button uiSignUpButton;
    [SerializeField] private SignUpUi SignUpUi;
    [SerializeField] private Button uiSignInButton;
    [SerializeField] private SignInUi SignInUi;

    [SerializeField] private InputField inputpseudo;
    [SerializeField] private InputField inputmdp;

    [SerializeField] private InputField inputmdpregister;
    [SerializeField] private InputField inputpseudoregister;
    [SerializeField] private InputField inputpseudoregisterconfirm;
    [SerializeField] private GameObject   canvasLogin;
    [SerializeField] private GameObject   canvasFormulaire;
    public static ClientNetworkModule.Communicator communicator = new ClientNetworkModule.Communicator("127.0.0.1",9999);
    public static uint userid;


    public void afficherLogin()
    {
        canvasLogin.SetActive(true);
    }
    
        public void afficherFormulaire()
    {
        canvasFormulaire.SetActive(true);
    }

    public void hideLogin()
    {
        canvasLogin.SetActive(false);
    }

    public void hideFormulaire()
    {
        canvasFormulaire.SetActive(false);
    }

    // Start is called before the first frame update
    private void Start()
    {
        //uiSignUpButton.onClick.AddListener(()=> SignUpUi.Ouvrir());
        //uiSignInButton.onClick.AddListener(()=> SignInUi.Ouvrir());
        
    }

   private void OnDestroy (){
      // uiSignUpButton.onClick.RemoveAllListeners ();
      // uiSignInButton.onClick.RemoveAllListeners ();

   }

   public void clickregistration()
    {
        
        if (inputmdpregister.text==inputpseudoregisterconfirm.text)
        {
            RawMessage login = communicator.Register(inputpseudoregister.text, inputmdpregister.text);
            Debug.Log(login.ToString());
            if (login.ResponseCode == 200)
            {
                hideFormulaire();
            }
        }
        else
          Debug.Log("les mdp ne correspondent pas");
       
    }
     public void clickloging()
    {

        RawMessage login = communicator.Login(inputpseudo.text,inputmdp.text);
        Debug.Log(login.ToString());
        if (login.RequestCode == 100 && login.ResponseCode == 200)
        {
            userid = login.UserID;
            Loader.LoadBrowser();
        }
        
    }
}
