using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public RectMask2D BGMask;
    public Animator BGAnimator;
    public Image BGImage;
    public TextMeshProUGUI BGText;

    private Conversation curConv;
    private int curLineIndex = 0;

    public GameObject panel;
    
    //presets
    public GameObject image;
    public GameObject dialogueBox;
    public GameObject dialogueBoxInvert;
    
    private void Awake()
    {   
        //load the current conversation
        curConv = DialogueManager.Instance.dialogues[UniversalInfo.curConvIndex];
        
        SetupScene();
    }
    
    /// <summary>
    /// Return the current line from the current conversation
    /// </summary>
    /// <returns></returns>
    private DialogueLine CurLine()
    {
        return curConv.dialogueLines[curLineIndex];
    }
    
    /// <summary>
    /// Set up the mask, animator controller, scene title and background image. 
    /// </summary>
    private void SetupScene()
    {
        BGMask.softness = curConv.maskSoftness;
        BGAnimator.runtimeAnimatorController = curConv.aniController;
        BGText.text = curConv.sceneHeading;
        BGImage.sprite = curConv.background;
    }
    
    /// <summary>
    /// Load the next scene for the conversation
    /// </summary>
    private void NextScene()
    {   
        //increment
        UniversalInfo.curConvIndex++;
        curConv = DialogueManager.Instance.dialogues[UniversalInfo.curConvIndex];
        
        //save the change
        
        //clear all children of panel
        foreach (Transform child in panel.transform)
        {
            Destroy(child.gameObject);
        }
        
        //Setup all over again
        SetupScene();
    }
    
    /// <summary>
    /// Load the nextLine in the conversation
    /// </summary>
    private void NextLine()
    {   
        //if end reached, go into next conversation
        if (curLineIndex >= curConv.dialogueLines.Count)
        {
            NextScene();
        }
        
        if (!CurLine().isImage)
        {
            GameObject curDialogueBox;
            if (CurLine().isReversed)
            {
                curDialogueBox = Instantiate(dialogueBoxInvert, panel.transform);
            }
            else
            {
                curDialogueBox = Instantiate(dialogueBox, panel.transform);
            }
            
            
            
        }
        else
        {
            GameObject curImage = Instantiate(image, panel.transform);
            curImage.GetComponent<Image>().sprite = CurLine().Image;
            curLineIndex++;
        }
    }
}
