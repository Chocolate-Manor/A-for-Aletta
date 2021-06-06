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
    public ScrollRect scroll;
    
    //presets
    public GameObject image;
    public GameObject dialogueBox;
    public GameObject dialogueBoxInvert;
    public GameObject continueButton;
    
    private void Awake()
    {   
        //load the current conversation
        curConv = DialogueManager.Instance.dialogues[UniversalInfo.curConvIndex];
        
        ClearPanel();
        SetupScene();
        NextLine();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && !isTyping)
        {
            NextLine();
        }
        
        //constantly refresh canvas to prevent weird things
        Canvas.ForceUpdateCanvases();

        if (isTyping)
        {
            ScrollToBottom();
        }
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
        Debug.Log("into next scene!");
        
        //increment
        UniversalInfo.curConvIndex++;
        curConv = DialogueManager.Instance.dialogues[UniversalInfo.curConvIndex];
        
        //save the change
        
        //clear all children of panel
        ClearPanel();
        
        //Setup all over again
        SetupScene();
    }

    private void ClearPanel()
    {
        foreach (Transform child in panel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private bool isTyping;
    private IEnumerator TypeWrite(string text, DialogueBoxController dialogueBoxController, float typeSpeed)
    {
        isTyping = true;
        dialogueBoxController.dialogueText.text = "";

        string[] lines = text.Split("\n"[0]);
        foreach (string line in lines)
        {
            foreach (char c in line)
            {
                dialogueBoxController.dialogueText.text += c;
                yield return new WaitForSeconds(typeSpeed);
            }
            dialogueBoxController.dialogueText.text += "\n";
            yield return new WaitForSeconds(0.1f);
        }
        isTyping = false;
    }

    private void ScrollToBottom()
    {  
        Canvas.ForceUpdateCanvases();
        if (scroll.verticalNormalizedPosition > 0)
        {
            scroll.verticalNormalizedPosition = 0;
        }
    }
    
    /// <summary>
    /// Set up the textbox of current line.
    /// </summary>
    /// <param name="curLine"></param>
    private void SetUpTextBox(DialogueLine curLine)
    {
        GameObject curDialogueBox;
        if (curLine.isReversed)
        {
            curDialogueBox = Instantiate(dialogueBoxInvert, panel.transform);
        }
        else
        {
            curDialogueBox = Instantiate(dialogueBox, panel.transform);
        }
        DialogueBoxController dialogueBoxController = curDialogueBox.GetComponent<DialogueBoxController>();
        if (curLine.hideMouth) dialogueBoxController.mouth.SetActive(false);
        dialogueBoxController.portrait.sprite = curLine.portrait;
        StartCoroutine(TypeWrite(curLine.text, dialogueBoxController, curLine.typeSpeed));
        dialogueBoxController.dialogueText.fontSize = curLine.textSize;
        dialogueBoxController.dialogueText.color = curLine.textColor;
        dialogueBoxController.nameText.text = curLine.character.CharacterName;
        dialogueBoxController.textBG.sprite = curLine.character.textBoxSprite;
    }

    private bool continueButtonSpawned = false;
    
    /// <summary>
    /// Load the nextLine in the conversation
    /// </summary>
    private void NextLine()
    {
        //if end reached, go into next conversation
        if (curLineIndex >= curConv.dialogueLines.Count)
        {
            if (!continueButtonSpawned)
            {
                GameObject curButton = Instantiate(continueButton, panel.transform);
                curButton.GetComponent<Button>().onClick.AddListener(NextScene);
                continueButtonSpawned = true;
                ScrollToBottom();
                return;
            }
            else
            {
                return;
            }
        }

        DialogueLine curLine = CurLine();
        
        if (!curLine.isImage)
        {
            SetUpTextBox(curLine);
            ScrollToBottom();
        }
        else
        {
            GameObject curImage = Instantiate(image, panel.transform);
            curImage.GetComponent<Image>().sprite = curLine.Image;
            ScrollToBottom();
        }
        curLineIndex++;
    }
}
