using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    public Image image;
    private void Start()
    {
        image.enabled = true;
    }

    public void OnFadeComplete()
    {  
        SceneManager.LoadScene("ChatScene");
    }
}
