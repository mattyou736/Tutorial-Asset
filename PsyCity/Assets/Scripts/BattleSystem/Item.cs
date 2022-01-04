using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public new string name;

    [Header("Recovery")]
    public int healthAmount;
    public int apAmount;

    public int uses;

    public Sprite icon;

    public GameObject button;
}
