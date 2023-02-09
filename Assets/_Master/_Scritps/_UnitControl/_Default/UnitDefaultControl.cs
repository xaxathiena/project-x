using System;
using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;
using UnityEngine.Serialization;

public class UnitDefaultControl : FSMSystem
{
    [Header("Setup state")]
    [SerializeField] protected UnitDefaultSpawnState spawState;
    [SerializeField] protected UnitDefaultIdleState idle;
    [SerializeField] protected UnitDefaultMoveState move;
    [SerializeField] protected UnitDefaultAttackState attack;
    [SerializeField] protected UnitDefaultDeadState dead;

    [Space(10)] 
    [SerializeField] private Animator amin;
    public UnitDefaultDataBinding dataBinding;

    
    
    private void Start()
    {
        dataBinding = new UnitDefaultDataBinding();
        dataBinding.Init(amin);
        
        spawState = new UnitDefaultSpawnState();
        spawState.parent = this;
        RegisterState(spawState);
        
        idle = new UnitDefaultIdleState();
        idle.parent = this;
        RegisterState(idle);
        
        move = new UnitDefaultMoveState();
        move.parent = this;
        RegisterState(move);
        
        attack = new UnitDefaultAttackState();
        attack.parent = this;
        RegisterState(attack);
        
        dead = new UnitDefaultDeadState();
        dead.parent = this;
        
        RegisterState(dead);
        SetEntryState(spawState);
        SystemStart();
    }


    public void GotoIdle(params object[] data)
    {
        GotoState(idle, data);
    }
    public void GotoMove(params object[] data)
    {
        GotoState(move, data);
    }
    public void GotoDead(params object[] data)
    {
        GotoState(dead, data);
    }
    public void GotoAttack(params object[] data)
    {
        GotoState(attack, data);
    }
    public void GotoSpawn(params object[] data)
    {
        GotoState(spawState, data);
    }
    public override void SystemUpdate()
    {
        dataBinding.OnUpdate();
    }

    public override void SystemFixedUpdate()
    {
        dataBinding.OnFixedUpdate();
    }
}
