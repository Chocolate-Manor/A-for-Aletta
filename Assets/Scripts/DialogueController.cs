using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public RectMask2D BGMask;
    public Animator BGAnimator;
    public Image BGImage;
    public TextMeshProUGUI BGText;
    public AudioSource audioSource;
    public Animator camAnim;
    
    private Conversation curConv;
    private int curLineIndex = 0;

    public GameObject panel;
    public ScrollRect scroll;
    
    //presets
    public GameObject image;
    public GameObject dialogueBox;
    public GameObject dialogueBoxInvert;
    public GameObject continueButton;
    public GameObject narrationBox;
    
    private void Awake()
    {   
        //load the current conversation
        curConv = DialogueManager.Instance.dialogues[UniversalInfo.curConvIndex];
        
        ClearPanel();
        SetupScene();
    }

    private void Update()
    {
        // if (Input.GetButtonDown("Fire1") && !isTyping)
        // {
        //     NextLine();
        // }
        
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
    /// As well as reset the conversation, starts the next line.
    /// </summary>
    private void SetupScene()
    {
        BGMask.softness = curConv.maskSoftness;
        BGAnimator.runtimeAnimatorController = curConv.aniController;
        BGText.text = curConv.sceneHeading;
        BGImage.sprite = curConv.background;
        
        //avoid replaying the same song
        if (audioSource.clip != curConv.backgroundMusic)
        {
            audioSource.clip = curConv.backgroundMusic;
            audioSource.Play();
        }
        
        //remember to reset curLineIndex..
        curLineIndex = 0;
        NextLine();
        continueButtonSpawned = false;
    }
    
    /// <summary>
    /// Load the next scene for the conversation
    /// </summary>
    private void NextConv()
    {   
        //trigger the nextConv event
        UniversalInfo.nextSceneEvent(UniversalInfo.curConvIndex);
        
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
            string lineTrim = line.Trim();
            foreach (char c in lineTrim)
            {
                dialogueBoxController.dialogueText.text += c;
                yield return new WaitForSeconds(typeSpeed);
            }
            dialogueBoxController.dialogueText.text += " ";
            yield return new WaitForSeconds(0.4f);
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
        if (curLine.screenShake) CamShake();
        GameObject curDialogueBox;
        if (curLine.isNarration)
        {
            curDialogueBox = Instantiate(narrationBox, panel.transform);
        }
        else
        {
            curDialogueBox = curLine.isReversed
                ? Instantiate(dialogueBoxInvert, panel.transform)
                : Instantiate(dialogueBox, panel.transform);
        }
        
        DialogueBoxController dialogueBoxController = curDialogueBox.GetComponent<DialogueBoxController>();
        
        //if isn't narration, then don't ignore mouth, portrait and name text since they exist
        //Nor is there need to set textBG.
        if (!curLine.isNarration)
        {
            if (curLine.hideMouth) dialogueBoxController.mouth.SetActive(false);
            dialogueBoxController.portrait.sprite = curLine.portrait;
            dialogueBoxController.nameText.text = curLine.character.CharacterName;
            dialogueBoxController.textBG.sprite = curLine.character.textBoxSprite;
        }
        dialogueBoxController.dialogueText.fontSize = curLine.textSize;
        dialogueBoxController.dialogueText.color = curLine.textColor;
        
        //Only do typewrite if type speed is above 0.
        if (curLine.typeSpeed > 0)
        {
            StartCoroutine(TypeWrite(curLine.text, dialogueBoxController, curLine.typeSpeed));
        }
        else
        {
            dialogueBoxController.dialogueText.text = curLine.text;
        }
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
                curButton.GetComponent<Button>().onClick.AddListener(NextConv);
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
        
        //set up either image or box
        if (!curLine.isImage)
        {
            SetUpTextBox(curLine);
            ScrollToBottom();
        }
        else
        {
            SetupImage(curLine);
            ScrollToBottom();
        }
        curLineIndex++;
    }

    private void SetupImage(DialogueLine curLine)
    {
        GameObject curImage = Instantiate(image, panel.transform);
        ImageController imageController = curImage.GetComponent<ImageController>();
        imageController.image.sprite = curLine.Image;
    }

    private void CamShake()
    {
        camAnim.SetTrigger("Shake");
    }
}
