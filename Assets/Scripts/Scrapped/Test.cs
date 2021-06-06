using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public TextAsset twineTxt;
    
    void Start()
    {
        // Character Floris = CharacterManager.Instance.GetCharacterByName("Floris");
        // Debug.Log(Floris.characterName);

        DialogueParser.Dialogue dialogue = new DialogueParser.Dialogue(twineTxt);
    }


}
