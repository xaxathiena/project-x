using UnityEngine;
using System;
public class UnityLifeCircle : Singleton<UnityLifeCircle>
{
    private Action OnAwakeEvent;
    private Action OnStartEvent;
    private Action OnUpdateEvent;
    private Action OnFixUpdateEvent;

    #region Register event
    public void RegisterUnityEvent(UnityEventType type, Action action)
    {
        switch (type)
        {
            case UnityEventType.Awake:
                OnAwakeEvent += action;
                break;
            case UnityEventType.Start:
                OnStartEvent += action;
                break;
            case UnityEventType.Update:
                OnUpdateEvent += action;
                break;
            case UnityEventType.FixUpdate:
                OnFixUpdateEvent += action;
                break;
        }
    }
    public void UnRegisterUnityEvent(UnityEventType type, Action action)
    {
        switch (type)
        {
            case UnityEventType.Awake:
                OnAwakeEvent -= action;
                break;
            case UnityEventType.Start:
                OnStartEvent -= action;
                break;
            case UnityEventType.Update:
                OnUpdateEvent -= action;
                break;
            case UnityEventType.FixUpdate:
                OnFixUpdateEvent -= action;
                break;
        }
    }
    #endregion

    #region Unity life cirle
    protected override void OnAwake()
    {
        OnAwakeEvent?.Invoke();
    }

    private void Start()
    {
        OnStartEvent?.Invoke();
    }

    private void Update()
    {
        OnUpdateEvent?.Invoke();
    }

    private void FixedUpdate()
    {
        OnFixUpdateEvent?.Invoke();
    }
    #endregion
    
}

public enum UnityEventType
{
    Awake,
    Start,
    Update,
    FixUpdate,
}
