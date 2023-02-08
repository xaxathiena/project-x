using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BestHTTP;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using System.Net.NetworkInformation;
using UnityEngine.Networking;

namespace ClientUnity
{
    public class HTTPControl
    {

        public static Action UserCanCommunicatedServerEvent;

        private static HTTPControl instance;
        private static bool isCheckServerAlive = true;
        private static ServerDefineLocal serverDefine;
        public static ServerDefineLocal ServerDefine => serverDefine;
        //https://asia-southeast1-p02-89e31.cloudfunctions.net/api
#if UNITY_EDITOR
        private static string HostName = "localhost:5001/p02-server-test/asia-southeast1";
#elif !SERVERTEST
        private static string HostName = "asia-southeast1-p02-89e31.cloudfunctions.net";
#else
        private static string HostName = "asia-southeast1-p02-server-test.cloudfunctions.net";
#endif
        private static string codeSign = "W753EE6sFTYD6sjjCMmrhaE5nWaHqBjSs7PNCLCg";


        public static HTTPControl Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HTTPControl();
                    if(serverDefine == null)
                    {
                        serverDefine = Resources.Load<ServerDefineLocal>("ServerDefineLocal");
                    }
                    ServerTypeEnum serverType = ServerTypeEnum.Local;
#if UNITY_EDITOR
                    if(serverDefine.ServerType != ServerTypeEnum.Local)
                    {
                        serverType = ServerTypeEnum.ServerTest;
                    }
#elif DEVELOPMENT_BUILD
                    serverType = ServerTypeEnum.ServerTest;
#else
                    serverType = ServerTypeEnum.ServerReal;
#endif
                    switch (serverType)
                    {
                        case ServerTypeEnum.Local:
                            instance.m_ServerHost = "http://" + serverDefine.LocalServer + "/api/";
                            HostName = serverDefine.LocalServer;
                            break;
                        case ServerTypeEnum.ServerReal:
                            instance.m_ServerHost = "https://" + serverDefine.Servers[0].serverUrl + "/api/";
                            HostName = serverDefine.Servers[0].serverUrl;
                            break;
                        case ServerTypeEnum.ServerTest:
                            instance.m_ServerHost = "https://" + serverDefine.TestingServer + "/api/";
                            HostName = serverDefine.TestingServer;
                            break;
                        case ServerTypeEnum.Offline:
                            
                            break;
                        default:
                            instance.m_ServerHost = "https://" + serverDefine.DefaultServer + "/api/";
                            HostName = serverDefine.DefaultServer;
                            break;
                    }
                }
                return instance;
            }
        }

        const string tokenGCP = "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFjYjZiZTUxZWZlYTZhNDE5ZWM5MzI1ZmVhYTFlYzQ2NjBmNWIzN2MiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJhY2NvdW50cy5nb29nbGUuY29tIiwiYXpwIjoiNjE4MTA0NzA4MDU0LTlyOXMxYzRhbGczNmVybGl1Y2hvOXQ1Mm4zMm42ZGdxLmFwcHMuZ29vZ2xldXNlcmNvbnRlbnQuY29tIiwiYXVkIjoiNjE4MTA0NzA4MDU0LTlyOXMxYzRhbGczNmVybGl1Y2hvOXQ1Mm4zMm42ZGdxLmFwcHMuZ29vZ2xldXNlcmNvbnRlbnQuY29tIiwic3ViIjoiMTExMTQ1MjQ5Nzc2NDQ1MzY1NjE3IiwiZW1haWwiOiJodW9uZ3RoaWVuYmtAZ21haWwuY29tIiwiZW1haWxfdmVyaWZpZWQiOnRydWUsImF0X2hhc2giOiJIdDhHTE1yeGhLT082UEVSdGN3eGpBIiwiaWF0IjoxNjQ1NDI4MTEwLCJleHAiOjE2NDU0MzE3MTAsImp0aSI6Ijg4NWFlNjM3YTk4NmQxYWM2NjFmYjM4ZGUwZGMxYTgxYWZmNjg0OTgifQ.Mc4DvQlZDYEMN0VJLbI608W4OYGzi00lf-QQSt4QEkRXvQ5CgITCGY9Rt6_y4Xt9yVrBkbjPdwdw5Y2a4Dil8E4UBpeW7CG76Bpr30GExZeNWwYgcpMl1QpaNcpF8-4Hl0Dh-AwJ6AcUHT9HBWXvEKzQhDKvvU5EpAW0bmC4eCuMP-sEaCEdtEX0U2oJy3ayQG05al9v497lr3_DblUTNdDB9QhSRQ7QS2s4-vXqFrphYhe414dGqcUfGxvCngaAMI2-JhLYrcwzwhhRPzGWRm8R05DAROSsTuVO56uaM4SLYy5KpSSucuqt6uisdlO4uSLXkRscw4ZjAV-kzjRssA";
        public static Action OnDoNotFoundAccessTokenEvent;
        public const string ACCESS_TOKEN = "accesstoken";
        public const string TIME_UPDATE = "TIME_UPDATE";
        private static IEnumerator GetRequest(string uri, Action<bool> action)
        {
            Debug.LogWarning("GetRequest");
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;
                Debug.LogWarning("webRequest.result = " + webRequest.result);
                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                    case UnityWebRequest.Result.ProtocolError:
                        isConnectedInternet = false;
                        action(false);
                        break;
                    case UnityWebRequest.Result.Success:
                        isConnectedInternet = true;
                        action(true);
                        break;
                    default:
                        isConnectedInternet = false;
                        action(false);
                        break;
                }
            }
        }
        public IEnumerator CheckInternetConnection(Action<bool> action)
        {
            if(serverDefine.ServerType == ServerTypeEnum.Offline)
            {
                yield return null;
                action?.Invoke(false);
            }
            else
            {
                string httpString = "http://";
                switch (serverDefine.ServerType)
                {
                    case ServerTypeEnum.Local:
                        action(true);
                        yield break;
                    case ServerTypeEnum.Offline:
                        action?.Invoke(false);
                        yield break;
                    case ServerTypeEnum.ServerReal:
                    case ServerTypeEnum.ServerTest:
                    default:
                        httpString = "https://" + HostName;
                        break;
                }
                yield return GetRequest(httpString, action);
            }
        }
        private static bool isConnectedInternet = false;
        private static bool IsConnectedInternet => isConnectedInternet;
        public bool IsInternetConnected
        {
            get
            {
                try
                {

                    if (serverDefine.ServerType == ServerTypeEnum.Offline)
                    {
                        return false;
                    }

                    if (Application.internetReachability == NetworkReachability.NotReachable && !isConnectedInternet)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("Internet fail + " + ex.Message);
                    return false;
                }

            }
        }

        [SerializeField]
        private string m_ServerHost = "http://localhost:5001/p02-server-test/asia-southeast1/api/";
        public bool IsSameHost(string value) => m_ServerHost == value;

        public void SetupServerHost(string url)
        {
            m_ServerHost = url;
        }
        private void Start()
        {
            OnDoNotFoundAccessTokenEvent += OnDoNotFoundAccessTokenHandle;
            
        }
        private float maxTimeRequest = 63f;

        private bool _isRequestWaiting = false;
        private bool isRequestWaiting
        {
            get
            {
                return _isRequestWaiting;
            }
            set{
                _isRequestWaiting = value;
            }
        }
        private float currentTimeRequest = 0;


        public bool IsRequestWaiting => isRequestWaiting;
        public void Update()
        {
            if (isRequestWaiting)
            {
                currentTimeRequest += Time.deltaTime;
                if (currentTimeRequest > maxTimeRequest)
                {
                    currentTimeRequest = 0;
                    isRequestWaiting = false;
                }
            }
        }
        private void OnDoNotFoundAccessTokenHandle()
        {
            Debug.LogError("Do not found access token!");
        }
        /// <summary>
        /// Send mess to server with post method
        /// </summary>
        /// <param name="path">Path to server understand what post want to do</param>
        /// <param name="rawData">Data in body post</param>
        /// <param name="headerData">Data in header post</param>
        /// <param name="requestFinishedcallback">Event return data from server</param>
        public void Post(string path, object rawData, Dictionary<string, string> headerData, Action<ServerHTTPDataReponse> requestFinishedcallback, bool isForcePost = false)
        {
            if (isRequestWaiting && !isForcePost)
            {
                // Debug.LogError("waiting_other_request");
                //
                // PopupController.instance.ShowPopup(PopupIndex.CommonPopup, null, new CommonPopupParam("warning", "waiting_other_request", new ButtonData("confirm", ButtonType.Tier1, ()=> {
                //    
                // }),()=> {
                //     
                // }));
                // PopupController.instance.HidePopup(PopupIndex.LoadingPopup);
                requestFinishedcallback?.Invoke(new ServerHTTPDataReponse()
                {
                    error = "Other request is running, please waiting!"
                });
                return;
            }
            isRequestWaiting = true;
            if (HTTPControl.instance.IsInternetConnected)
            {
                //Push event after get internet
                UserCanCommunicatedServerEvent?.Invoke();
#if UNITY_EDITOR
                Debug.LogError("path HTTP: " + (m_ServerHost + path));
#endif
                var request = new HTTPRequest(new Uri(m_ServerHost + path), HTTPMethods.Post, (_request, _response) =>
                {
                    isRequestWaiting = false;
                    if (_response == null)
                    {
                        requestFinishedcallback?.Invoke(new ServerHTTPDataReponse
                        {
                            error = "Some thing wrong, please try again!",
                        });
                    }
                    else if (StatusCodeHttpHandle(_response))
                    {
                        //Debug.LogError(_response.DataAsText.GetValueByKey("timeUpdate"));
                        //Debug.Log("Data return: " + _response.DataAsText);
                        requestFinishedcallback?.Invoke(new ServerHTTPDataReponse
                        {
                            value = _response.DataAsText,
                            code = _response.StatusCode
                        });
                    }
                    else
                    {
                        Debug.LogError("DataAsText = " + _response.DataAsText);
                        requestFinishedcallback?.Invoke(new ServerHTTPDataReponse
                        {
                            error = _response.DataAsText,
                            code = _response.StatusCode
                        });
                    }
                });

                request.SetHeader("Content-Type", "application/json; charset=UTF-8");
                request.SetHeader("tsa", codeSign);
                //request.SetHeader("bearer", tokenGCP);
                if (headerData != null)
                {
                    foreach (var data in headerData)
                    {
                        Debug.Log(data.Key + " - " + data.Value);
                        request.SetHeader(data.Key, data.Value);
                    }
                }
                if (rawData != null)
                {
                    request.RawData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(rawData));
                    PlayerPrefs.SetString("DataSend", JsonConvert.SerializeObject(rawData));
                }
                request.Send();
                Debug.LogWarning("Send");
            }
            else
            {
                isRequestWaiting = false;
                // PopupController.instance.ShowPopup(PopupIndex.CommonPopup, null, new CommonPopupParam("warning", "need_internet", new ButtonData("confirm", ButtonType.Tier1, () => {
                //    
                // }), () => {
                //     
                // }));
                // PopupController.instance.HidePopup(PopupIndex.LoadingPopup);
            }
        }
        private bool StatusCodeHttpHandle(HTTPResponse response)
        {
            var statusCode = response.StatusCode;
            if (statusCode == 403)
            {
                Debug.LogError("ACCESS_TOKEN = Empty");
                //PlayerPrefs.SetString(ACCESS_TOKEN, string.Empty);
                Debug.Log("Token not found!");
                return false;
            }
            else if (statusCode == 400)
            {
                Debug.Log("something wrong!");
                return false;
            }
            else
            {
                return true;
            }
        }
        private static bool PingHost(string nameOrAddress)
        {
            bool pingable = false;
            System.Net.NetworkInformation.Ping pinger = null;

            try
            {
                pinger = new System.Net.NetworkInformation.Ping();
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

            return pingable;
        }
    }
    public class ServerHTTPDataReponse
    {
        public string error;
        public string value;
        public int code;
    }
    public class DataRecieveSignin
    {
        public string accesstoken;
        public string secretpass;
        // public object userInfo;
    }
    public class AuthenData
    {
        public string username;
        public string pass;
    }

    [Serializable]
    public class DataUser
    {
        public string _id = "jtejasdfoi";
        public string userId = "fasasdfasdf";
        public string dateCreated = "ddfasfasfdasdf";
        public string personalname = "asdadfadfasdf";
        [SerializeField]
        public Dictionary<string, ItemInventory> playerInventory = new Dictionary<string, ItemInventory>();
        [SerializeField]
        public Dictionary<string, WeaponData> weapons = new Dictionary<string, WeaponData>();
        public override string ToString()
        {
            return "(" + userId + ") (" + personalname + ")";
        }
    }
    [Serializable]
    public class WeaponData
    {
        public string _id = "sdiidfsdf";
        public int id;
        public int level;
    }
    [Serializable]
    public class ItemInventory
    {
        public int _value;
    }

    public enum HTTPCode
    {
        Continue = 101,
        OK = 200,
        Create = 201,
        Accepted = 202,
        Non_Authen = 203,
        No_Content = 204,
        ALREADY_REPORTED = 208,
        YES = 210,
        NO = 211,
        MUILTIPLE_CHOICES = 300,
        Found = 302,
        SEE_OTHER = 303,
        NOT_MODIFIED = 304,
        //error,
        Bad_Request = 400,
        Unauthorized = 401,
        Payment_Required = 402,
        Forbidden = 403,
        Not_Found = 404,
        Method_Not_Allowed = 405,
        Not_Acceptable = 406,
        Request_Timeout = 408,
        Conflict = 409,
        Locked = 423,
        Failed_Dependency = 424,
        Upgrade_Required = 426,
        Precondition_Required = 428,
        Too_Many_Requests = 429,
        //Server error
        Error = 500,
        Not_Implemented = 501,
        Unavailable = 503,
        Function_Not_Work = 512

    }
}