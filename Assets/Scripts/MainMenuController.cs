using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public AudioSource audSource;
    public AudioClip startSound;
    public Animator sceneFader;
    public GameObject continueButton;
    
    private void Start()
    {
        if (UniversalInfo.Load_ConvIndex() == 0)
        {
            continueButton.SetActive(false);
        }
        else
        {
            continueButton.SetActive(true);
        }
    }

    public void StartNewGame()
    {
        UniversalInfo.curConvIndex = 0;
        PlayerPrefs.SetInt("curConvIndex", 0);
        audSource.Stop();
        audSource.PlayOneShot(startSound);
        sceneFader.SetTrigger("FadeOut");
    }

    public void Continue()
    {
        audSource.Stop();
        audSource.PlayOneShot(startSound);
        sceneFader.SetTrigger("FadeOut");
    }
}
