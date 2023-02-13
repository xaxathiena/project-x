using System.Collections.Generic;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Tsp;
using StateMachine;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class UnitDefaultMoveState: IState
{
    [HideInInspector] public UnitDefaultControl parent;

    [SerializeField]
    private bool move;
    private NavMeshAgent Agent => parent.agent;
    public NavMeshPath path;

    private float speedMove;
    private IUnit targetUnit;

    public bool isBreakNormalBehauviour = false;
    // private List<Transform> points = new List<Transform>();
    // private int currentPoint = 0;
    // public Vector3 nextPos = Vector3.zero;
    public bool canTransitionToSelf { get; }
    // public Vector3 targetPos = Vector3.zero;
    public void Initialize(FSMSystem parent, params object[] datas)
    {
        path = new NavMeshPath();
        // Agent = this.parent.agent;
        speedMove = 3f;
        this.parent.dataBinding.Speed = speedMove;
        move = false;
        // points = (List<Transform>)datas[0];
    }

    public void OnEnter(params object[] data)
    {
        //Set agen
        
        targetUnit = InGameManager.instance.MotherTreePosition;
        move = true;
        parent.agent.enabled = true;
        Debug.Log("Obstackle enableed = " + false);
        parent.obsTackle.enabled = false;
    }
    
    public void OnEnterFromSameState(params object[] data)
    {
    }

    private float delta = 0.01f;

    public void OnUpdate()
    {
        if (move)
        {

            if (parent.CurrentTarget != null)
            {
                var isInside = parent.position.IsPositionInRange(parent.CurrentTarget.position, parent.attackRange,
                    parent.CurrentTarget.boderRange);
                var dir = CalNavmeshDir();
                if (!isInside)
                {
                    MoveByDir(dir);
                }
                else
                {
                    move = false;
                    Agent.isStopped = true;
                    this.parent.dataBinding.Speed = 0f;
                    var q = Quaternion.LookRotation(dir, Vector3.up);
                    this.parent.transform.localRotation =
                        Quaternion.Lerp(this.parent.transform.localRotation, q, Time.deltaTime * 10);
                    this.parent.GotoAttack();
                }

                isBreakNormalBehauviour = true;

            }
            else
            {
                if (!isBreakNormalBehauviour)
                {
                    MoveByDir(parent.Dir);
                }
                else
                {
                    MoveByDir(CalNavmeshDir());
                }
            }
        }
    }

    private Vector3 CalNavmeshDir()
    {
        Agent.CalculatePath(parent.CurrentTarget.position, path);
        var dir = ( path.corners[1] - parent.position).normalized;
        return dir;
    }
    private void MoveByDir(Vector3 dir)
    {
        var q = Quaternion.LookRotation(dir, Vector3.up);
        this.parent.transform.localRotation =
            Quaternion.Lerp(this.parent.transform.localRotation, q, Time.deltaTime * 10);

        this.parent.controller.Move(this.parent.controller.transform.forward.normalized * Time.deltaTime *
                                    speedMove);
        this.parent.dataBinding.Speed = speedMove;
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

    public void OnDrawGizmos()
    {
        
    }
}