using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
[CreateAssetMenu(menuName = "Resource/ServerDefineLocal")]
public class ServerDefineLocal : ScriptableObject
{

    [SerializeField] private List<ServerDefineByRegoin> servers;
    [SerializeField] private string defaultServer;
    [SerializeField] private string testingServer;
    [SerializeField] private string localServer;
    [SerializeField] private ServerTypeEnum serverType;

    public List<ServerDefineByRegoin> Servers => servers;
    public string DefaultServer=> defaultServer;
    public string TestingServer => testingServer;
    public string LocalServer=> localServer;
    public ServerTypeEnum ServerType => serverType;
    
    [System.Serializable]
    public class ServerDefineByRegoin
    {
        public string serverUrl;
        [Tooltip("Split by ',")]
        public string regions;
    }
}
public enum ServerTypeEnum
{
    Local,
    ServerTest,
    ServerReal,
    Offline
}