using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public bool isActive;

    public string title;
    public string description;
    public int EXPReward;
    public int goldReward;

    public QuestGoal questGoal;

    public void Complete()
    {
        isActive = false;
        Debug.Log(title + " Is completed");
    }
}
