using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimStateBehaviour : StateMachineBehaviour
{
    //private CharacterControl characterControl;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // if(characterControl==null)
        //     characterControl=animator.GetComponentInParent<CharacterControl>();
        // characterControl.CheckAiming(true);
        // base.OnStateEnter(animator, stateInfo, layerIndex);
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // characterControl.CheckAiming(false);
        // base.OnStateExit(animator, stateInfo, layerIndex);
    }
}
