using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Baiscally we put constraint on what T can be. T can only be a SingletonScriptableObject<T>..Russian Doll Feels..
/// T itself needs to inherit from SingletonScriptableObject, T itself needs to be singleton. It makes sense with an example.
/// public class GameSettings : SingletonScriptableObject<GameSettings> would satisfy the where constraint as GameSettings is T, and it inherits from
/// SingletonScriptableObject, thus T is also of type SingletonScriptableObject 
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                T[] assets = Resources.LoadAll<T>("");
                if (assets == null || assets.Length < 1)
                {
                    throw new System.Exception("Could not find any singleton scriptable object instances in Resources");
                } else if (assets.Length > 1)
                {
                    Debug.LogError("Multiple instances of the singleton scriptable object found in the resources.");
                }

                _instance = assets[0];
                //so singleton doesn't accidentally be garbage collected
                _instance.hideFlags = HideFlags.DontUnloadUnusedAsset;
            }
            return _instance;
        }
    }
}
