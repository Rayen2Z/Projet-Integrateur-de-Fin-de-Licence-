using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager gm;
    private int rows = 5;
    private int cols = 5;
    private float size = 2.2f;
    private Vector2 positionOrigine;
    [SerializeField] public Card[,] tableau = new Card[5, 5];
    [SerializeField] private string[] mots;
    [SerializeField] private Color[] couleurs;

    public static Color bleu;
    public static Color rouge;
    public static Color noir;
    public static Color gris;
    

    // Start is called before the first frame update
    void Start()
    {
        gm = this;
        bleu = new Color(0.283f,0.3554977f,1f,1);
        rouge = new Color(1,0.259434f,0.259434f,1);
        noir = new Color(0.3018868f,0.3018868f,0.3018868f,1);
        gris = new Color(0.754717f,0.751157f,0.751157f,1);
        positionOrigine = transform.position;
        Debug.Log(positionOrigine);
    }

    public void delete()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Destroy(tableau[row, col].getTile());
            }
        }
        float gridW = cols * size;
        float gridH = rows * size;
        transform.position = positionOrigine;
    }
    public int cherchertourner(RawMessage update)
    {
        int num = 0;
        int res = 0;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                
                if (update.RoomInfo.CardList[num].TurnedUp == true && tableau[num / 5, num % 5].retournee != true)
                {
                    Card.tourner((uint)num);
                    res++;
                }
                num++;
            }
        }
        return res;
    }

    public void Generate(RawMessage update)
    {
        GameObject referenceTile = (GameObject)Instantiate(Resources.Load("Square"));
        //Color gris = (referenceTile.transform.Find("image").gameObject).GetComponent<SpriteRenderer>().color;

        int num = 0;
        /*
        mots = new string[]{"chouette", "licorne", "poteau", "balcon", "moisissure", "patate", "chien", "machine", "brosse", "arbre", "chromosome", "saucisse", "lingot", "montre", "ortie", "baignoire", "pince", "collier", "sauvage", "train", "voiture", "cheval", "capuche", "veste", "champagne"};
        shuffleString(mots);
        couleurs = new Color[]{gris, gris, gris, gris, gris, gris, gris, noir, bleu, bleu, bleu, bleu, bleu, bleu, bleu, bleu, bleu, rouge, rouge, rouge, rouge, rouge, rouge, rouge, rouge};
        shuffleColor(couleurs);
        */
        
        //init tableau
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                tableau[row, col] = new Card();
            }
        }

        //remplissage tableau
        for (int row = 0; row < rows; row ++)
        {
            for(int col = 0; col < cols; col ++)
            {
                GameObject tile = (GameObject)Instantiate(referenceTile, transform);
                
                tile.AddComponent<Card>();
                float posX = col * size;
                float posY = row * (-size/1.5f)-1.7f;

                tableau[row,col].ChangeMot(tile, update.RoomInfo.CardList[num].Word);
                tile.GetComponent<Card>().ChangeMot(tile, update.RoomInfo.CardList[num].Word);
                //BLUE(0),RED(1),ANONYMOUS(2),BLACK(3);
                Color coul;
                if (update.RoomInfo.CardList[num].Property == 0)
                    coul = bleu;
                else if (update.RoomInfo.CardList[num].Property == 1)
                    coul = rouge;
                else if (update.RoomInfo.CardList[num].Property == 2)
                    coul = gris;
                else
                    coul = noir;
                tableau[row,col].ChangeCouleur(coul);
                tile.GetComponent<Card>().ChangeCouleur(coul);

                tableau[row, col].setId(num);
                tile.GetComponent<Card>().setId(num);
                num++;

                tile.GetComponent<Card>().setGridManager(this as GridManager);

                tile.transform.position = new Vector3(posX, posY);
                tableau[row, col].setTile(tile);
                

            }
        }
        Destroy(referenceTile);
        
        float gridW = cols * size;
        float gridH = rows * size;
        transform.position = new Vector2(-gridW / 2 + size / 2 + 0.35f, gridH / 2 - size / 2);

    }

    void shuffleString(string[] array)
    {
        for (int i = 0; i < array.Length-1; i++ ){
            int r = Random.Range(i, array.Length);
            string tmp = array[r];
            array[r] = array[i];
            array[i] = tmp;
        }
    }

    void shuffleColor(Color[] array)
    {
        for (int i = 0; i < array.Length-1; i++ ){
            int r = Random.Range(i, array.Length);
            Color tmp = array[r];
            array[r] = array[i];
            array[i] = tmp;
        }
    }

    // Update is called once per frames
    void Update()
    {
        
    }
}
