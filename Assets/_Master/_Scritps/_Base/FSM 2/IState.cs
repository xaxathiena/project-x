using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StateMachine
{
    public interface IState 
    {
        bool canTransitionToSelf { get; }
        void Initialize(FSMSystem parent, params object[] datas);
        void OnEnter(params object[] data);
        void OnEnterFromSameState(params object[] data);
        void OnUpdate();
        void OnLateUpdate();
        void OnFixedUpdate();
        void OnExit();
        void Dispose();
        void OnDrawGizmos();
    }
}
