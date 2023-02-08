using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DataChangeTrigger : UnityEvent<object>
{

}
public class DataEventTrigger : UnityEvent<object[]>
{

}
public static class DataTrigger
{
    public static Dictionary<string, DataChangeTrigger> dicOnValueChange = new Dictionary<string, DataChangeTrigger>();
    public static Dictionary<string, DataChangeTrigger> dicServerDataChange = new Dictionary<string, DataChangeTrigger>();
    public static Dictionary<string, DataEventTrigger> dicServerEvent = new Dictionary<string, DataEventTrigger>();
    public static Dictionary<string, DataEventTrigger> dicClientEvent = new Dictionary<string, DataEventTrigger>();
    #region RegisterValue Base
    public static void RegisterValueChange(string s, UnityAction<object> delegateDataChange)
    {
        if (dicOnValueChange.ContainsKey(s))
        {
            dicOnValueChange[s].AddListener(delegateDataChange);
        }
        else
        {
            dicOnValueChange.Add(s, new DataChangeTrigger());
            dicOnValueChange[s].AddListener(delegateDataChange);
        }
    }

    //extention method 
    public static void TriggerEventData(this object data, string path)
    {
        if (dicOnValueChange.ContainsKey(path))
            dicOnValueChange[path].Invoke(data);
    }
    #endregion


    #region Register server data
    public static void RegisterServerDataChange(string s, UnityAction<object> delegateDataChange)
    {
        if (dicServerDataChange.ContainsKey(s))
        {
            dicServerDataChange[s].AddListener(delegateDataChange);
        }
        else
        {
            dicServerDataChange.Add(s, new DataChangeTrigger());
            dicServerDataChange[s].AddListener(delegateDataChange);
        }
    }
    //extention method 
    public static void TriggerServerData(this object data, string path)
    {
        if (dicServerDataChange.ContainsKey(path))
            dicServerDataChange[path].Invoke(data);
    }
    #endregion
    #region Register server event
    /// <summary>
    /// Register in client event send make by server
    /// </summary>
    /// <param name="key"></param>
    /// <param name="delegateDataChange"></param>
    public static void RegisterServerEvent(string key, UnityAction<object[]> delegateDataChange)
    {
        if (dicServerEvent.ContainsKey(key))
        {
            dicServerEvent[key].AddListener(delegateDataChange);
        }
        else
        {
            dicServerEvent.Add(key, new DataEventTrigger());
            dicServerEvent[key].AddListener(delegateDataChange);
        }
    }
    //extention method 
    public static void TriggerServerEvent(this string key,object[] datas)
    {
        if (dicServerEvent.ContainsKey(key))
            dicServerEvent[key].Invoke(datas);
    }
    #endregion

    #region Register client event
    /// <summary>
    /// Register int server event make by client
    /// </summary>
    /// <param name="key"></param>
    /// <param name="delegateDataChange"></param>
    public static void RegisterClientEvent(string key, UnityAction<object[]> delegateDataChange)
    {
        if (dicClientEvent.ContainsKey(key))
        {
            dicClientEvent[key].AddListener(delegateDataChange);
        }
        else
        {
            dicClientEvent.Add(key, new DataEventTrigger());
            dicClientEvent[key].AddListener(delegateDataChange);
        }
    }
    //extention method 
    public static void TriggerClientEvent(this string key, object[] datas)
    {
        if (dicClientEvent.ContainsKey(key))
            dicClientEvent[key].Invoke(datas);
    }
    #endregion

    
}