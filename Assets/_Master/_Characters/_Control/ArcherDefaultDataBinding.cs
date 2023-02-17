using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherDefaultDataBinding : MonoBehaviour
{
    public Animator animator;
    private int key_SpeedMove;
    private int key_Fire;  
    void Start()
    {
        key_SpeedMove = Animator.StringToHash("SpeedMove");
        key_Fire = Animator.StringToHash("OnFire");
    }
    public bool OnFire
    {
        set
        {
            if(value)
            {
                animator.applyRootMotion = false;
                animator.SetTrigger(key_Fire);
                SpeedMove = 0f;
            }
        }
    }
    public float SpeedMove
    {
        set
        {
            if(value>0)
                animator.applyRootMotion = false;

            animator.SetFloat(key_SpeedMove, value);
        }
    }
}
