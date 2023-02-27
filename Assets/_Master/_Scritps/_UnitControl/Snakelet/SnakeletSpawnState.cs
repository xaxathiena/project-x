using StateMachine;
using UnityEngine;

public class SnakeletSpawnState : IState
{
    [HideInInspector] public SnakeletControl parent;

    private float currentTime;
    
    public bool canTransitionToSelf { get; }
    public void Initialize(FSMSystem parent, params object[] datas)
    {
    }

    public void OnEnter(params object[] data)
    {
        parent.dataBinding.Speed = 0f;
    }

    public void OnEnterFromSameState(params object[] data)
    {
    }

    public void OnUpdate()
    {
        currentTime += Time.deltaTime;
        if (currentTime > 3f)
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