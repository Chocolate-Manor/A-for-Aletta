using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.Rendering;


public class DialogueController : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public AudioSource audioSource;
    public Animator camAnim;
    
    private Conversation curConv;
    private int curLineIndex = 0;

    public GameObject panel;
    public ScrollRect scroll;
    public GameObject panelPrefab;
    
    //presets
    public GameObject dialogueBox;
    public GameObject narrationBox;
    
    //fade in fade out animator
    public Animation fadeAnimation;
    public Image cover;
    public GameObject volumeObject;

    private void Awake()
    {   
        //load the current conversation
        curConv = DialogueManager.Instance.dialogues[UniversalInfo.curConvIndex];
        ClearPanel();
        SetupScene();
    }
    
    /// <summary>
    /// The external method attached to the action button. So that on click we get the next line of dialogue. 
    /// </summary>
    public void NextLineOnButtonClick()
    {
        if (!isTyping)
        {
            NextLine();
        }
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
        //print the curConv index for debug reasons
        //Debug.Log(UniversalInfo.curConvIndex);
        
        titleText.text = curConv.sceneHeading;
        //avoid replaying the same song
        if (audioSource.clip != curConv.backgroundMusic)
        {
            audioSource.clip = curConv.backgroundMusic;
            audioSource.Play();
        }
        
        //turn on bloom
        if (curConv.bloomOn)
        {
            volumeObject.SetActive(true);
        }
        else
        {
            volumeObject.SetActive(false);
        }

        //remember to reset curLineIndex..
        curLineIndex = 0;
        
        //play enter sound;
        if (curConv.hasEnterSound)
        {
            audioSource.PlayOneShot(curConv.enterSound);
        }
        

        NextLine();

        haveFadedOut = false;
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
        
        //to make panel be in correct location
        //ResetPanelLocation();

        //Setup all over again
        SetupScene();
    }

    private void ClearPanel()
    {
        Destroy(panel.gameObject);
        panel = Instantiate(panelPrefab, scroll.transform);
        scroll.content = panel.GetComponent<RectTransform>();
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
            curDialogueBox = Instantiate(dialogueBox, panel.transform);
        }
        
        DialogueBoxController dialogueBoxController = curDialogueBox.GetComponent<DialogueBoxController>();
        
        //if isn't narration, then don't ignore mouth, portrait and name text since they exist
        //Nor is there need to set textBG.
        if (!curLine.isNarration)
        {
            dialogueBoxController.portrait.sprite = curLine.portrait;
            //deals with nameOverwrite
            dialogueBoxController.nameText.text = curLine.hasNameOverwrite ? curLine.nameOverwrite : curLine.character.CharacterName;
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
    
    //set to false in scene set up. 
    private bool haveFadedOut = false;
    
    /// <summary>
    /// Load the nextLine in the conversation
    /// </summary>
    private void NextLine()
    {
        //ignore all inputs if it is fading out. 
        if (isFadeingOut) return;
        
        //if end reached
        if (curLineIndex >= curConv.dialogueLines.Count)
        {   
            //set the correct fadeout color
            cover.color = curConv.hasWhiteTransition ? Color.white : Color.black;

            //fadeout if it is not done.
            if (!haveFadedOut)
            {
                StartCoroutine(FadeOut());
                haveFadedOut = true;
                
                //also plays transition sound for first time if it's dream transition
                if (curConv.hasExitSound || curConv.hasWhiteTransition)
                {
                    audioSource.Stop();
                    audioSource.PlayOneShot(curConv.exitSound);
                }
                return;
            }
            
            //if fade out had already been done, go to next conv and fade in. 
            NextConv();
            FadeIn();
            return;
        }


        DialogueLine curLine = CurLine();
        if (curLine.switchMusic)
        {
            audioSource.clip = curLine.musicToSwitchTo;
            audioSource.Play();
        }
        SetUpTextBox(curLine);
        ScrollToBottom();
        curLineIndex++;
    }

    private bool isFadeingOut = false;
    
    private IEnumerator FadeOut()
    {
        isFadeingOut = true;
        
        //the fadeout 
        fadeAnimation.Play("Transition fade out");
        while (fadeAnimation.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }

        isFadeingOut = false;
    }

    private void FadeIn()
    {
        fadeAnimation.Play("Transition fade in");
    }
    
    

    private void CamShake()
    {
        camAnim.SetTrigger("Shake");
    }
}
