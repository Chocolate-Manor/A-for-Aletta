using System.Collections;
using System.Collections.Generic;

public static class UniversalInfo
{
    public static int curConvIndex = 0;
    
    //public static delegate when next scene happens
    public delegate void nextConvDel(int curConvIndex);
    public static nextConvDel nextSceneEvent = delegate {};
}
