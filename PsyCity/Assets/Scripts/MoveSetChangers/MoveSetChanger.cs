using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveSetChanger : GenericWindow
{
    public MoveSet[] sets;
    public Text[] buttonTexts;
    public Text[] action1Texts;
    public Text[] action2Texts;
    public Text[] action3Texts;
    public Text[] weaknessTexts;
    public Text infoText;

    public GameObject partyHolder, setsHolder;

    MoveSet set_;

    public NewBattleSystem battleSystem;
    GenericWindow gameWindow;
    PlayerMovement player;

    //mesage window
    public WindowManager windowManager
    {
        get
        {
            return GenericWindow.manager;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        player.canMove = false;
        for (int i = 0; i < sets.Length; i++)
        {
            buttonTexts[i].text = sets[i].setName;
            action1Texts[i].text = sets[i].actions[1].name;
            action2Texts[i].text = sets[i].actions[2].name;
            action3Texts[i].text = sets[i].actions[3].name;
            weaknessTexts[i].text = "Weakness " + sets[i].weakness;
        }
    }

    public void SelectSet(int setNumber)
    {
        partyHolder.SetActive(true);
        set_ = sets[setNumber];
    }

    public void SelectPlayer(int partyMember)
    {
        partyHolder.SetActive(false);
        battleSystem.partyMembers[partyMember].actions = set_.actions;
        battleSystem.partyMembers[partyMember].weaknessElement = set_.weakness;
        infoText.text = battleSystem.partyMembers[partyMember].name + " has gained the " + set_.name + " battle set.";
    }

    public void CloseShop()
    {
        player.canMove = true;
        infoText.text = " ";
        gameWindow = windowManager.Open((int)Windows.DefaultWindow - 1, false) as GenericWindow;
        Close();
    }
}
