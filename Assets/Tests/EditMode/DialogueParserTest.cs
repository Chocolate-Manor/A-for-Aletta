using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DialogueParserTest
{
    private TextAsset textAsset;
    private DialogueParser.Dialogue dialogue;

    [OneTimeSetUp]
    public void Setup()
    {
        textAsset = Resources.Load<TextAsset>("testDialogue");
        dialogue = new DialogueParser.Dialogue(textAsset);
    }

    [Test]
    public void Correct_Number_Of_Nodes_Read()
    {
        Assert.AreEqual(6, dialogue.Nodes.Count);
    }

    [Test]
    public void Correct_Start_Node_Home_Read()
    {
        //titleOfStart is home
        Assert.AreEqual("Home", dialogue.TitleOfStartNode);

        //getStartNode get's the Home node.
        DialogueParser.Node homeNode = dialogue.GetStartNode();
        Assert.AreEqual("Home", homeNode.Title);

        //normal GetNode also get Home node.
        Assert.AreEqual("Home", dialogue.GetNode("Home").Title);

        //tags
        Assert.IsTrue(homeNode.tags.Count == 4);
        Assert.IsTrue(homeNode.tags[0] == "Ning");
        Assert.IsTrue(homeNode.tags[1] == "Diary");
        Assert.IsTrue(homeNode.tags[2] == "Indifferent");
        Assert.IsTrue(homeNode.tags[3] == "START");

        //node info
        Assert.AreEqual("Ning", homeNode.nodeInfo.Character);
        Assert.AreEqual("Diary", homeNode.nodeInfo.TextBox);
        Assert.AreEqual("Indifferent", homeNode.nodeInfo.Portrait);

        //dialogueText
        Assert.AreEqual(homeNode.dialogueText, @"Time to wake up.
It's school day again.
Just Another day.
Heart without love.");
        
        //responses
        Assert.AreEqual(homeNode.Responses[0].DisplayText, "Take train");
        Assert.AreEqual(homeNode.Responses[0].DestinationNode, "Train");
        Assert.AreEqual(homeNode.Responses[1].DisplayText, "Jog to school");
        Assert.AreEqual(homeNode.Responses[1].DestinationNode, "Jog");
        Assert.AreEqual(homeNode.Responses[2].DisplayText, "Bike to school");
        Assert.AreEqual(homeNode.Responses[2].DestinationNode, "Bike");
        
        //not end node
        Assert.AreEqual(false, homeNode.isEndNode());
    }

    [Test]
    public void Correct_End_Node_Lost_Read()
    {
        DialogueParser.Node lostNode = dialogue.GetNode("Lost");
        
        //title
        Assert.AreEqual("Lost", lostNode.Title);
        
        //tags
        Assert.IsTrue(lostNode.tags.Count == 4);
        Assert.AreEqual("Ning", lostNode.tags[0]);
        Assert.AreEqual("Speech", lostNode.tags[1]);
        Assert.AreEqual("Worried", lostNode.tags[2]);
        Assert.AreEqual("END", lostNode.tags[3]);
        
        //node Info
        Assert.AreEqual("Ning", lostNode.nodeInfo.Character);
        Assert.AreEqual("Speech", lostNode.nodeInfo.TextBox);
        Assert.AreEqual("Worried", lostNode.nodeInfo.Portrait);
        
        //dialogueText
        Assert.AreEqual("Oh no. I am lost. I have no clue where I am.", lostNode.dialogueText);
        
        //Responses
        Assert.AreEqual(0, lostNode.Responses.Count);
        
        //isEndNode
        Assert.AreEqual(true, lostNode.isEndNode());
        
    }

    [Test]
    public void Correct_End_Node_School_Read()
    {
        DialogueParser.Node SchoolNode = dialogue.GetNode("School");
        
        //title
        Assert.AreEqual("School", SchoolNode.Title);
        
        //tags
        Assert.IsTrue(SchoolNode.tags.Count == 4);
        Assert.AreEqual("Ning", SchoolNode.tags[0]);
        Assert.AreEqual("Thought", SchoolNode.tags[1]);
        Assert.AreEqual("Indifferent", SchoolNode.tags[2]);
        Assert.AreEqual("END", SchoolNode.tags[3]);
        
        //node Info
        Assert.AreEqual("Ning", SchoolNode.nodeInfo.Character);
        Assert.AreEqual("Thought", SchoolNode.nodeInfo.TextBox);
        Assert.AreEqual("Indifferent", SchoolNode.nodeInfo.Portrait);
        
        //dialogueText
        Assert.AreEqual(@"Time for another busy school day.
And having to look at the girl you like from a distance.", SchoolNode.dialogueText);
        
        //Responses
        Assert.AreEqual(0, SchoolNode.Responses.Count);
        
        //isEndNode
        Assert.AreEqual(true, SchoolNode.isEndNode());
    }
}