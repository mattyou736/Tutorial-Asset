using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetUtil : MonoBehaviour
{
    //T stands for type / this is a generic method thats gonna always be derivitive of ScriptableObject
    public static void CreateScriptableObject<T>() where T : ScriptableObject
    {
        var asset = ScriptableObject.CreateInstance<T>();

        //we will be creating in a folder so folder will be activeObject
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        //will create unique new name for asset
        var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New" + typeof(T) + ".asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);
        Selection.activeObject = asset;
        EditorUtility.FocusProjectWindow();
        AssetDatabase.SaveAssets();

    }
}
