using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public TextMeshProUGUI startGameText;
    public AudioSource audSource;
    public AudioClip startSound;
    public Animator sceneFader;
    private void Start()
    {   
        //check if a safe already exists
        startGameText.text = UniversalInfo.Load_ConvIndex() == 0 ? "New Game" : "Continue";
        
    }

    public void StartGame()
    {   
        audSource.Stop();
        audSource.PlayOneShot(startSound);
        sceneFader.SetTrigger("FadeOut");
    }
}
