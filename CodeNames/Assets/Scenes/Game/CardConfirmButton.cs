using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardConfirmButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Card card;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void changeCard(Card _card) {
        this.card = _card;
    }

    public void retournerCardFromButton()
    {
        ((Card)this.card).retournerCard();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ((Card)this.card).setCanTag(false);
    }
 
    public void OnPointerExit(PointerEventData eventData)
    {
        ((Card)this.card).setCanTag(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
