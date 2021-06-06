using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "New Conversation", menuName = "Scriptable Objects/Conversation")]
public class Conversation : ScriptableObject
{
    public Sprite background;
    public AnimatorController ac;
    public List<DialogueLine> DialogueLines;

    public bool Valid => DialogueLines != null && DialogueLines.Count > 0;
    
    public void AssignDefaults()
    {
        for (int i = 0; i < DialogueLines.Count; i++)
        {
            if (DialogueLines[i].portrait == null)
            {
                DialogueLines[i] = new DialogueLine();
            }
        }
    }
}
