using System.Collections;
using System.Collections.Generic;using MoreMountains.Tools;
using StateMachine;
using UnityEngine;

public class UnitControlBase : FSMSystem, IUnit
{
    public virtual int uuid { get; set; }
    public virtual UnitSide unitSide { get; }
    public virtual float boderRange { get; }
    public virtual Vector3 position { get => transform.position;}
    public virtual Quaternion rotation {  get => transform.rotation; }

    public virtual void UnitSpawn()
    {

    }

    public virtual void UnitDestroy()
    {
    }

    public virtual Vector3 Dir { get; set; }
    public virtual bool IsReceiveDirective { get; set; }

    public virtual void ApplyDamage(AttackData data)
    {
    }

    public bool IsDead { get; set; }
}
