using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestGiver : MonoBehaviour
{
    public Quest quest;
    public PlayerStats player;
    public GameObject QuestWindow;
    public TMP_Text titleText;
    public TMP_Text discriptionText;
    public TMP_Text EXPText;
    public TMP_Text goldText;

    public void OpenQuestWindow()
    {
        QuestWindow.SetActive(true);
        titleText.text = quest.title;
        discriptionText.text = quest.description;
        EXPText.text = quest.EXPReward.ToString();
        goldText.text = quest.goldReward.ToString();
    }

    public void AcceptQuest()
    {
        QuestWindow.SetActive(true);
        quest.isActive = true;
        player.quest = quest;
    }
}
