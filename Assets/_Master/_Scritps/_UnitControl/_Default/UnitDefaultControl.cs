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
    public UnitDefaultIdleState idle;
    public UnitDefaultMoveState move;
    public UnitDefaultAttackState attack;
    public UnitDefaultDeadState dead;
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
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, move.nextPos);
    }
}
