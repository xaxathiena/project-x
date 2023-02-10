using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolCreaterBase<T> : MonoBehaviour where T: MonoBehaviour
{
    public CustomPool<T> pool;
    public bool initInStart = false;
    private PoolChunkManager<T> chuckManager;
    public void Start()
    {
        chuckManager = PoolManager.instance.GetPoolChunkManager<T>() as PoolChunkManager<T>;
        if(initInStart)
            CreatePool();
    }

    public void CreatePool()
    {
        chuckManager.AddNewPool(pool);
    }
    public T GetPoolElement()
    {
        return chuckManager.GetPool(pool.namePool);
    }    
}
