using StateMachine;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class UnitDefaultMoveState: IState
{
    [HideInInspector] public UnitDefaultControl parent;

    private NavMeshAgent Agent => parent.agent;
    private NavMeshPath path;
    [SerializeField]
    private bool move;

    private float speedMove;
    private float playerDistance;
    private Vector3 targetPosition;
    public Vector3 nextPos = Vector3.zero;
    public bool canTransitionToSelf { get; }
    public void Initialize(FSMSystem parent, params object[] datas)
    {
        path = new NavMeshPath();
        // Agent = this.parent.agent;
        speedMove = 3f;
        playerDistance = 99f;
        this.parent.dataBinding.Speed = speedMove;
        move = false;
    }

    public void OnEnter(params object[] data)
    {
        //Set agen
        
        Debug.Log("Enter move");
        targetPosition = InGameManager.instance.MotherTreePosition;
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
            playerDistance = Vector3.Distance(parent.transform.position, targetPosition);
            if (playerDistance < parent.attackRange)
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
                // if (path.corners.Length > 0)
                // {
                //     if (Vector3.Distance(parent.transform.position, path.corners[1]) < delta)
                //     {
                //         Agent.CalculatePath(targetPosition,path);
                //     }
                // }
                // else
                // {
                //     Agent.CalculatePath(targetPosition,path);
                // }
                Agent.CalculatePath(targetPosition,path);
                nextPos = path.corners[1];
                //Move
                var dir = (path.corners[1] - this.parent.transform.position).normalized;
                var q = Quaternion.LookRotation(dir, Vector3.up);
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