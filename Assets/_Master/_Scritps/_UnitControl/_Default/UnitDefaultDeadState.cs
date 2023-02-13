using StateMachine;
using UnityEngine;

[System.Serializable]
public class  UnitDefaultDeadState : IState
{
    [HideInInspector] public UnitDefaultControl parent;

    private float currentTime;
    
    public bool canTransitionToSelf { get; }
    public void Initialize(FSMSystem parent, params object[] datas)
    {
        currentTime = 0f;
    }

    public void OnEnter(params object[] data)
    {
        currentTime = 0f;
        parent.dataBinding.IsDead = true;
        parent.agent.enabled = false;
        Debug.Log("Obstackle enableed = " + false);
        parent.obsTackle.enabled = false;
        parent.IsShowHealBar = false;
    }

    public void OnEnterFromSameState(params object[] data)
    {
    }

    public void OnUpdate()
    {
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