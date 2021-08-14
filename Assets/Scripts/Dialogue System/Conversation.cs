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
    public string sceneHeading;
    public AudioClip backgroundMusic;

    public bool hasWhiteTransition;
    public bool bloomOn;

    public List<DialogueLine> dialogueLines;

    public bool Valid => dialogueLines != null && dialogueLines.Count > 0;
    
    public void AssignDefaults()
    {
        for (int i = 0; i < dialogueLines.Count; i++)
        {
            DialogueLine curLine = dialogueLines[i];
            if (curLine.portrait == null && !curLine.isNarration)
            {
                dialogueLines[i] = new DialogueLine();
            }
        }
    }
}
