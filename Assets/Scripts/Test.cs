using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    void Start()
    {
        Character Floris = CharacterManager.Instance.GetCharacterByName("Floris");
        Debug.Log(Floris.characterName);
    }


}
