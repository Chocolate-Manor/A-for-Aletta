using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private bool pressed;
    private RectTransform rect;
    private Vector2 originalSizeDelta;

    public float pressedSize = 1.2f;

    private void Awake()
    {
        rect = this.GetComponent<RectTransform>();
        originalSizeDelta = rect.sizeDelta;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
        rect.sizeDelta = originalSizeDelta;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
        rect.sizeDelta = originalSizeDelta * pressedSize;
    }
    
}
