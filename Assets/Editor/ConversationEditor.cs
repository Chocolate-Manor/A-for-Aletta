using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Conversation))]
public class ConversationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (Conversation) target;

        if (script.Valid)
        {
            if (GUILayout.Button("Assign Defaults", GUILayout.Height(40)))
            {
                script.AssignDefaults();
            }
        }
        else
        {
            GUIStyle style = GUI.skin.box;
            style.normal.textColor = Color.yellow;
            style.fixedHeight = 40;
            style.stretchWidth = true;
            style.alignment = TextAnchor.MiddleCenter;
 
            GUILayout.Box("Add Elements to the list to enable this button", style);   
        }
    }
}
