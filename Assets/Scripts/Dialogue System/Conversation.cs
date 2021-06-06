using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Conversation", menuName = "Scriptable Objects/Conversation")]
public class Conversation : ScriptableObject
{
    public Sprite background;
    public string sceneHeading;
    public AnimatorController aniController;
    public Vector2Int maskSoftness = new Vector2Int(300, 0);
    public List<DialogueLine> dialogueLines;

    public bool Valid => dialogueLines != null && dialogueLines.Count > 0;
    
    public void AssignDefaults()
    {
        for (int i = 0; i < dialogueLines.Count; i++)
        {
            if (dialogueLines[i].portrait == null)
            {
                dialogueLines[i] = new DialogueLine();
            }
        }
    }
}
