using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//asosiate editor with actor
[CustomEditor(typeof(Actor))]
public class ActorEditor : Editor
{
    [MenuItem("Assets/Create/Actor")]
    public static void CreateActor()
    {
        AssetUtil.CreateScriptableObject<Actor>();
    }
}
