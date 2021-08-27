using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Conversation", menuName = "Scriptable Objects/Conversation")]
public class Conversation : ScriptableObject
{
    public string sceneHeading;
    public AudioClip backgroundMusic;

    public bool hasWhiteTransition;
    public bool bloomOn;
    
    public bool hasEnterSound;
    public AudioClip enterSound;
    public bool hasExitSound;
    public AudioClip exitSound;

    public bool isLastInEpisode;
    [TextArea(2, 10)] public string nextEpisodeHeading;
    
    public List<DialogueLine> dialogueLines;

    public bool Valid => dialogueLines != null && dialogueLines.Count > 0;
    
    public void AssignDefaults()
    {
        for (int i = 0; i < dialogueLines.Count; i++)
        {
            DialogueLine curLine = dialogueLines[i];
            if (curLine.portrait == null && !curLine.isNarration)
            {
                dialogueLines[i] = new DialogueLine();
            }
        }
    }
}
