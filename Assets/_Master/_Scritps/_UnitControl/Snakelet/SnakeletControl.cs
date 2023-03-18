using System.Collections.Generic;
using StateMachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(NavMeshObstacle))]
[RequireComponent(typeof(CharacterController))]

public class SnakeletControl : UnitControlBase
{
    [Header("Setup state")]
    public SnakeletSpawnState spawState;
    public SnakeletIdleState idleState;
    public SnakeletMoveState moveState;
    public SnakeletAttackState attackState;
    public SnakeletKnockState knockState;
    public SnakeletDeadState deadState;
    [Space(10)] 
    [SerializeField] private Animator amin;
    [SerializeField] private bool isCharacterFirst;
    public NavMeshAgent agent;
    public NavMeshObstacle obsTackle;
    public SnakeletDataBinding dataBinding;
    public CharacterController controller;
    
    [Header("Setup parameter")]
    public Transform fireStartPivot;
    public string bulletName;
    public float bulletSpeed;
    public float deplayToFire;
    public HealBarController healBarController;
    private IUnit currentTarget;
    private List<IUnit> tempResult = new List<IUnit>();
    private float timeToFindNewTarget = 0.5f;
    private float currentTimeToFindNewTarget = 0f;
    private Vector3 dir = Vector3.right;
    
    public float attackRange = 3;
    public float detectRange = 10;
    public float rof = 0.3f;
    public List<Transform> positionsPath;
    
    public IUnit CurrentTarget => currentTarget;
    public Renderer[] mat;

    public Collider collider;
    private void Start()
    {
        IsReceiveDirective = true;
        dir = transform.forward;
        dataBinding = new SnakeletDataBinding();
        dataBinding.Init(amin);
        
        spawState = new SnakeletSpawnState();
        spawState.parent = this;
        RegisterState(spawState);
        
        idleState = new SnakeletIdleState();
        idleState.parent = this;
        RegisterState(idleState);
        
        moveState = new SnakeletMoveState();
        moveState.parent = this;
        RegisterState(moveState);
        
        attackState = new SnakeletAttackState();
        attackState.parent = this;
        RegisterState(attackState);
        
        deadState = new SnakeletDeadState();
        deadState.parent = this;
        RegisterState(deadState);
        
        knockState = new SnakeletKnockState();
        knockState.parent = this;
        RegisterState(knockState);
        
        
        SetEntryState(spawState);
        SystemStart();
        healBarController.InitHealBar();
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
    public void GotoKnock(params object[] data)
    {
        currentStateStr = "GotoKnock";
        GotoState(knockState, data);
    }
    public void GotoDead(params object[] data)
    {
        currentStateStr = "GotoDead";
        collider.enabled = false;
        controller.enabled = false;
        UnitsManager.instance.RemoveUnit(this);
        GotoState(deadState, data);
    }
    
    private float currentHeal = 100;
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
        if (isCharacterFirst)
        {
            FindNewCharacter();
        }
        else
        {
            FindNewTarget();
            
        }
        dataBinding.OnFixedUpdate();
    }
    private void FindNewCharacter()
    {
        if (currentTimeToFindNewTarget > timeToFindNewTarget)
        {
            currentTimeToFindNewTarget = 0f;
            tempResult.Clear();
            UnitsManager.instance.GetUnitInRange(ref tempResult, transform.position, float.MaxValue,
                unitSide);
            if (tempResult.Count > 0)
            {
                tempResult.Sort(ComparePosition);
                currentTarget = tempResult[0];
            }
            else
            {
                GotoIdle();
            }
        }
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

    
    public override UnitSide unitSide { get => UnitSide.Enemy; }
    public override float boderRange { get => 1f; }
    public override void UnitSpawn()
    {
        UnitsManager.instance.AddUnit(this);
    }
    public override void UnitDestroy()
    {
        UnitsManager.instance.RemoveUnit(this);
    }

    public override Vector3 Dir { get => dir; set => dir = value; }
    public bool IsShowHealBar
    {
        set => healBarController.gameObject.SetActive(value);
    }

    public override void ApplyDamage(AttackData data)
    {
        currentHeal -= data.damage;
        healBarController.SetupHealth(currentHeal, 0, 100);
        if (currentHeal <= 0)
        {
            GotoDead();
        }
        else
        {
            GotoKnock();
        }
    }


    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        if (Application.isPlaying)
        {
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
}