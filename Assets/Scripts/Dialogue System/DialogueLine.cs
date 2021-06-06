using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueLine {
    public Sprite portrait;
    public Character character;

    public Color textColor = Color.white;
    public float textSize = 30f;
    public float typeSpeed = 0.1f;
    
    public bool isImage;
    public Sprite Image;

    public bool screenShake;
    public bool isReversed;

    [TextArea(2, 5)] public string text;

    public DialogueLine()
    {
        textColor = Color.white;
        textSize = 30f;
        typeSpeed = 0.1f;
        isImage = false;
        isReversed = false;
    }
}


