using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.XR;

public class DialogueParser
{
    private const string StartTag = "START";
    private const string EndTag = "END";

    public struct Response
    {
        public string DisplayText { get; private set; }
        public string DestinationNode { get; private set; }

        public Response(string displayText, string destinationNode)
        {
            DisplayText = displayText;
            DestinationNode = destinationNode;
        }
    }

    /// <summary>
    /// The tags ought to follow the 1.Character, 2.TextBox, 3.Portrait order. Maybe 4.Start or End 
    /// </summary>
    public struct NodeInfo
    {
        public string Character;
        public string TextBox;
        public string Portrait;

        public NodeInfo(string character, string textBox, string portrait)
        {
            Character = character;
            TextBox = textBox;
            Portrait = portrait;
        }
    }


    public class Node
    {
        public string Title;
        public List<string> tags;
        public NodeInfo nodeInfo;
        public string dialogueText;
        public List<Response> Responses;
        
        public bool isEndNode()
        {
            return tags.Contains(EndTag);
        }
    }

    public class Dialogue
    {
        public Dictionary<string, Node> Nodes;
        public string TitleOfStartNode;

        public Dialogue(TextAsset textAsset)
        {
            Nodes = new Dictionary<string, Node>();
            Parse(textAsset);
        }

        public Node GetNode(string nodeTitle)
        {
            return Nodes[nodeTitle];
        }

        public Node GetStartNode()
        {
            UnityEngine.Assertions.Assert.IsNotNull( TitleOfStartNode );
            return Nodes [ TitleOfStartNode ];
        }
        
        /// <summary>
        /// End nodes do not have responses. 
        /// </summary> [[message|destinationNode]]
        /// <param name="reader">the current reader</param>
        /// <param name="curNode">reference of the current node</param>
        private void ParseAndAddResponses(StringReader reader,  Node curNode)
        {
            curNode.Responses = new List<Response>();
            if (!curNode.isEndNode())
            {
                while (reader.Peek() != -1)
                {
                    string curLine = reader.ReadLine();

                    if (curLine.Length > 0) //line might be empty
                    {
                        int displayTextStart = curLine.IndexOf("[[") + 2;
                        int displayTextEnd = curLine.IndexOf("|") - displayTextStart;
                        string displayText = curLine.Substring(displayTextStart, displayTextEnd).Trim();

                        int destinationNodeStart = curLine.IndexOf("|") + 1;
                        int destinationNodeEnd = curLine.IndexOf("]]") - destinationNodeStart;
                        string destinationNode = curLine.Substring(destinationNodeStart, destinationNodeEnd).Trim();
                                
                        curNode.Responses.Add(new Response(displayText, destinationNode));
                    }
                            
                }
            }
        }
        

        public void Parse(TextAsset textAsset)
        {
            string text = textAsset.text;
            string[] nodeData = text.Split(new string[] {"::"}, StringSplitOptions.None);

            const int indexOfContentStart = 5;
            for (int i = indexOfContentStart; i < nodeData.Length; i++)
            {
                string curNodeText = nodeData[i];
                using (StringReader reader = new StringReader(curNodeText))
                {
                    //title
                    string firstLine = reader.ReadLine();
                    string title = firstLine.Substring(0, firstLine.IndexOf("[")).Trim();
                    
                    //tags
                    int tagStartIndex = firstLine.IndexOf("[") + 1;
                    int tagEndIndex = firstLine.IndexOf("]");
                    string tagsString = firstLine.Substring(tagStartIndex, tagEndIndex - tagStartIndex);
                    List<string> tags = new List<string>(tagsString.Split(new string[] {" "}, StringSplitOptions.None));

                    //node info
                    NodeInfo nodeInfo = new NodeInfo(tags[0], tags[1], tags[2]);
                    
                    //text
                    int dialogueStart = curNodeText.IndexOf("}") + 1;
                    int dialogueEnd = curNodeText.IndexOf("[[") == -1 ? curNodeText.Length : curNodeText.IndexOf("[["); 
                    string dialogueText = curNodeText.Substring(dialogueStart, dialogueEnd - dialogueStart).Trim();
                    int dialogueTextLength = dialogueText.Split('\n').Length;
                    for (int k = 0; k < dialogueTextLength; k++)
                    {   
                        //skip over the lines that had the dialogue.
                        reader.ReadLine();
                    }

                    //make the node
                    Node curNode = new Node();
                    curNode.Title = title;
                    curNode.tags = tags;
                    curNode.nodeInfo = nodeInfo;
                    curNode.dialogueText = dialogueText;

                    //set the starting title if applies
                    if (curNode.tags.Contains(StartTag))
                    {
                        UnityEngine.Assertions.Assert.IsTrue(null == TitleOfStartNode);
                        TitleOfStartNode = curNode.Title;
                    }
                    
                    //parse responses and add them to current Node
                    ParseAndAddResponses(reader, curNode);
                    
                    //add node to dictionary finally
                    Nodes[curNode.Title] = curNode;
                }
            }
        }
    }
}