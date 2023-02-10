
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
            parent.obsTackle.enabled = true;
        }

        public void OnEnterFromSameState(params object[] data)
        {
        }

        public void OnUpdate()
        {
            
            currentTime += Time.deltaTime;
            if(currentTime > parent.rof)
            {
                currentTime = 0;
                var playerDistance = Vector3.Distance(parent.transform.position, InGameManager.instance.MotherTreePosition.position) - InGameManager.instance.MotherTreePosition.boderRange;
                if (playerDistance < parent.attackRange)
                {
                    this.parent.dataBinding.Speed = 0f;
                    parent.dataBinding.Attack = true;
                    //this.parent.GotoAttack();
                    //currentData.control.SwiktchAction(Actionkey.ATTACK, currentData);
                }
                else
                {    
                    parent.GotoIdle();
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