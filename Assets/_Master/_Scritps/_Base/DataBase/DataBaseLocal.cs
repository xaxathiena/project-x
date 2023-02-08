using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using Newtonsoft.Json;
using UnityEngine.Events;



public class DataBaseLocal : MonoBehaviour
{
    // playerInfo/username
    private PlayerData dataPlayer;
    public bool LoadData()
    {
        //PlayerPrefs.DeleteAll();
        //return;
        return false;
        // if (ObscuredSaver.GetString(SAVER_TYPE.DATA_PLAYER) != "")
        // {
        //     GetData();
        //     return true;
        // }
        // else
        // {
        //     return false;
        //   
        // }
    }
    public void CreateNewData (PlayerData dataPlayer,Action callback)
    {
        this.dataPlayer = dataPlayer;
        SaveData();
        callback?.Invoke();
    }
    public T Read<T>(string path)
    {
        object data=null;

        string[] s = path.Split('/');
        List<string> paths = new List<string>();
        paths.AddRange(s);

        ReadDataBypath(paths, dataPlayer, out data);

        return (T)data;
    }
    public T Read<T>(string path, object key)
    {
        object data = null;

        string[] s = path.Split('/');
        List<string> paths = new List<string>();
        paths.AddRange(s);
       
        ReadDataBypath(paths, dataPlayer, out data);
        Dictionary<string, T> newDic = (Dictionary<string, T>)data;

        return newDic[key.ToKey()];
    }
    private void ReadDataBypath(List<string> paths,object data, out object dataOut)
    {
        string p = paths[0];

        Type t = data.GetType();

        FieldInfo field = t.GetField(p);
        if(paths.Count==1)
        {
            dataOut = field.GetValue(data);
        }
        else
        {
            paths.RemoveAt(0);
            ReadDataBypath(paths, field.GetValue(data), out dataOut);
        }

    }
    public void UpdateData(string path,object dataNew,Action callback)
    {

        string[] s = path.Split('/');
        List<string> paths = new List<string>();
        paths.AddRange(s);
        UpdateDataBypath(paths, dataPlayer, dataNew, callback);
        SaveData();

        dataNew.TriggerEventData(path);
    }
    private void UpdateDataBypath(List<string> paths, object data, object datanew, Action callback)
    {
        string p = paths[0];

        Type t = data.GetType();

        FieldInfo field = t.GetField(p);
        if (paths.Count == 1)
        {
            field.SetValue(data, datanew);
            if(callback!=null)
            {
                callback();
            }
           
        }
        else
        {
            paths.RemoveAt(0);
            UpdateDataBypath(paths, field.GetValue(data), datanew, callback);
        }

    }
    public void UpdateData<TValue>(string path, object key, TValue dataNew, Action callback)
    {

        string[] s = path.Split('/');
        List<string> paths = new List<string>();
        paths.AddRange(s);
        UpdateDataDicBypath(paths, dataPlayer, key,dataNew, callback);
        SaveData();
        dataNew.TriggerEventData(path);
    }
    private void UpdateDataDicBypath<TValue>(List<string> paths, object data, object key, TValue dataNew, Action callback)
    {
        string p = paths[0];

        Type t = data.GetType();

        FieldInfo field = t.GetField(p);


        if (paths.Count == 1)
        {
 
            object dic = field.GetValue(data);

            Dictionary<string, TValue> newDic = (Dictionary<string, TValue>)dic;
            newDic[key.ToKey()] = dataNew;
            field.SetValue(data, newDic);
            if (callback != null)
            {
                callback();
            }
           
        }
        else
        {
            paths.RemoveAt(0);
            UpdateDataDicBypath(paths, field.GetValue(data),key, dataNew, callback);
        }

    }
    // public bool TryRead<T>(string path,out T dataOut,  bool isGetClone = true)
    // {
    //     object data = default;
    //
    //     string[] s = path.Split('/');
    //     List<string> paths = new List<string>(s);
    //     //paths.AddRange(s);
    //
    //     if (ReadDataBypath(paths, this.data, out data))
    //     {
    //         if (isGetClone)
    //         {
    //             dataOut= data.DeepClone<T>();
    //
    //         }
    //         dataOut = (T)data;
    //         return true;
    //     }
    //     dataOut =  default;
    //     return false;
    // }
    public void Delete()
    {

    }
    private void SaveData()
    {
        string s = JsonConvert.SerializeObject(dataPlayer, Formatting.None);
        // ObscuredSaver.Save(SAVER_TYPE.DATA_PLAYER, s);
    }
    private void GetData()
    {
        // string s = ObscuredSaver.GetString(SAVER_TYPE.DATA_PLAYER);
        // dataPlayer = JsonConvert.DeserializeObject<PlayerData>(s);
    }
}
