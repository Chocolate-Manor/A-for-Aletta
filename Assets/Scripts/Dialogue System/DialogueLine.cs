using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class DialogueLine {
    public Sprite portrait;
    public Character character;
    
    public string nameOverwrite;
    public bool hasNameOverwrite;
    
    public Color textColor = Color.white;
    public float textSize = 30f;
    public float typeSpeed = 0.1f;

    public bool screenShake;

    public bool isNarration;

    public AudioClip musicToSwitchTo;
    public bool switchMusic;
    
    [TextArea(2, 10)] public string text;

    public DialogueLine()
    {
        textColor = new Color32(129, 129, 129, 255);
        textSize = 30f;
        typeSpeed = 0.05f;
        isNarration = false;
        hasNameOverwrite = false;
        switchMusic = false;
    }
}


