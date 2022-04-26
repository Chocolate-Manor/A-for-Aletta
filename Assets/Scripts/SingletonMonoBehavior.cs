using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehavior<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    
    /// <summary>
    /// PROTECTED means it is not public, but nor it is private (hidden). Only its derived class can access it.
    /// VIRTUAL means 
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        } 
    }
}