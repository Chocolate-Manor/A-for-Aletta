using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : SingletonMonoBehavior<DontDestroy>
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
    
}
