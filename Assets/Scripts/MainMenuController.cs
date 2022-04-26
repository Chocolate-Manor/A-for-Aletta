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
    public Animator popUpAnimator;

    public TextMeshProUGUI popUpText;
    public Button popUpYesButton;
    
    private bool popUpIsOpen;
    
    
    private void Start()
    {
        popUpIsOpen = false;

        audSource = GameObject.FindWithTag("audio").GetComponent<AudioSource>();
        
        if (UniversalInfo.Load_ConvIndex() == 0)
        {
            continueButton.SetActive(false);
        }
        else
        {
            continueButton.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClickOnQuit();
        } 
    }

    public void NewGamePopUp()
    {
        if (!popUpIsOpen)
        {
            popUpYesButton.onClick.AddListener(delegate { StartNewGame();});
            popUpText.text = "Start a new game?";
            popUpAnimator.SetTrigger("Open");
            popUpIsOpen = true;
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

    public void ClosePopUp()
    {
        popUpAnimator.SetTrigger("Close");
        RemoveAllYesButtonListeners();
        popUpIsOpen = false;
    }

    private void RemoveAllYesButtonListeners()
    {
        popUpYesButton.onClick.RemoveAllListeners();
    }
    
    public void Continue()
    {
        audSource.Stop();
        audSource.PlayOneShot(startSound);
        sceneFader.SetTrigger("FadeOut");
    }

    public void ClickOnQuit()
    {   
        if (!popUpIsOpen)
        {
            popUpYesButton.onClick.AddListener(delegate { QuitGame(); });
            popUpText.text = "Exit the game?";     
            popUpAnimator.SetTrigger("Open");
            popUpIsOpen = true;
        }
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
