using System;
using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class UnitDefaultControl : FSMSystem
{
    [Header("Setup state")]
    public UnitDefaultSpawnState spawState;
    public UnitDefaultIdleState idleState;
    public UnitDefaultMoveState moveState;
    public UnitDefaultAttackState attackState;
    public UnitDefaultDeadState deadState;
    [Space(10)] 
    [SerializeField] private Animator amin;
    public NavMeshAgent agent;
    public NavMeshObstacle obsTackle;
    public UnitDefaultDataBinding dataBinding;
    public CharacterController controller;
    public float attackRange = 3;
    public float detectRange = 10;
    public float rof = 0.3f;
    private void Start()
    {
        dataBinding = new UnitDefaultDataBinding();
        dataBinding.Init(amin);
        
        spawState = new UnitDefaultSpawnState();
        spawState.parent = this;
        RegisterState(spawState);
        
        idleState = new UnitDefaultIdleState();
        idleState.parent = this;
        RegisterState(idleState);
        
        moveState = new UnitDefaultMoveState();
        moveState.parent = this;
        RegisterState(moveState);
        
        attackState = new UnitDefaultAttackState();
        attackState.parent = this;
        RegisterState(attackState);
        
        deadState = new UnitDefaultDeadState();
        deadState.parent = this;
        RegisterState(deadState);
        
        SetEntryState(spawState);
        SystemStart();
    }


    public void GotoIdle(params object[] data)
    {
        GotoState(idleState, data);
    }
    public void GotoMove(params object[] data)
    {
        GotoState(moveState, data);
    }
    public void GotoDead(params object[] data)
    {
        GotoState(deadState, data);
    }
    public void GotoAttack(params object[] data)
    {
        GotoState(attackState, data);
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
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(moveState.targetPos, attackRange);
    }
}
