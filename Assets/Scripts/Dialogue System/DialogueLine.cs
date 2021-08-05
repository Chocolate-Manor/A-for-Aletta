using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueLine {
    public Sprite portrait;
    public Character character;
    
    public string nameOverwrite;
    public bool hasNameOverwrite;
    
    public Color textColor = Color.white;
    public float textSize = 30f;
    public float typeSpeed = 0.1f;
    
    public bool isImage;
    public Sprite Image;

    public bool screenShake;
    public bool isReversed;
    public bool hideMouth;

    public bool isNarration;
    
    [TextArea(2, 10)] public string text;

    public DialogueLine()
    {
        textColor = new Color32(129, 129, 129, 255);
        textSize = 30f;
        typeSpeed = 0.1f;
        isImage = false;
        isReversed = false;
        isNarration = false;
        hasNameOverwrite = false;
    }
}


