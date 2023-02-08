using System;
using System.Collections;
using System.Collections.Generic;
using ClientUnity;
using UnityEngine;

public class ServerManager : Singleton<ServerManager>
{
    public void GetdataPlayer(object dataPush, Action< string > successCallback, Action<string > failCallback, bool isForce = false)
    {
        HTTPControl.Instance.Post(ServerPath.GET_USER_DATA, dataPush,null, (dataReturn) =>
        {
            if (!string.IsNullOrEmpty(dataReturn.error))
            {
                Debug.Log("Register new uer failed");
                failCallback?.Invoke(dataReturn.error);
            }
            else
            {
                Debug.Log("Register new uer successed");
                successCallback?.Invoke(dataReturn.value);
            }
        }, isForce);
    }

    public void UpdatePlayerAvatarAndName(object dataPush, Action< string > successCallback, Action<string > failCallback)
    {
        HTTPControl.Instance.Post(ServerPath.UPDATE_AVATAR_AND_NAME,dataPush,null, (dataReturn) =>
        {
            if (!string.IsNullOrEmpty(dataReturn.error))
            {
                Debug.Log("Register new uer failed");
                failCallback?.Invoke(dataReturn.error);
            }
            else
            {
                Debug.Log("Register new uer successed");
                successCallback?.Invoke(dataReturn.value);
            }
        });
    }
}


[Serializable]
public class DataUserRank
{
    public List<DataPlayerServer> higher;
    public List<DataPlayerServer> lower;

}

[Serializable]
public class DataPlayerServer
{
    public int chip;
    //public DateTime joinedAt;
    public string name;
    public bool isBot;
    public string userID;
    public int rank;

}