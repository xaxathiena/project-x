﻿using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;
[System.Serializable]
public class  UnitDefaultSpawnState : IState
{
    [HideInInspector] public UnitDefaultControl parent;

    private float currentTime;
    
    public bool canTransitionToSelf { get; }
    public void Initialize(FSMSystem parent, params object[] datas)
    {
    }

    public void OnEnter(params object[] data)
    {
        Debug.Log("enter spawn");
    }

    public void OnEnterFromSameState(params object[] data)
    {
    }

    public void OnUpdate()
    {
        currentTime += Time.deltaTime;
        if (currentTime > 3f)
        {
            parent.GotoIdle();
        }
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
