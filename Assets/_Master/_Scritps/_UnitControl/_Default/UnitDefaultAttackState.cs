
    using StateMachine;
    using UnityEngine;

    public class UnitDefaultAttackState : IState
    {
        public UnitDefaultControl parent;

        private float currentTime;
        private float rof = 0.3f;
        public bool canTransitionToSelf { get; }
        public void Initialize(FSMSystem parent, params object[] datas)
        {
            currentTime = 0f;
        }

        public void OnEnter(params object[] data)
        {
            currentTime = 0f;
            parent.dataBinding.Speed = 0;
            parent.dataBinding.Attack = true;
            parent.agent.enabled = false;
            Debug.Log("Obstackle enableed = " + true);
            parent.obsTackle.enabled = true;
        }

        public void OnEnterFromSameState(params object[] data)
        {
        }

        public void OnUpdate()
        {
            currentTime += Time.deltaTime;
            
            if (parent.CurrentTarget == null)
            {
                parent.GotoIdle();
            }
            else
            {
                var isInSide = parent.position.IsPositionInRange( parent.CurrentTarget.position,parent.CurrentTarget.boderRange);
                if (currentTime > parent.rof)
                {
                    if (isInSide)
                    {
                        var dir = (parent.CurrentTarget.position - this.parent.transform.position).normalized;
                        var q = Quaternion.LookRotation(dir, Vector3.up);
                        this.parent.transform.localRotation = Quaternion.Lerp(this.parent.transform.localRotation,q, Time.deltaTime*10);
                        currentTime = 0;
                        this.parent.dataBinding.Speed = 0f;
                        parent.dataBinding.Attack = true;
                    }
                    else
                    {
                        parent.GotoIdle();
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