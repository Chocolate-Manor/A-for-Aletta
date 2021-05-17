using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Character", menuName = "Scriptable Objects/Character")]
public class Character : ScriptableObject
{
    public string characterName;

    public List<string> portraitNameIndex; 
    public List<Sprite> portraits;

    public List<string> textBoxNameIndex;
    public List<Sprite> textBoxes;

    public AudioClip characterVoice;
    
    public Sprite GetPortraitByName(string portraitName)
    {
        if (portraitNameIndex.Count != portraits.Count) throw new Exception("PortraitsIndex size and the amount of portraits does not match");
        int i = portraitNameIndex.IndexOf(portraitName); 
        if (i == -1) throw new Exception("Portrait name not found in portraitNameIndex.");

        return portraits[i];
    }
    
    public Sprite GetTextBoxByName(string textBoxName)
    {
        if (textBoxNameIndex.Count != textBoxes.Count) throw new Exception("TextBox size and the amount of textboxes does not match");
        int i = textBoxNameIndex.IndexOf(textBoxName); 
        if (i == -1) throw new Exception("TextBox name not found in textBoxNameIndex.");

        return textBoxes[i];
    }
    
    
}
