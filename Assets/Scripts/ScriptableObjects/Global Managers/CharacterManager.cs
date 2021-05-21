using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Manager", menuName = "Scriptable Objects/Character Manager")]
public class CharacterManager : SingletonScriptableObject<CharacterManager>
{
    public List<string> characterNameIndex;
    public List<Character> characters;

    public Character GetCharacterByName(string characterName)
    {
        if (characterNameIndex.Count != characters.Count) throw new Exception("NameIndex size and the amount of characters does not match");
        int i = characterNameIndex.IndexOf(characterName); 
        if (i == -1) throw new Exception("character name not found in characterNameIndex.");

        return characters[i];
    }
    
}
