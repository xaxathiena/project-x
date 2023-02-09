using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolCreater : MonoBehaviour
{
    public CustomPool pool;
    public bool initInStart = false;
    public void Start()
    {
        if(initInStart)
            CreatePool();
    }

    public void CreatePool()
    {
        PoolManager.AddNewPool(pool);
    }
    public Transform GetPoolElement()
    {
        return PoolManager.GetPool(pool.namePool);
    }    
}
