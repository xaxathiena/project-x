using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ConfigManager :Singleton<ConfigManager> 
{
#if UNITY_STANDALONE
    [SerializeField] private DataConfigRef standAloneConfigRef;
    
#else 
    [SerializeField] private DataConfigRef mobileConfigRef;
#endif

    private DataConfigRef configRef;
    public ConfigUnit ConfigUnit => configRef.ConfigUnit;
    
    // Start is called before the first frame update
    public void InitConfig(Action callback)
    {
#if UNITY_STANDALONE
        configRef =standAloneConfigRef;
#else 
        configRef = mobileConfigRef;
#endif
        StartCoroutine(configRef.CheckAllAssign(callback));
    }
}

[CreateAssetMenu(menuName = "Config/GameConfig/Data config ref", fileName = "ConfigRef", order = 1)]
public class DataConfigRef : ScriptableObject
{
    [FormerlySerializedAs("_configEnemy")] [SerializeField] private ConfigUnit configUnit;
    public ConfigUnit ConfigUnit => configUnit;

    public IEnumerator CheckAllAssign(Action callback)
    {
        yield return new WaitUntil(() => configUnit != null);
        callback?.Invoke();
    }
}