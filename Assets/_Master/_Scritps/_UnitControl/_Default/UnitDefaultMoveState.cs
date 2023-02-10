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
    private NavMeshPath path;

    private float speedMove;
    private IUnit targetUnit;
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
                //Chuyen Qua attack State
                move = false;
                Agent.isStopped = true;
                this.parent.dataBinding.Speed = 0f;
                this.parent.GotoAttack();
                //currentData.control.SwiktchAction(Actionkey.ATTACK, currentData);
            }
            else
            {
                // targetPos = targetUnit.position +
                //             (parent.transform.position - targetUnit.position).normalized *
                //             targetUnit.boderRange;
                // Agent.CalculatePath(targetPos,path);
                // nextPos = path.corners[1];
                //Move
                // var dir = (path.corners[1] - this.parent.transform.position).normalized;
                var q = Quaternion.LookRotation(parent.Dir, Vector3.up);
                this.parent.transform.localRotation  = Quaternion.Lerp(this.parent.transform.localRotation,q, Time.deltaTime*10);
                // Debug.LogError(Vector3.Distance(currentData.control.transform.position, agent.nextPosition));
                // agent.destination = playerTrans.position;
                
                this.parent.controller.Move(this.parent.controller.transform.forward.normalized* Time.deltaTime * speedMove);
                // controlTrans.Translate(Vector3.forward * Time.deltaTime * speedMove);
                // Debug.LogError("updatePosition: " + agent.updatePosition);
                this.parent.dataBinding.Speed =speedMove;
                
            }
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