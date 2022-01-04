using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Actor : ScriptableObject
{
    //name of actor
    public new string name;
    public GameObject characterObject;
    public Image characterImage;

    [Header("Stats")]
    public float health;
    public float maxHealth;

    [Header("Action Points")]
    public float ac;
    public float maxAc;

    [Header("Weakness Element: Fire/Lightning/Water/Neutral")]
    public string weaknessElement;
    

    [Header("Weakness Property: Physical/Ranged/Slicing")]
    public string weaknessProperty;

    [Header("Property: Fire/Lightning/Water/Physical/Ranged/Slicing")]
    public string resistance;

    public GenericBattleAction[] actions;

    [Header("Property: none/posioned/sleep")]
    public string status;

    public float goldReward;

    //should be removed and added somewhere else-----
    public Vector2 attackRange = Vector2.one;
    //-----------------------------------------------

    public bool alive
    {
        get
        {
            return health > 0;
        }
    }
    //never drop below 0
    public void DecreaseHealth(int value)
    {
        
    }

    public void ResetHealth()
    {
        
    }

    public void ResetAP()
    {
        
    }

    public void ResetStatus()
    {
        
    }

    //cloning  actor 
    public T Clone<T>() where T : Actor
    {
        var clone = ScriptableObject.CreateInstance<T>();
       
        return clone;

    }

}
