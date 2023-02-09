
    using StateMachine;
    using UnityEngine;

    public class UnitDefaultAttackState : IState
    {
        public UnitDefaultControl parent;

        private float currentTime;
    
        public bool canTransitionToSelf { get; }
        public void Initialize(FSMSystem parent)
        {
            currentTime = 0f;
        }

        public void OnEnter(params object[] data)
        {
            currentTime = 0f;
            Debug.Log("Enter spawn");
            parent.dataBinding.Speed = 0;
            parent.dataBinding.Attack = true;
        }

        public void OnEnterFromSameState(params object[] data)
        {
        }

        public void OnUpdate()
        {
            currentTime += Time.deltaTime;
            if(currentTime > 1) parent.GotoDead();
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