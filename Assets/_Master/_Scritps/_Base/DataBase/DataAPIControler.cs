using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataAPIControler : Singleton<DataAPIControler>
{
    [SerializeField]
    private DataBaseLocal model;
    // Start is called before the first frame update

    public void OnInit(Action callback)
    {   
        if(model.LoadData())
        {
            callback?.Invoke();
        }
      else
        {
            // create new data
            PlayerData playerData = new PlayerData();   
        }
    }

    public void OnInit(string dataRaw, Action callback)
    {
        PlayerData playerData = new PlayerData()
        {
            playerName = dataRaw.GetValueByKey("name"),
            inventory = new PlayerInventory(dataRaw.GetValueByKey("chip")),
            avatarName = dataRaw.GetValueByKey("avatar")
        };
        model.CreateNewData(playerData, callback);
    }

    #region Base functions
    public void SetData<T>(T data,string dataPath)
    {
        model.UpdateData(dataPath, data, null);
    }
    public void SetData<T>(T data, string dataPath,object key)
    {
        model.UpdateData(dataPath, key, data, null);
    }
    public T GetData<T>(string dataPath) => model.Read<T>(dataPath);
    public T GetData<T>(string dataPath, object key) => model.Read<T>(dataPath, key);
    public T GetData<T>(string dataPath, object key, T defaultValue)
    {
        var data =model.Read<T>(dataPath, key);
        if (data == null)
            return defaultValue;
        return data;
    }

    public void RemoveDic<T>(string dataPath, object key)
    {
        var dic = GetData<Dictionary<string, T>>(dataPath);
        dic.Remove(key.ToKey());
        SetData(dic, dataPath);
    }
    public void AddDic<T>(T value, string dataPath, object key)
    {
        var dic = GetData<Dictionary<string, T>>(dataPath);
        string keyString = key.ToKey();
        if (dic.ContainsKey(keyString))
        {
            dic[keyString] = value;
        }
        else
        {
            dic.Add(keyString, value);
        }
        SetData(dic, dataPath);
    }
    #endregion
}

