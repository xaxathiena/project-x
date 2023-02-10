using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;
[System.Serializable]
public class  UnitDefaultIdleState : IState
{
    [HideInInspector] public UnitDefaultControl parent;

    private float currentTime;
    
    public bool canTransitionToSelf { get; }
    public void Initialize(FSMSystem parent, params object[] datas)
    {
        currentTime = 0f;
    }

    public void OnEnter(params object[] data)
    {
        currentTime = 0f;
        parent.dataBinding.Speed = 0;
        parent.agent.enabled = false;
        parent.obsTackle.enabled = true;
    }

    public void OnEnterFromSameState(params object[] data)
    {
    }

    public void OnUpdate()
    {
        parent.GotoMove();
    }

    public void OnLateUpdate()
    {
    }

    public void OnFixedUpdate()
    {
    }

    public void OnExit()
    {
        Debug.Log("Exit spawn");
    }

    public void Dispose()
    {
    }
}