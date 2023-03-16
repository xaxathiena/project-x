using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IUnit
{
    int uuid { get; set; }
    UnitSide unitSide { get; }
    float boderRange { get;}
    Vector3 position { get; }
    Quaternion rotation { get; }
    void UnitSpawn();
    void UnitDestroy();
    Vector3 Dir { get; set; }
    bool IsReceiveDirective { get; set; }
    void ApplyDamage(AttackData data);
    bool IsDead { get; set; }
    void OnSetup(UnitData data);

}

public enum UnitSide
{
    Enemy,
    Ally
}

public class UnitData
{
    public UnitRecord config;
}