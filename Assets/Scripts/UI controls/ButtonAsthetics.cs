using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAsthetics : MonoBehaviour
{
    public float sizeScale = 1.5f;
    
    /// <summary>
    /// Increase button size on click, to make it feel more interactive.
    /// </summary>
    public void Increase_Size_On_Click()
    {
        RectTransform rect = this.GetComponent<RectTransform>();
        rect.sizeDelta = rect.sizeDelta * sizeScale;
    }
}
