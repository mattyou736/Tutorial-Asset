using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class ItemShop : GenericWindow
{
    GenericWindow gameWindow;
    PlayerMovement player;
    public Inventory partyInventory;
    public Text infoText;
    public Text goldAmount;
    public GameObject[] buttons;

    //mesage window
    public WindowManager windowManager
    {
        get
        {
            return GenericWindow.manager;
        }
    }

    private void OnEnable()
    {
        foreach(GameObject button in buttons)
        {
            button.GetComponent<ItemButtonSetter>().partyInventory = partyInventory;
        }
        SetHUD();
    }

    void SetHUD()
    {
        goldAmount.text = partyInventory.money.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        player.canMove = false;
    }

    public void SelectItem(int index)
    {
        if (partyInventory.money >= 20)
        {
            partyInventory.items[index].uses += 1;
            partyInventory.money -= 20;
            SetHUD();
        }
    }

    public void CloseShop()
    {
        player.canMove = true;
        infoText.text = " ";
        gameWindow = windowManager.Open((int)Windows.DefaultWindow - 1, false) as GenericWindow;
        Close();
    }
}
