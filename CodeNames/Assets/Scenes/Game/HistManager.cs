using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistManager : MonoBehaviour
{
    public static bool Hist = false;
    public GameObject HistPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonHist()
    {
        if (Hist == false)
            Hist = true;
        else
            Hist = false;
        HistPanel.SetActive(Hist);
    }
}
