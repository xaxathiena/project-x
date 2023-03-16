using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Compare
{
    public static int ComparePosition(Transform x, Transform y)
    {
        return -1;
    }
}

public static class Utility
{
    #region Extension methods

    public static bool IsPositionInRange(this Vector3 center, Vector3 targetPosition, float range, float boderRange = 0f)
    {
        var distance = (center.x - targetPosition.x).sqr() + (center.y - targetPosition.y).sqr() +
                       (center.z - targetPosition.z).sqr();
        
        return distance < (range + boderRange).sqr();
    }

    public static float DistanceSQR(this Vector3 start, Vector3 from)
    {
        return (start.x - from.x).sqr() + (start.y - from.y).sqr() +
               (start.z - from.z).sqr();
    }
    public static float sqr(this float value)
    {
        return value * value;
    }

    public static void DelayToDo(float time, Action action)
    {
        DontDestroyOnLoad.Instance.StartCoroutine(IEDeplay(time, action));
    }

    public static T GetRandom<T>(this List<T> list)
    {
        if (list == null) return default;
        return list[Random.Range(0, list.Count)];
    }
    public static T GetRandom<T>(this T[] list)
    {
        if (list == null) return default;
        return list[Random.Range(0, list.Length)];
    }
    #endregion
    
    
    
    
    
    private static IEnumerator IEDeplay(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }
}