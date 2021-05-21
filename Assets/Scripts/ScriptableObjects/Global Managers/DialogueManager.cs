using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue Manager", menuName = "Scriptable Objects/Dialogue Manager")]
public class DialogueManager : SingletonScriptableObject<DialogueManager>
{
    public List<TextAsset> dialogues;
}
