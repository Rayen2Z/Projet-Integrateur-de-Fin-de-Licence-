using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Theme : MonoBehaviour
{
    private string title;
    private int popularity;
    private int id;
    private static int idmax = 0;

    public static List<int> activeThemes = new List<int>();
    
    private GameObject go;
    private bool activated;
    private Button button;

    [SerializeField] public static Text NumThemes;
    
   public static void Start()
    {
        NumThemes = GameObject.Find("NumThemes").GetComponent<Text>();
    }

    
    public Theme(string _title, int _popularity, int _id)
    {
        this.title = _title;
        this.popularity = _popularity;
        this.activated = false;
        this.id = _id;
        idmax++;
        this.go = GameThemeManager.listgo[_id];
        this.button = this.go.GetComponent<Button>();
        this.button.onClick.AddListener(delegate{ClickTheme(_id);} );
        Text[] obj = this.go.GetComponentsInChildren<Text>();
        obj[0].text = title;
        obj[1].text = this.popularity.ToString();
 // get button text component in children and set the text property
        

        
    }
    public void setTitle(string _name)
    {
        this.title = _name;
    }

    public void setPop(int _pop)
    {
        this.popularity = _pop;
    }

    public GameObject getGo()
    {
        return this.go;
    }

    public string getTitle()
    {
        return this.title;
    }

    public int getPopularity()
    {
        return this.popularity;
    }

    public static void ClickTheme(int id)
    {
        Debug.Log("click " + id);
        if (activeThemes.Count < 3)
        {
            if (GameThemeManager.listtheme[id].activated)
                GameThemeManager.listtheme[id].activated = false;
            else
                GameThemeManager.listtheme[id].activated = true;

            List<string> stringtheme = new List<string>();
            foreach (Theme element in GameThemeManager.listtheme)
            {
                //todo a verif
                if(element.activated)
                    stringtheme.Add(element.title);
            }

            RawMessage update = MainMenuManager.communicator.SetThemes(MainMenuManager.userid, stringtheme);
            Debug.Log(update);
            if (update.ResponseCode == 305)
            {
                GameObject go = GameThemeManager.listgo[id];
                Theme theme = GameThemeManager.listtheme[id];
                if (theme.activated != true)
                {

                    activeThemes.Remove(id);
                    NumThemes.text = activeThemes.Count.ToString();

                    //GameObject.Find("themeUnActivated").GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                    go.transform.SetParent(GameObject.Find("themeUnActivated").transform);
                    go.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 0.4166666f);
                    RectTransform rt = (RectTransform)GameThemeManager.listgo[id].transform;
                    //rt.sizeDelta = new Vector2(5f, 0f);
                    rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 326.14f);
                    rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 27.62598f);
                }
                else
                {
                    activeThemes.Add(id);
                    NumThemes.text = activeThemes.Count.ToString();
                    go.transform.SetParent(GameObject.Find("themeActivated").transform);
                    RectTransform rt = (RectTransform)GameThemeManager.listgo[id].transform;
                    //rt.sizeDelta = new Vector2(-15f, 0f);
                    rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 112.23f);
                    rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 21.8f);
                }
            }

        }
    }


    public static void UpdateTheme(RawMessage update)
    {
        //init activate à false et les listes à nulles
        for(int j = 0;j< GameThemeManager.listtheme.Count;j++ )
        {
            GameThemeManager.listtheme[j].activated = false;
            //Destroy(GameThemeManager.listgo[j]);
        }
        for(int k = 0; k < activeThemes.Count;k++)
        {
            activeThemes.Remove(0);
        }


        //parcours de l'update
        for(int i = 0; i<update.RoomInfo.ThemeList.Count;i++)
        {
            int id = 0;
            bool trouved = false;
            while(trouved == false)
            {
                if (update.RoomInfo.ThemeList[i].CompareTo(GameThemeManager.listtheme[id].getTitle()) == 0)
                {
                    trouved = true;
                    GameThemeManager.listtheme[id].activated = true;
                }
                else
                {
                    id++;
                }
            }
                
        

            GameObject go = GameThemeManager.listgo[id];
            Theme theme = GameThemeManager.listtheme[id];
            if (theme.activated != true)
            {

                activeThemes.Remove(id);
                NumThemes.text = activeThemes.Count.ToString();
                //GameObject.Find("themeUnActivated").GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                go.transform.SetParent(GameObject.Find("themeUnActivated").transform);
                go.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 0.4166666f);
                RectTransform rt = (RectTransform)GameThemeManager.listgo[id].transform;
                //rt.sizeDelta = new Vector2(5f, 0f);
                rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 326.14f);
                rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 27.62598f);
            }
            else
            {
                activeThemes.Add(id);
                NumThemes.text = activeThemes.Count.ToString();
                go.transform.SetParent(GameObject.Find("themeActivated").transform);
                RectTransform rt = (RectTransform)GameThemeManager.listgo[id].transform;
                //rt.sizeDelta = new Vector2(-15f, 0f);
                rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 112.23f);
                rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 21.8f);
            }
        }
    }

    public void ClickClear()
    {
        List<int> cpy = new List<int>(activeThemes);
        foreach(int th in cpy) {
            ClickTheme(th);
        }
    }

}
