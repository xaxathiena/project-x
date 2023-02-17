using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherDefaultCotrol : MonoBehaviour, IUnit
{
    public UnitSide unitSide { get => UnitSide.Ally; }
    public float boderRange { get => 2f; }
    public Vector3 position { get => transform.position; }
    public Quaternion rotation { get => transform.rotation; }

    public ArcherDefaultDataBinding dataBinding;
    Vector3 moveDir = Vector3.zero;
    private Transform trans;
    public float speedRotate=5;
    public float speedMove = 5;
    public CharacterController controller;
    public Transform anchorFootTrackMove;
    public LayerMask maskBG;
    public float rof = 0.2f;
    private bool isFire;
    private float timeCount;
    private bool IsFire
    {
        set
        {
            if (value)
            {
                timeCount = 0;
                dataBinding.OnFire = true;
            }

            isFire = value;
        }
        get => isFire;
    }

    private void Start()
    {
        trans = transform;  
        InputManager.instance.OnFireHandle+=OnFireHandle;
    }

    private void OnFireHandle()
    {
        if (IsFire) return;
        IsFire = true;
    }

    private void Update()
    {
        moveDir = InputManager.moveDir;
        timeCount += Time.deltaTime;
        if (moveDir.magnitude > 0)
        {
            Quaternion q = Quaternion.LookRotation(moveDir, Vector3.up);

            Quaternion qc = trans.localRotation;

            qc = Quaternion.Slerp(qc, q, Time.deltaTime * speedRotate);

            trans.localRotation = qc;
            // trans.Translate(Vector3.forward * moveDir.magnitude * Time.deltaTime * speedMove);
                
        }
        dataBinding.SpeedMove = moveDir.magnitude;
        if (!Physics.Raycast(anchorFootTrackMove.position, -trans.up, 1, maskBG))
        {
            moveDir.y = -1f;
        }
        controller.Move(moveDir * Time.deltaTime * speedMove);
        dataBinding.SpeedMove = controller.velocity.magnitude;
        if (timeCount >= rof)
        {
           
            IsFire = false;
            // currentEnemy = null;
        }
    }

    public void UnitSpawn()
    {
        UnitsManager.instance.AddUnit(this);
    }

    public void UnitDestroy()
    {
        UnitsManager.instance.RemoveUnit(this);
    }

    public Vector3 Dir { get; set; }
    public bool IsReceiveDirective { get; set; }
    public void ApplyDamage(AttackData data)
    {
    }
}
