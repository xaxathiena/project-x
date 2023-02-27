using StateMachine;
using UnityEngine;

public class SnakeletIdleState: IState
{
    [HideInInspector] public SnakeletControl parent;

    private float currentTime;
    
    public bool canTransitionToSelf { get; }
    public void Initialize(FSMSystem parent, params object[] datas)
    {
        currentTime = 0f;
    }

    public void OnEnter(params object[] data)
    {
        Debug.Log("OnEnter idle");
        currentTime = 0f;
        parent.dataBinding.Speed = 0;
        parent.agent.enabled = false;
        Debug.Log("Obstackle enableed = " + true);
        parent.obsTackle.enabled = true;
    }

    public void OnEnterFromSameState(params object[] data)
    {
    }

    public void OnUpdate()
    {
        parent.GotoMove();
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