using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    [Header("Setting")] 
    public int numberBullet;

    public int rof;
    public float rangeAttack;
    public string bulletName;
    private List<Transform> targets = new List<Transform>();
    private void FixedUpdate()
    {
        targets.Clear();
        UnitsManager.instance.GetEnemiesInRange(ref targets, transform.position, rangeAttack);
        targets.Sort(ComparePosition);
        for (int i = 0; i < numberBullet; i++)
        {
            if (targets.Count < numberBullet)
            {
                var bullet= PoolManager.GetPool(bulletName);
                // bullet
            }
        }
        
    }

    private int ComparePosition(Transform x, Transform y)
    {
        return Vector3.Distance(y.position, this.transform.position) >
               Vector3.Distance(y.position, this.transform.position)? 1: -1;
    }
}

// public class ComparePosition : IComparer<>
// {
//     public int CompareTo(object obj)
//     {
//         throw new NotImplementedException();
//     }
// }
