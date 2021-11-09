using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UniversalInfo
{
    public static int curConvIndex = 0;
    
    //public static delegate when next scene happens
    public delegate void nextConvDel(int curConvIndex);
    public static nextConvDel nextSceneEvent = delegate {};

    /// <summary>
    /// Increment the current convIndex and save it inside playerprefs.
    /// </summary>
    public static void Increment_ConvIndex_And_Save()
    {
        curConvIndex++;
        PlayerPrefs.SetInt("curConvIndex", curConvIndex);
    }
    
    /// <summary>
    /// Load the current ConvIndex to see 
    /// </summary>
    public static int Load_ConvIndex()
    {
        curConvIndex = PlayerPrefs.GetInt("curConvIndex");
        return curConvIndex;
    }
}
