using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSetter : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    public int action;
    Text buttonText;
    Actor[] testparty;
    Inventory partyInventory;
    int itemNumber;
    int memberTurn;
    public bool itemButton;
    public bool selectionButton;

    public Image icon;

    public Text theHeader;
    public Text theText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!itemButton && !selectionButton)
        {
            theHeader.text = "Cost";
            theText.text = testparty[memberTurn].actions[action].apCost + " AP";
        }
            

        if (itemButton)
        {
            theHeader.text = "uses left";
            theText.text = partyInventory.items[itemNumber].uses.ToString();
        }
            
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!selectionButton)
        {
            theText.text = " ";
            theHeader.text = " ";
        }
            
    }

    //get the name of the actions from the current party memebers turn and apply to button
    public void SetButtons(Actor[] partyMembers, int currentPartyMemberTurn)
    {
        testparty = partyMembers;
        memberTurn = currentPartyMemberTurn;
        buttonText = GetComponentInChildren<Text>();
        buttonText.text = partyMembers[currentPartyMemberTurn].actions[action].name;
        icon.sprite = partyMembers[currentPartyMemberTurn].actions[action].icon;

    }

    //get the name of the actions from the current party memebers turn and apply to button
    public void SetItemButtons(Inventory partyInventory_,int number)
    {
        buttonText = GetComponentInChildren<Text>();
        itemNumber = number;
        partyInventory = partyInventory_;
        buttonText.text = partyInventory_.items[number].name;
        icon.sprite = partyInventory_.items[number].icon;
    }

    public void SetButtonsTargets(Actor target)
    {
        buttonText = GetComponentInChildren<Text>();
        buttonText.text = target.name;
    }



}
