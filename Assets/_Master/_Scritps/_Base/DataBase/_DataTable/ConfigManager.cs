using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager :Singleton<ConfigManager> 
{
    public static Action OnLoadConfigCompleteEvent;

    // Start is called before the first frame update
    public void InitConfig(Action callback)
    {

        StartCoroutine(LoadConfig(callback));
    }
    IEnumerator LoadConfig(Action callback)
    {
        yield return new WaitForSeconds(1f);
        callback?.Invoke();
        OnLoadConfigCompleteEvent?.Invoke();
    }
}
