using System;
using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class UnitDefaultControl : FSMSystem, IUnit
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
    
    [Header("Setup parameter")]
    public float attackRange = 3;
    public float detectRange = 10;
    public float rof = 0.3f;
    public List<Transform> positionsPath;
    
    
    private IUnit currentTarget;
    private List<IUnit> tempResult = new List<IUnit>();
    private float timeToFindNewTarget = 0.5f;
    private float currentTimeToFindNewTarget = 0f;
    private Vector3 dir = Vector3.right;
    public IUnit CurrentTarget => currentTarget;
    
    private void Start()
    {
        IsReceiveDirective = true;
        dir = transform.forward;
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
        currentStateStr = "GotoIdle";
        GotoState(idleState, data);
    }
    public void GotoMove(params object[] data)
    {
        currentStateStr = "GotoMove";
        GotoState(moveState, data);
    }
    public void GotoDead(params object[] data)
    {
        currentStateStr = "GotoDead";
        GotoState(deadState, data);
    }
    public void GotoAttack(params object[] data)
    {
        currentStateStr = "GotoAttack";
        GotoState(attackState, data);
    }
    public void GotoSpawn(params object[] data)
    {
        currentStateStr = "idle";
        GotoState(spawState, data);
    }
    public override void SystemUpdate()
    {
        dataBinding.OnUpdate();
        currentTimeToFindNewTarget += Time.deltaTime;
    }

    public override void SystemFixedUpdate()
    {
        FindNewTarget();
        dataBinding.OnFixedUpdate();
    }
    
    private void FindNewTarget()
    {
        if (currentTimeToFindNewTarget > timeToFindNewTarget)
        {
            currentTimeToFindNewTarget = 0f;
            tempResult.Clear();
            UnitsManager.instance.GetUnitInRange(ref tempResult, transform.position, detectRange,
                unitSide);
            if (tempResult.Count == 0)
            {
                if (moveState.isBreakNormalBehauviour)
                {
                    currentTarget = UnitsManager.instance.GetNearestTower(position);
                }
                else
                {
                    UnitsManager.instance.GetTowerInRange(ref tempResult, transform.position, detectRange,
                        unitSide);
                    
                }
            }

            if (tempResult.Count > 0)
            {
                tempResult.Sort(ComparePosition);
                currentTarget = tempResult[0];
            }
        }
    }

    public UnitSide unitSide { get => UnitSide.Enemy; }
    public float boderRange { get => 1f; }
    public Vector3 position { get => transform.position; }
    public Quaternion rotation { get => transform.rotation; }
    public void UnitSpawn()
    {
        UnitsManager.instance.AddUnit(this);
    }
    public void UnitDestroy()
    {
        UnitsManager.instance.RemoveUnit(this);
    }

    public Vector3 Dir { get => dir; set => dir = value; }
    public bool IsReceiveDirective { get; set; }
    public void ApplyDamage(AttackData data)
    {
        throw new NotImplementedException();
    }
    
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        if (moveState.path.corners.Length > 1)
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < moveState.path.corners.Length - 1; i++)
            {
                Gizmos.DrawLine(moveState.path.corners[i], moveState.path.corners[i +1]);
            }
        }
    }
}
