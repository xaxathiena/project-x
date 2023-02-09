using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PoolManager : MonoBehaviour
{
    public List<CustomPool> pools;
    public static Dictionary<string, CustomPool > dicPool = new Dictionary<string, CustomPool>();
    private static Dictionary<string, List<string>> dicPoolLoaded = new Dictionary<string, List<string>>();
    // Start is called before the first frame update
    
    void Start()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        //foreach(CustomPool pool in pools)
        //{
        //    CreatePrefab(pool);
        //    dicPool[pool.namePool] = pool;
        //    //dicPool.Add(pool.namePool, pool);
        //}
    }

    private void OnSceneUnloaded(Scene scene)
    {
        if(dicPoolLoaded.ContainsKey(scene.name))
        {
            foreach (var e in dicPoolLoaded[scene.name])
            {
                ReleasePool(e);
            }
            dicPoolLoaded.Remove(scene.name);
        }
    }
    public static bool CheckExist(string namePool)
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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pool"></param>
    /// <param name="isOverride"></param>
    /// <param name="isUnloadWhenSceneUnload">set it true if it never unload</param>
    public static void AddNewPool(CustomPool pool, bool isUnloadWhenSceneUnload = true)
    {
        if(!dicPool.ContainsKey(pool.namePool))
        {
            for (int i = 0; i < pool.total; i++)
            {
                Transform trans = Instantiate(pool.prefab, Vector3.zero, Quaternion.identity, pool.parent);
                pool.elements.Add(trans);
                //trans.SetParent(pool.parent);
                trans.gameObject.SetActive(false);
                pool.onInit?.Invoke(trans);
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

    public static List<T> AddNewPool<T>(CustomPool pool, bool isUnloadWhenSceneUnload = true)
    {
        List<T> rs = new List<T>();
        if (!dicPool.ContainsKey(pool.namePool))
        {
            for (int i = 0; i < pool.total; i++)
            {
                Transform trans = Instantiate(pool.prefab, Vector3.zero, Quaternion.identity, pool.parent);
                pool.elements.Add(trans);
                rs.Add(trans.GetComponent<T>());
                trans.gameObject.SetActive(false);
                pool.onInit?.Invoke(trans);
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
    public static CustomPool GetCustomPool(string namePool, Transform parent = null)
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
    public static Transform GetPool(string namePool, Transform parent = null)
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
    public static T GetPool<T>(string namePool, Transform parent = null)
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
//     public static T GetAsPool<T>(string namePool, Transform parent = null)
//     {
//         if (dicPool.ContainsKey(namePool))
//         {
//             return ((T)dicPool[namePool].OnSpawned(parent));
//         }
// #if UNITY_EDITOR
//         //Debug.LogErrorFormat("There is no pool name {0}", namePool);
// #endif
//         return default;
//     }
    public static bool IsHasPool(CustomPool pool)
    {
        return dicPool.ContainsKey(pool.namePool);
    }
    public static bool IsHasPool(string namePool)
    {
        return dicPool.ContainsKey(namePool);
    }
    public static void ExpandPool(string namePool, int number = 1)
    {
        if(dicPool.ContainsKey(namePool))
        {
            var pool = dicPool[namePool];
            for (int i = 0; i < number; i++)
            {
                Transform trans = Instantiate(pool.prefab, Vector3.zero, Quaternion.identity);
                pool.elements.Add(trans);
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
    public void CreatePrefab(CustomPool pool )
    {
        for (int i = 0; i < pool.total; i++)
        {
            Transform trans = Instantiate(pool.prefab, Vector3.zero, Quaternion.identity);
            pool.elements.Add(trans);
            trans.gameObject.SetActive(false);
        }
    }
    public static void ReleasePool(string poolName)
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
    public static void DisablePool(string poolName)
    {
        if (dicPool.ContainsKey(poolName))
        {
            var pool = dicPool[poolName];
            pool.DespawnAll();
        }
    }
    public static void DespawnPool(string poolName, Transform trans)
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


