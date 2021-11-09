using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


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
    
    //episode sound
    public AudioClip episodeSound;
    public TextMeshProUGUI episodeText;
    public GameObject episodeTextBox;
    
    //audio fade animator
    public Animation audioFadeAnimation;
    
    private void Awake()
    {   
        //for testing
        //PlayerPrefs.SetInt("curConvIndex", 41);
        
        //load the current conversation
        UniversalInfo.Load_ConvIndex();
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

        NextLine();

        haveFadedOut = false;
        haveDisplayedEpisodeText = false;
    }
    
    /// <summary>
    /// Load the next scene for the conversation
    /// </summary>
    private void NextConv()
    {   
        //ifIndexExceeds the length itself, return to title page and reset index to zero.
        if (UniversalInfo.curConvIndex >= DialogueManager.Instance.dialogues.Count - 1)
        {
            SceneManager.LoadScene("Main Menu");
            UniversalInfo.curConvIndex = 0;
            PlayerPrefs.SetInt("curConvIndex", 0);
            return;
        }
        
        //trigger the nextConv event
        UniversalInfo.nextSceneEvent(UniversalInfo.curConvIndex);
        
        //increment
        //UniversalInfo.curConvIndex++;
        UniversalInfo.Increment_ConvIndex_And_Save();
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
    
    
    /// <summary>
    /// set episode text if suitable, otherwise empty. Change text color accordingly due to transition color. 
    /// </summary>
    private void SetUpEpisodeHeading()
    {
        if (curConv.isLastInEpisode)
        {
            if (curConv.hasWhiteTransition)
            {
                episodeText.color = Color.black;
            }
            else
            {
                episodeText.color = Color.white;
            }
        }
        else
        {
            episodeText.text = "";
        }
    }
    
    
    //set to false in scene set up. 
    private bool haveFadedOut;
    private bool haveDisplayedEpisodeText;
    
    /// <summary>
    /// Load the nextLine in the conversation
    /// </summary>
    private void NextLine()
    {
        //ignore all inputs if it is fading out or displaying episode text
        if (isFadeingOut || isTypingEpisodeText) return;

        //if end reached
        if (curLineIndex >= curConv.dialogueLines.Count)
        {
            cover.color = curConv.hasWhiteTransition ? Color.white : Color.black;
            SetUpEpisodeHeading();

            if (!haveFadedOut)
            {
                StartCoroutine(FadeOut());
                haveFadedOut = true;
                
                if (curConv.hasExitSound || curConv.hasWhiteTransition)
                {
                    audioSource.Stop();
                    audioSource.PlayOneShot(curConv.exitSound);
                }
                return;
            }
            
            //if fade out had already been done, go to next conv and fade in. 
            if (!curConv.isLastInEpisode || haveDisplayedEpisodeText)
            {
                //if not the last episode, then just fade in
                NextConv();
                FadeIn();
                return;
            }
            else
            {
                //if is last episode, first display text
                volumeObject.SetActive(false);
                StartCoroutine(TypeWriteEpisodeText(curConv.nextEpisodeHeading, episodeText));
                haveDisplayedEpisodeText = true;
                return;
            }
        }
        
        DialogueLine curLine = CurLine();
        if (curLine.switchMusic)
        {
            StartCoroutine(SwitchMusicWithTransition(curLine.musicToSwitchTo));
        }
        SetUpTextBox(curLine);
        ScrollToBottom();
        curLineIndex++;
    }
    
    /// <summary>
    /// fade out the previous music, but no need to actually fade into the new music since it should start with its start.
    /// </summary>
    /// <param name="clip"></param>
    /// <returns></returns>
    private IEnumerator SwitchMusicWithTransition(AudioClip clip)
    {
        audioFadeAnimation.Play("Audio fade out");
        while (audioFadeAnimation.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }
        audioSource.clip = clip;
        audioSource.Play();
        audioFadeAnimation.Play("Audio fade in");
        while (audioFadeAnimation.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }
    }
    
    
    private bool isTypingEpisodeText;
    private IEnumerator TypeWriteEpisodeText(string text, TextMeshProUGUI textmesh)
    {
        isTypingEpisodeText = true;
        SetUpEpisodeHeading();
        episodeTextBox.SetActive(true);
        audioSource.Stop();
        audioSource.PlayOneShot(episodeSound);
        textmesh.text = "";

        string[] lines = text.Split("\n"[0]);
        foreach (string line in lines)
        {
            string lineTrim = line.Trim();
            foreach (char c in lineTrim)
            {
                textmesh.text += c;
                yield return new WaitForSeconds(0.08f);
            }
            textmesh.text += " ";
            yield return new WaitForSeconds(0.4f);
        }
        isTypingEpisodeText = false;
    }
    
    
    

    private bool isFadeingOut = false;
    
    private IEnumerator FadeOut()
    {
        isFadeingOut = true;
        episodeTextBox.SetActive(false);
        
        //the fadeout 
        fadeAnimation.Play("Transition fade out");
        while (fadeAnimation.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }

        isFadeingOut = false;
    }
    
    /// <summary>
    /// It plays the fade in animation.
    /// And because it is after next conv is called, it can play the correct enter sound. 
    /// </summary>
    private void FadeIn()
    {
        fadeAnimation.Play("Transition fade in");
        //play enter sound;
        if (curConv.hasEnterSound)
        {
            audioSource.PlayOneShot(curConv.enterSound);
        }
    }
    
    

    private void CamShake()
    {
        camAnim.SetTrigger("Shake");
    }
}
