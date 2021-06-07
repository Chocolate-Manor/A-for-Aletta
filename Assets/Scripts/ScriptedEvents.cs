using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedEvents : MonoBehaviour
{
    private void Awake()
    {
        UniversalInfo.nextSceneEvent += Transition;
    }

    private void Transition(int prevSceneIndex)
    {
        switch (prevSceneIndex)
        {
            case 0:
                Debug.Log("Conv 0=>1 transition");
                break;
            case 1:
                Debug.Log("Conv 1=>2 transition");
                break;
            default:
                throw new ArgumentException("Unknown scene transition index");
        }
    }
}
