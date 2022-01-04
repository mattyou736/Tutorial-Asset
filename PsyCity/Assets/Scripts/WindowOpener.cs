using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowOpener : MonoBehaviour
{
    MoveSetChanger moveShopWindow;
    ItemShop itemShopWindow;

    public bool itemShop;
    public GameObject interactionUI;

    public WindowManager windowManager
    {
        get
        {
            return GenericWindow.manager;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player" && Input.GetKeyDown(KeyCode.E))
        {
            OpenShop();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            interactionUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            interactionUI.SetActive(false);
        }
    }

    public void OpenShop()
    {
        if (itemShop)
        {
            itemShopWindow = windowManager.Open((int)Windows.ItemShop - 1, false) as ItemShop;
        }
        else
        {
            moveShopWindow = windowManager.Open((int)Windows.MoveSetShop - 1, false) as MoveSetChanger;
        }
        
        interactionUI.SetActive(false);
    }
}
