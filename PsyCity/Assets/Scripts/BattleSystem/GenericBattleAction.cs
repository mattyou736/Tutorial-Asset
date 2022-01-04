using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericBattleAction : ScriptableObject {

    public new string name;
    public Vector2 baseDamage;
    public int apCost;

    public Vector2 missChance;

    [Header("Element: Fire/Lightning/Water/Neutral")]
    public string element;
    

    [Header("Property: Physical/Ranged/Slicing")]
    public string property;

    public Sprite icon;
    public GameObject AnimationHolder;


    protected string message = "undifined battle action message";
    public int attackValue_ = 0;

    [Header("Status effect")]
    public bool posion;
    public bool sleep;

    public float effectHitChance;

    public virtual void Action(Actor target1, Actor target2, GenericBattleAction action, string actionName, Transform enemySpawn)
    {
        //overide with action logic

    }

    public virtual void ComboAction(Actor target1, Actor target2, List<GenericBattleAction>  action, string actionName, Transform enemySpawn)
    {
        //overide with action logic

    }

    public virtual void EnemyAction(Actor target1, Actor target2, GenericBattleAction action, string actionName)
    {
        //overide with action logic

    }

    public virtual void EnemyComboAction(Actor target1, Actor target2, List<GenericBattleAction> attack, string actionName)
    {
        //overide with action logic

    }

    public override string ToString()
    {
        return message;
    }
}
