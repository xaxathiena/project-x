using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootLoader : MonoBehaviour
{
    public static Action OnLoadConfigCompleteEvent;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        ConfigManager.instance.InitConfig(() =>
        {
            Debug.Log("Init config done");
            OnLoadConfigCompleteEvent?.Invoke();
        });
    }
}
