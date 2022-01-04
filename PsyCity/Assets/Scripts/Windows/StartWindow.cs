using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartWindow : GenericWindow
{
    public Button continueButton;
    public GameObject infoWindow;
    public Inventory partyInventory;

    public override void Open()
    {
        var canContinue = false;

        continueButton.gameObject.SetActive(canContinue);

        if (continueButton.gameObject.activeSelf)
        {
            firstSelected = continueButton.gameObject;
        }


        base.Open();
    }

    public void NewGame()
    {
        OnNextWindow();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().canMove = true;
        partyInventory.money = 100;
    }

    public void Continue()
    {
        Debug.Log("Continue pressed");

    }

    public void InfoButton()
    {
        infoWindow.SetActive(true);
        infoWindow.GetComponent<InfoWindow>().GeneralButton(0);
    }

    public void QuitButton()
    {
        Application.Quit();
    }


}
