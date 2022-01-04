using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MoveSet", menuName = "MoveSet")]
public class MoveSet : ScriptableObject
{
    public string setName;

    //moveset to put together
    public GenericBattleAction[] actions;

    public string weakness;
}
