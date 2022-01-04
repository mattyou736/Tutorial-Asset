using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public int health = 5;
    public int EXP = 40;
    public int gold = 1000;
    public bool togglePlayerMovement;

    public Quest quest;
    //public GameObject battleWindow;

    public BattleWindow battleWindow;

    public MoveSetChanger moveShopWindow;
    //mesage window
    public WindowManager windowManager
    {
        get
        {
            return GenericWindow.manager;
        }
    }

    public string endScene;

    public void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E pressed");
            //OpenShop();
        }
    }

    public void GoBattle()
    {
        battleWindow = windowManager.Open((int)Windows.BattleScreen - 1, false) as BattleWindow;
        togglePlayerMovement = false;
    }

    public void OpenShop()
    {
        moveShopWindow = windowManager.Open((int)Windows.MoveSetShop - 1, false) as MoveSetChanger;
        togglePlayerMovement = false;
    }

    public void BattleOver()
    {
        Debug.Log("closing window");
        battleWindow.Close();
        togglePlayerMovement = true;
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(endScene);
    }
}
