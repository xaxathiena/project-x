using System.Collections;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Config/GameConfig/Data config ref", fileName = "ConfigRef", order = 1)]
public class DataConfigRef : ScriptableObject
{
    [SerializeField] private ConfigUnit configUnit;
    [SerializeField] private ConfigWaves  configWaves;
    
    
    public ConfigUnit ConfigUnit => configUnit;
    public ConfigWaves ConfigWaves => configWaves;

    public IEnumerator CheckAllAssign(Action callback)
    {
        yield return new WaitUntil(() => configUnit != null);
        yield return new WaitUntil(() => configWaves != null);
        callback?.Invoke();
    }
}
