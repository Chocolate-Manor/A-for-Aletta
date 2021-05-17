using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParser
{
    private const string StartTag = "START";
    private const string EndTag = "End";
    
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

    public struct NodeInfo
    {
        public string Character;
        public string TextBoxType;
        public string Emotion;
    }
    

    public class Node
    {
        public string Title;
        public List<string> tags;
        public NodeInfo nodeInfo;
        public string text;
        public List<Response> Responses;

        public bool isEndNode()
        {
            return tags.Contains(EndTag);
        }
    }
}
