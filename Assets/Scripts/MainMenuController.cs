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
    private void Start()
    {   
        //check if a safe already exists
        startGameText.text = UniversalInfo.curConvIndex == 0 ? "New Game" : "Resume Game";

    }

    public void StartGame()
    {
        SceneManager.LoadScene("ChatScene");
    }
}
