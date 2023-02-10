using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    float boderRange { get;}
    Vector3 position { get; }
    Quaternion rotation { get; }
}