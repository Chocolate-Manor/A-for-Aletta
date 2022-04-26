using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamManager : MonoBehaviour
{
    private void Awake()
    {
        try
        {
            Steamworks.SteamClient.Init(1980400);
        }
        catch (System.Exception e)
        {
            Debug.Log("Couldn't initialize Steam Client");
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnDisable()
    {
       Steamworks.SteamClient.Shutdown(); 
    }

    private void Update()
    {
        //Possible callbacks for achievements. Man, achievements
       Steamworks.SteamClient.RunCallbacks(); 
    }
}
