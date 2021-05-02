using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBarHandler : MonoBehaviour
{
    public GameObject CanFront;
    public GameObject CanBack;
    public GameObject LifeBarHolder;
    public GameObject LifeBarHolderBack;

    private int _initLife;
    void Start()
    {
        _initLife = AppSingleton.Instance.LevelService.Player.LifeNumber;
        for (int i = 0; i < _initLife; ++i)
        {
            var goFront = GameObject.Instantiate(CanFront, LifeBarHolder.transform);
            var goBack = GameObject.Instantiate(CanBack, LifeBarHolderBack.transform);
        }
    }

    void Update()
    {
        if (AppSingleton.Instance.LevelService.Player.LifeNumber < LifeBarHolder.transform.childCount && LifeBarHolder.transform.childCount > 0)
        {
            Destroy(LifeBarHolder.transform.GetChild(0).gameObject);
        }
    }
}
