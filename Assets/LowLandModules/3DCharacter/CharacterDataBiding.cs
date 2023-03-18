using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDataBiding : MonoBehaviour
{
    public Animator animator;
    
    public float SpeedMove
    {
        set
        {
            if(value>0)
                animator.applyRootMotion = false;

            animator.SetFloat(key_SpeedMove, value);
        }
    }
    public int IndexCombo
    {
        set
        {
            animator.SetInteger(key_indexCombo, value);
        }
    }
    public bool Attack
    {
        set
        {
            if(value)
            {
                animator.applyRootMotion = false;
                animator.SetTrigger(key_Attack);

            }
        }
    }

    public int Skill
    {
        set
        {
            animator.SetFloat(key_SpeedMove, 0);
            animator.applyRootMotion = true;    
            animator.SetInteger(key_NumberSkill, value);
            animator.SetTrigger(key_SkillTrigger);
        }
    }
    public bool Dash
    {
        set
        {
            if(value)
            {
                animator.applyRootMotion = false;
                animator.SetTrigger(key_Dash);
            }
        }
    }
    public bool KnockBack
    {
        set
        {
            if(value)
            {
                animator.applyRootMotion = false;
                animator.SetTrigger(key_KnockBack);
            }
        }
    }
    private int key_SpeedMove;
    private int key_indexCombo;
    private int key_Attack;

    private int key_NumberSkill;
    private int key_Dash;

    private int key_SkillTrigger;

    private int key_KnockBack;
    // Start is called before the first frame update
    void Start()
    {
        key_SpeedMove = Animator.StringToHash("SpeedMove");
        key_indexCombo = Animator.StringToHash("indexCombo");
        key_Attack = Animator.StringToHash("Attack");
        
        key_NumberSkill = Animator.StringToHash("NumberSkill");
        key_SkillTrigger = Animator.StringToHash("Skill");
        key_Dash = Animator.StringToHash("Dash");
        key_KnockBack = Animator.StringToHash("KnockBack");
    }

    
}

[System.Serializable]
public class AnimationData
{
    public float timeAttak;
    public float timeAnim;
    public int index;
    public float force;
    public float angleForce;
    public float dashLimit;
}