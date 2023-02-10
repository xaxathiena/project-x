using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StateMachine
{
    public class FSMSystem : MonoBehaviour
    {
        protected bool isDispose = true;
        public IState currentState;
        public List<IState> states = new List<IState>();
        protected IState entryState;
        private object[] entryData;
        public void RegisterState(IState newState)
        {
            if(states.Contains(newState))
            {
                return;
            }
            states.Add(newState);
            newState.Initialize(this);
        }
        
        public void SetEntryState(IState entryState,params object[] entryData)
        {
            RegisterState(entryState);
            this.entryState = entryState;
            this.entryData = entryData;
        }
        public void SetEntryState(IState entryState)
        {
            RegisterState(entryState);
            this.entryState = entryState;
            this.entryData = null;
        }
        public virtual void SystemStart()
        {
            //for (int i = 0; i < states.Count; i++)
            //{
            //    states[i].Initialize(this);
            //}
            isDispose = false;
            if (Equals(entryState, null))
                entryState = states[0];
            currentState = entryState;
            currentState.OnEnter(entryData);
        }
        public void GotoState(IState newState,params object[] data)
        {
            if (isDispose)
            {
                Debug.LogError($"{gameObject.name} Trying to enter {newState} but it been disposed then nothing will happend");
                return;
            }
            if (currentState != null)
            {
                currentState.OnExit();
            }
            if (currentState != newState || newState == null)
            {
                currentState = newState;
                
                currentState?.OnEnter(data);
            }
            else if (newState.canTransitionToSelf)
            {
                currentState?.OnEnterFromSameState(data);
            }
        }
        #region Mono Callbacks
        // Update is called once per frame
        private void Update()
        {
            SystemUpdate();
            if (currentState != null)
            {
                currentState.OnUpdate();
            }
        }

        private void FixedUpdate()
        {
            SystemFixedUpdate();
            if (currentState != null)
            {
                currentState.OnFixedUpdate();
            }
        }

        private void LateUpdate()
        {
            SystemLateUpdate();
            if (currentState != null)
            {
                currentState.OnLateUpdate();
            }
        }
        public virtual void SystemUpdate()
        {

        }

        public virtual void SystemFixedUpdate()
        {

        }

        public virtual void SystemLateUpdate()
        {

        }
        private void OnDestroy()
        {
            Dispose();
        }
        public void DisposeState(IState state)
        {
            if (!states.Contains(state))
            {
                return;
            }
            states.Remove(state);
            state.Dispose();
        }
        public virtual void Dispose()
        {
            if(!isDispose)
            {
                isDispose = true;
                for (int i = 0; i < states.Count; i++)
                {
                    states[i].Dispose();
                }
                states.Clear();
                currentState = null;
                entryState = null;
                entryData = null;
            }
        }
        public int ComparePosition(IUnit x, IUnit y)
        {
            return Vector3.Distance(y.position, this.transform.position) >
                   Vector3.Distance(y.position, this.transform.position)? 1: -1;
        }
    }
    #endregion
    
}
