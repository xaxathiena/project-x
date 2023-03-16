using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPoolControl : MonoBehaviour
{
    public GameObject prefab;
    public abstract void OnPlay(DataObjectPool data);
}

public class DataObjectPool
{
    public Vector3 position;
    public Quaternion rotation;
    public float timeLife;
    public object data;
}