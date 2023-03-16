using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PoolChunkManager<T> where T: MonoBehaviour
{
    public Dictionary<string, CustomPool<T> > dicPool = new Dictionary<string, CustomPool<T>>();
    private Dictionary<string, List<string>> dicPoolLoaded = new Dictionary<string, List<string>>();
    
    public bool CheckExist(string namePool)
    {
        if (dicPool.ContainsKey(namePool))
        {
            return true;
        }
#if UNITY_EDITOR
        Debug.LogErrorFormat("There is no pool name {0}", namePool);
#endif
        return false;
    }
    public void AddNewPool(CustomPool<T> pool, bool isUnloadWhenSceneUnload = true)
    {
        if(!dicPool.ContainsKey(pool.namePool))
        {
            for (int i = 0; i < pool.total; i++)
            {
                T trans = MonoBehaviour.Instantiate(pool.prefab, Vector3.zero, Quaternion.identity, pool.parent);
                pool.elements.Add(trans.GetComponent<T>());
                //trans.SetParent(pool.parent);
                trans.gameObject.SetActive(false);
                pool.onInit?.Invoke(trans.transform);
            }
            dicPool[pool.namePool] = pool;
        }
        else
        {
            Debug.LogError("Already had a pool name " + pool.namePool);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pool"></param>
    /// <param name="isOverride"></param>
    /// <param name="isUnloadWhenSceneUnload">set it true if it never unload</param>

    public List<T> AddNewPoolThenGet(CustomPool<T> pool, bool isUnloadWhenSceneUnload = true)
    {
        List<T> rs = new List<T>();
        if (!dicPool.ContainsKey(pool.namePool))
        {
            for (int i = 0; i < pool.total; i++)
            {
                T trans =MonoBehaviour.Instantiate(pool.prefab, Vector3.zero, Quaternion.identity, pool.parent);
                pool.elements.Add(trans.GetComponent<T>());
                rs.Add(trans.GetComponent<T>());
                trans.gameObject.SetActive(false);
                pool.onInit?.Invoke(trans.transform);
            }
            dicPool[pool.namePool] = pool;
        }
        else
        {
            var _pool = dicPool[pool.namePool];
            for (int i = 0; i < _pool.total; i++)
            {
                rs.Add(_pool.elements[i].GetComponent<T>());
            }
        }
        return rs;

    }
    public CustomPool<T> GetCustomPool(string namePool, Transform parent = null)
    {
        if (dicPool.ContainsKey(namePool))
        {
            var go = dicPool[namePool].OnSpawned(parent);
            go.gameObject.SetActive(false);
            return dicPool[namePool];
        }
#if UNITY_EDITOR
        Debug.LogErrorFormat("There is no pool name {0}", namePool);
#endif
        return null;
    }
    public T GetPool(string namePool, Transform parent = null)
    {
        if (dicPool.ContainsKey(namePool))
        {
            return dicPool[namePool].OnSpawned(parent);
        }
#if UNITY_EDITOR
        Debug.LogErrorFormat("There is no pool name {0}", namePool);
#endif
        return null;
    }
    public T GetPool<T>(string namePool, Transform parent = null)
    {
        if (dicPool.ContainsKey(namePool))
        {
            return dicPool[namePool].OnSpawned(parent).GetComponent<T>();
        }
#if UNITY_EDITOR
        //Debug.LogErrorFormat("There is no pool name {0}", namePool);
#endif
        return default;
    }
    public bool IsHasPool(CustomPool<T> pool)
    {
        return dicPool.ContainsKey(pool.namePool);
    }
    public bool IsHasPool(string namePool)
    {
        return dicPool.ContainsKey(namePool);
    }
    public void ExpandPool(string namePool, int number = 1)
    {
        if(dicPool.ContainsKey(namePool))
        {
            var pool = dicPool[namePool];
            for (int i = 0; i < number; i++)
            {
                T trans = MonoBehaviour.Instantiate(pool.prefab, Vector3.zero, Quaternion.identity);
                pool.elements.Add(trans.GetComponent<T>());
                trans.gameObject.SetActive(false);
            }
        }
#if UNITY_EDITOR
        else
        {
            Debug.LogErrorFormat("There is no pool name {0}", namePool);
        }
#endif
    }
    public void CreatePrefab(CustomPool<T> pool )
    {
        for (int i = 0; i < pool.total; i++)
        {
            T trans =  MonoBehaviour.Instantiate(pool.prefab, Vector3.zero, Quaternion.identity);
            pool.elements.Add(trans.GetComponent<T>());
            trans.gameObject.SetActive(false);
        }
    }
    public void ReleasePool(string poolName)
    {
        if(dicPool.ContainsKey(poolName))
        {
            try
            {
                var pool = dicPool[poolName];
                if (pool != null)
                {
                    pool.Dispose();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("ReleasePool error: " + ex);
            }
            dicPool.Remove(poolName);
        }
    }
    public void DisablePool(string poolName)
    {
        if (dicPool.ContainsKey(poolName))
        {
            var pool = dicPool[poolName];
            pool.DespawnAll();
        }
    }
    public void DespawnPool(string poolName, T trans)
    {
        if(dicPool.ContainsKey(poolName))
        {
            dicPool[poolName].OnDespawned(trans);
        }
#if UNITY_EDITOR
        else
        {
            Debug.LogErrorFormat("There is no pool name {0}", poolName);
        }
#endif
    }
    
}



public class PoolManager : Singleton<PoolManager>
{
    private PoolChunkManager<BulletControl> _bulletControl = new PoolChunkManager<BulletControl>();
    private PoolChunkManager<PowerBulletControl> _powerBallControl = new PoolChunkManager<PowerBulletControl>();
    private PoolChunkManager<ObjectPoolControl> _ObjectPoolControl = new PoolChunkManager<ObjectPoolControl>();
    
    public object GetPoolChunkManager<T>() where T: MonoBehaviour
    {
        Type getType = typeof(T);
        if (getType == typeof(BulletControl))
        {
            return _bulletControl;
        }
        if (getType == typeof(PowerBulletControl))
        {
            return _powerBallControl;
        }

        if (getType == typeof(ObjectPoolControl))
        {
            return _ObjectPoolControl;
        }
        return default;
    }
    public T GetPool<T>(string namePool, Transform parent = null) where T: MonoBehaviour
    {
        Type getType = typeof(T);
        if (getType == typeof(BulletControl))
        {
            return _bulletControl.GetPool<T>(namePool, parent);
        }
        if (getType == typeof(PowerBulletControl))
        {
            return _powerBallControl.GetPool<T>(namePool, parent);
        }
        if (getType == typeof(ObjectPoolControl))
        {
            return _ObjectPoolControl.GetPool<T>(namePool, parent);;
        }
        return default;
    }
}


