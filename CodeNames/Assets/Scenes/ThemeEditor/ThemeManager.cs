using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ThemeManager : MonoBehaviour
{
    List<GameObject> roomList = new List<GameObject>();

    [SerializeField]
    private Text status;

    [SerializeField]
    private Text Mystatus;

    [SerializeField]
    private GameObject roomListItemPrefab;

    [SerializeField]
    private GameObject wordPrefab;

    [SerializeField]
    private Transform roomListParent;

    [SerializeField]
    private Transform MyroomListParent;

    [SerializeField]
    private Transform ListNewTheme;

    // Start is called before the first frame update
    void Start()
    {
        
        for (int i = 0; i < 200; i++)
        {
            GameObject _roomListItemGO = Instantiate(roomListItemPrefab, new Vector3(0, 0, 0), Quaternion.identity,roomListParent);
        }

        for (int i = 0; i < 5; i++)
        {
            GameObject _roomListItemGO = Instantiate(roomListItemPrefab, new Vector3(0, 0, 0), Quaternion.identity, MyroomListParent);
        }
        status.text = "";
        Mystatus.text = "";

        for (int i = 0; i < 36; i++)
        {
            GameObject _roomListItemGO = Instantiate(wordPrefab, new Vector3(0, 0, 0), Quaternion.identity, ListNewTheme);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
