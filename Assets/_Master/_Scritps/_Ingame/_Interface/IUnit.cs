using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IUnit
{
    UnitSide unitSide { get; }
    float boderRange { get;}
    Vector3 position { get; }
    Quaternion rotation { get; }
    void UnitSpawn();
    void UnitDestroy();
    Vector3 Dir { get; set; }
    bool IsReceiveDirective { get; set; }
}

public enum UnitSide
{
    Enemy,
    Ally
}