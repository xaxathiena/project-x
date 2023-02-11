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
    private int key_SpeedMove;
    private int key_indexCombo;
    private int key_Attack;
    // Start is called before the first frame update
    void Start()
    {
        key_SpeedMove = Animator.StringToHash("SpeedMove");
        key_indexCombo = Animator.StringToHash("indexCombo");
        key_Attack = Animator.StringToHash("Attack");
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