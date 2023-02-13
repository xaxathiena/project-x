using StateMachine;
using UnityEngine;

[System.Serializable]
public class  UnitDefaultDeadState : IState
{
    [HideInInspector] public UnitDefaultControl parent;

    private float currentTime;
    private float timeDead = 1.5f;
    public bool canTransitionToSelf { get; }
    private float currentDissolveAmount = 0f;
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
        currentTime += Time.deltaTime;
        if (currentTime > timeDead)
        {
            currentDissolveAmount += Time.deltaTime;
            if (currentDissolveAmount >= 1)
            {
                this.parent.gameObject.SetActive(false);
            }
            else
            {
                foreach (var mat in parent.mat)
                {
                    mat.material.SetFloat("_DissolveAmount", currentDissolveAmount * 0.3f);
                }
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

    public void OnDrawGizmos()
    {
        
    }
}