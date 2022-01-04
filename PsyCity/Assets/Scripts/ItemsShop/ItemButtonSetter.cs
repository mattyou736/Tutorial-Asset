using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtonSetter : MonoBehaviour ,IPointerEnterHandler, IPointerExitHandler
{
    Text buttonText;
    Actor[] testparty;
    public Inventory partyInventory;
    public int itemNumber;

    public Image icon;

    public Text theHeader;
    public Text theText;

    public void OnPointerEnter(PointerEventData eventData)
    {
       
        theHeader.text = "In inventory";
        theText.text = partyInventory.items[itemNumber].uses.ToString();

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        theText.text = " ";
        theHeader.text = " ";
    }

    public void UpdateText()
    {
        theText.text = partyInventory.items[itemNumber].uses.ToString();
    }
}
