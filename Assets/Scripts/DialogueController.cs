using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DialogueParser;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public GameObject Panel;
    
    public GameObject Textbox;
    public GameObject Button;
    public GameObject Image;
    public Image Portrait;
    public ScrollRect scroll;
    
    private int curDialogueIndex = 0;
    private Dialogue curDialogue;
    private Node curNode;

    public delegate void NodeEnteredHandler(Node node);
    public event NodeEnteredHandler NodeEnteredEvent;
    
    private void Start()
    {   
        //event set-up
        NodeEnteredEvent += OnNodeEntered;
        
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
        SetupTextBoxAndPortrait(node);
        
        SetupResponses(node);
        
        //scroll to the bottom
        Canvas.ForceUpdateCanvases();
        scroll.velocity = new Vector2 (0f, 2000f);
    }

    private void SetupTextBoxAndPortrait(Node node)
    {   
        //instantiate and gain control
        GameObject curTextbox = Instantiate(Textbox, Panel.transform);
        TextBoxController textBoxController = curTextbox.GetComponent<TextBoxController>();
        
        //get current character data
        Character curCharacter = CharacterManager.Instance.GetCharacterByName(node.nodeInfo.Character);
        
        //set textbox & portrait
        textBoxController.textBoxImage.sprite = curCharacter.GetTextBoxByName(node.nodeInfo.TextBox);
        Portrait.sprite = curCharacter.GetPortraitByName(node.nodeInfo.Portrait);
        
        //set text
        textBoxController.tmp.text = node.dialogueText;
    }

    private void SetupResponses(Node node)
    {
        List<Button> buttonsOfCurNode = new List<Button>(); 
            
        for (int i = 0; i < node.Responses.Count; i++)
        {
            //Instantiate & gain control
            GameObject curButton = Instantiate(Button, Panel.transform);
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
        foreach (Transform child in Panel.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
