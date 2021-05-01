using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppSingleton : Singleton<AppSingleton>
{
    public LevelService LevelService;

    void Start()
    {
        
    }
}
