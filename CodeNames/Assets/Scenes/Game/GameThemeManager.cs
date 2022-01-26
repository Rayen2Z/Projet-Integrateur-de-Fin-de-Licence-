using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameThemeManager : MonoBehaviour
{


    

    [SerializeField]
    public Transform listUnActivated;

    [SerializeField]
    public Transform listActivated;

    [SerializeField] private GameObject prefab;

    public static List<GameObject> listgo = new List<GameObject>();

    public static List<Theme> listtheme = new List<Theme>();

    public static GameThemeManager gtm;
    

    // Start is called before the first frame update
    public void Start()
    {
        
        gtm = this;
        foreach(GameObject elem in GameThemeManager.listgo)
        {
            Destroy(elem);
        }
        
        GameThemeManager.listtheme = new List<Theme>();
        GameThemeManager.listgo = new List<GameObject>();
        List<String> list = MainMenuManager.communicator.GetGlobalThemeList(MainMenuManager.userid);
        for (int i = 0; i < list.Count; i++)
        {
            
            listgo.Add(Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity, listUnActivated));

            Theme tm = new Theme(list[i], 8+i+50-i*2, i);
            listtheme.Add(tm);
            
            
        }
    }

    public static void gtmstart(Transform listUn, GameObject pref)
    {
        foreach (GameObject elem in GameThemeManager.listgo)
        {
            Destroy(elem);
        }

        GameThemeManager.listtheme = new List<Theme>();
        GameThemeManager.listgo = new List<GameObject>();
        List<String> list = MainMenuManager.communicator.GetGlobalThemeList(MainMenuManager.userid);
        for (int i = 0; i < list.Count; i++)
        {

            listgo.Add(Instantiate(pref, new Vector3(0, 0, 0), Quaternion.identity, listUn));

            Theme tm = new Theme(list[i], 8 + i + 50 - i * 2, i);
            listtheme.Add(tm);


        }
    }

    public void trierpop(bool pop)
    {
        List<Theme> listtemp = new List<Theme>();
        int taille = listtheme.Count;
        if(pop) //si true on retourne la liste min
        { 
            for(int i = 0; i < taille;i++)
            {
                int minpop = 999999;
                int max = 0;
                for(int j = 0; j < listtheme.Count;j++)
                {
                    if(listtheme[j].getPopularity() <= minpop)
                    {
                        minpop = listtheme[j].getPopularity();
                        max = j;
                    }
                }
                listtemp.Add(listtheme[max]);
                listtheme.RemoveAt(max);
            }
        }
        else
        {
            for(int i = 0; i < taille;i++)
            {
                int maxpop = 0;
                int max = 0;
                for(int j = 0; j < listtheme.Count;j++)
                {
                    if(listtheme[j].getPopularity() >= maxpop)
                    {
                        maxpop = listtheme[j].getPopularity();
                        max = j;
                    }
                }
                listtemp.Add(listtheme[max]);
                listtheme.RemoveAt(max);
            }
        }
        for(int i = 0; i< listgo.Count;i++)
        {
            Destroy(listgo[i]);
        }
        GameThemeManager.listgo = new List<GameObject>();
        listtheme = listtemp;
        for (int i = 0; i < listtheme.Count; i++)
        {
            
            listgo.Add(Instantiate(prefab));

            listgo[i].transform.localScale = new Vector3(2.4f,2.4f,1);
            listgo[i].transform.SetParent(listUnActivated);
            Theme tm = new Theme(listtheme[i].getTitle(), listtheme[i].getPopularity(), i);
        }
    }

    public void trierName(bool pop)
    {
        List<Theme> listtemp = new List<Theme>();
        int taille = listtheme.Count;
        if(pop) //si true on retourne la liste min
        { 
            for(int i = 0; i < taille;i++)
            {
                string minpop = "999999";
                int max = 0;
                for(int j = 0; j < listtheme.Count;j++)
                {
                    if(String.Compare(listtheme[j].getTitle(), minpop) <= 0)
                    {
                        minpop = listtheme[j].getTitle();
                        max = j;
                    }
                }
                listtemp.Add(listtheme[max]);
                listtheme.RemoveAt(max);
            }
        }
        else
        {
            for(int i = 0; i < taille;i++)
            {
                string maxpop = "0";
                int max = 0;
                for(int j = 0; j < listtheme.Count;j++)
                {
                    if(string.Compare(listtheme[j].getTitle(), maxpop) <= 0)
                    {
                        maxpop = listtheme[j].getTitle();
                        max = j;
                    }
                }
                listtemp.Add(listtheme[max]);
                listtheme.RemoveAt(max);
            }
        }
        for(int i = 0; i< listgo.Count;i++)
        {
            Destroy(listgo[i]);
        }
        GameThemeManager.listgo = new List<GameObject>();
        listtheme = listtemp;
        for (int i = 0; i < listtheme.Count; i++)
        {
            
            listgo.Add(Instantiate(prefab));

            listgo[i].transform.localScale = new Vector3(2.4f,2.4f,1);
            listgo[i].transform.SetParent(listUnActivated);
            Theme tm = new Theme(listtheme[i].getTitle(), listtheme[i].getPopularity(), i);
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
