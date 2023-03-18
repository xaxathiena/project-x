    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class  UnitDefaultDataBinding
{ 
    private Animator animator { get; set; }
    private int key_Speed;
    private int key_IsDead;
    private int key_Attack;
    private int key_KnockBack;
    /// <summary>
    /// 0 => idle
    ///  0 < speed < 1: walk => run
    /// </summary>
    public float Speed
    {
        set
        {
            animator.SetFloat(key_Speed, value);
        }
    }
    public bool IsDead
    {
        set
        {
            animator.SetBool(key_IsDead, value);
        }
    }

    public bool Attack
    {
        set
        {
            if (value)
            {
                animator.SetTrigger(key_Attack);
            }
        }
    }
    public bool KnockBack
    {
        set
        {
            if (value)
            {
                animator.SetTrigger(key_KnockBack);
            }
        }
    }
    public void Init(Animator animator)
    {
        this.animator = animator;
        key_Speed = Animator.StringToHash("Speed");
        key_IsDead = Animator.StringToHash("IsDead");
        key_Attack = Animator.StringToHash("Attack");
        key_KnockBack = Animator.StringToHash("KnockBack");
    }

    public void OnUpdate()
    {
        
    }
    public void OnFixedUpdate()
    {
        
    }
   
}
