using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPoolControl : MonoBehaviour
{
    [SerializeField] protected GameObject prefab;
    protected GameObject mainObj;
    public GameObject MainObj => mainObj;
    public abstract void OnPlay(DataObjectPool data);
}

public class DataObjectPool
{
    public Vector3 position;
    public Quaternion rotation;
    public float timeLife = 0;
    public object data;
}