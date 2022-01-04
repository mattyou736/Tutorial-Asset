using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.IO;
using System.Text;

//includes my windowmanager in this
[CustomEditor(typeof(WindowManager))]
public class WindowManagerEditor : Editor
{
    private ReorderableList list;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        list.DoLayoutList();

        serializedObject.ApplyModifiedProperties();

        //get all the windows based on array window manager
        if(GUILayout.Button("Generate Window Enums"))
        {
            var windows = ((WindowManager)target).windows;
            var total = windows.Length;
            //reconstructiong class
            var sb = new StringBuilder();
            sb.Append("public enum Windows{");
            sb.Append("None,");

            for(var i = 0; i < total; i++)
            {
                sb.Append(windows[i].name.Replace(" ", ""));
                if(i < total - 1)
                {
                    sb.Append(",");
                }

            }

            sb.Append("}");
            //promt user save path
            var path = EditorUtility.SaveFilePanel("Save the Window Enums", "", "WindowEnums.cs", "cs");

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(sb.ToString());
                }
            }
        }
    }

    private void OnEnable()
    {// true x4 = dragable , header visable , add button, remove button
        list = new ReorderableList(serializedObject, serializedObject.FindProperty("windows"), true, true, true, true);

        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Windows");
        };

        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            //figures out what deta to display in property field
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, Screen.width - 75, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
        };
    }

}
