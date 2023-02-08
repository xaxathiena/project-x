using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T: MonoBehaviour
{
    private static T instance_;
    public static T instance
    {

        get
        {
            return GetInstance();
        }
    }
    // Start is called before the first frame update
    private static T GetInstance()
    {
        if(instance_!=null)
        {
            return instance_;
        }
        else
        {
            GameObject go = new GameObject();

            go.AddComponent<T>();
            go.name = "[Singleton]" +typeof(T).ToString();
            instance_ = go.GetComponent<T>();
            return instance_;
        }
    }
    void Awake()
    {
        instance_ = gameObject.GetComponent<T>();
        OnAwake();
    }

    private void Reset()
    {
        gameObject.name = typeof(T).ToString();
    }
    protected virtual void OnAwake()
    {

    }
}
