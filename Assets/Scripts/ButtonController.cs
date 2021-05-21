using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public Button button;

    public Sprite clickedImage;
    
    private void Start()
    {
        button.onClick.AddListener(SetClickedImage);
    }

    private void SetClickedImage()
    {
        button.image.sprite = clickedImage;
        tmp.margin = new Vector4(tmp.margin.x, tmp.margin.y + 30, tmp.margin.z, tmp.margin.w);
    }
}
