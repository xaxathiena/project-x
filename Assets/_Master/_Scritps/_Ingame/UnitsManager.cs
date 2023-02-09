using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class UnitsManager : Singleton<UnitsManager>
{
    public List<Transform> unitsEnemy = new List<Transform>();

    public void GetEnemiesInRange(ref List<Transform> result, Vector3 center, float range)
    {
        foreach (var unit in unitsEnemy)
        {
            if (Vector3.Distance(unit.position, center) <= range)
            {
                result.Add(unit);
            }
        }
    }
}
