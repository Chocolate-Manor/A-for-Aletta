using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DialogueParser;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public GameObject panel;
    
    public GameObject textbox;
    public GameObject button;
    public GameObject image;
    public Image portrait;
    public ScrollRect scroll;
    
    public AudioSource audioSource;
    public AudioClip clickSound;
    public AudioClip flipSound;
    
    private int curDialogueIndex = 0;
    private Dialogue curDialogue;
    private Node curNode;
    private Character curCharacter;

    public delegate void NodeEnteredHandler(Node node);
    public event NodeEnteredHandler NodeEnteredEvent;
    
    private void Start()
    {   
        //event set-up
        NodeEnteredEvent += OnNodeEntered;
        isTyping = false;
        
        Clear();
        InitializeNextDialogue();

    }

    private void InitializeNextDialogue()
    {
        curDialogue = new Dialogue(DialogueManager.Instance.dialogues[curDialogueIndex]);
        curDialogueIndex++;
        curNode = curDialogue.GetStartNode();
        
        //invoke delegate event
        NodeEnteredEvent(curNode);
    }

    private void OnNodeEntered(Node node)
    {   
        audioSource.PlayOneShot(flipSound);
        SetupTextBoxAndPortrait(node);

        StartCoroutine(WaitForTypingFinishThenSetUpResponses(node));
        
        //scroll to the bottom
        Canvas.ForceUpdateCanvases();
        scroll.velocity = new Vector2 (0f, 500f);
    }

    private IEnumerator WaitForTypingFinishThenSetUpResponses(Node node)
    {
        yield return new WaitUntil(() => !isTyping);
        SetupResponses(node);
    }
    
    

    private bool isTyping;
    private IEnumerator Typewrite(string text, TextBoxController textBoxController)
    {
        isTyping = true;
        textBoxController.tmp.text = "";
        
        string[] lines = text.Split("\n"[0]);
        foreach (string line in lines)
        {
            foreach (char c in line)
            {
                textBoxController.tmp.text += c;
                
                //play audio
                // if (c != ' ')
                // {
                //     audioSource.PlayOneShot(curCharacter.characterVoice);
                // }

                yield return new WaitForSeconds(0.05f);
            }

            textBoxController.tmp.text += "\n";
            yield return new WaitForSeconds(0.1f);
        }
        isTyping = false;
    }

    private void SetupTextBoxAndPortrait(Node node)
    {   
        //instantiate and gain control
        GameObject curTextbox = Instantiate(textbox, panel.transform);
        TextBoxController textBoxController = curTextbox.GetComponent<TextBoxController>();
        
        //get current character data
        curCharacter = CharacterManager.Instance.GetCharacterByName(node.nodeInfo.Character);
        
        //set textbox & portrait
        textBoxController.textBoxImage.sprite = curCharacter.GetTextBoxByName(node.nodeInfo.TextBox);
        portrait.sprite = curCharacter.GetPortraitByName(node.nodeInfo.Portrait);
        
        //set text
        StartCoroutine(Typewrite(node.dialogueText, textBoxController));
    }

    private void SetupResponses(Node node)
    {
        List<Button> buttonsOfCurNode = new List<Button>(); 
            
        for (int i = 0; i < node.Responses.Count; i++)
        {
            //Instantiate & gain control
            GameObject curButton = Instantiate(button, panel.transform);
            ButtonController buttonController = curButton.GetComponent<ButtonController>();
            
            //Set-up visual
            buttonController.tmp.text = (i+1) + ". " + node.Responses[i].DisplayText;

            //Set-up on-click event
            //responseIndex reference is necessary because c# lambda
            buttonsOfCurNode.Add(buttonController.button);
            int responseIndex = i;
            buttonController.button.onClick.AddListener(delegate { ChooseResponse(responseIndex, buttonsOfCurNode); });
        }
    }
    

    private void ChooseResponse( int responseIndex, List<Button> buttonsOfCurNode)
    {   
        audioSource.PlayOneShot(clickSound);
        string nextNodeID = curNode.Responses[responseIndex].DestinationNode;
        curNode = curDialogue.GetNode(nextNodeID);
        
        //disable all buttons of current node
        foreach (Button button in buttonsOfCurNode)
        {
            button.interactable = false;
        }
        
        //invoke delegate event
        NodeEnteredEvent(curNode);
    }
    
    
    

    private void Clear()
    {
        foreach (Transform child in panel.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
