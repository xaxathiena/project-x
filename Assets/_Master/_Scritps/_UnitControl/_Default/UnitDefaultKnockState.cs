using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

public class UnitDefaultKnockState : IState
{
    [HideInInspector] public UnitDefaultControl parent;
    public bool canTransitionToSelf { get; }
    private float maxtimeKnock = 1f;
    private float currentTimeKnock;
    public void Initialize(FSMSystem parent, params object[] datas)
    {
    }

    public void OnEnter(params object[] data)
    {
        currentTimeKnock = 0f;
        parent.dataBinding.KnockBack = true;
    }

    public void OnEnterFromSameState(params object[] data)
    {
    }

    public void OnUpdate()
    {
        currentTimeKnock += Time.deltaTime;
        if (currentTimeKnock > maxtimeKnock)
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
    }

    public void Dispose()
    {
    }

    public void OnDrawGizmos()
    {
    }
}