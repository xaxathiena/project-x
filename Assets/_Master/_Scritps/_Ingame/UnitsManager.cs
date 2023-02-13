using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;

public class UnitsManager : Singleton<UnitsManager>
{
    public List<IUnit> towers = new List<IUnit>();
    public List<IUnit> units = new List<IUnit>();

    public void AddTower(IUnit tower)
    {
        towers.Add(tower);
    }
    public void RemoveTower(IUnit tower)
    {
        if (towers.Contains(tower))
        {
            towers.Remove(tower);
        }
    }

    public void AddUnit(IUnit unit)
    {
        units.Add(unit);
    }
    public void RemoveUnit(IUnit unit)
    {
        if (units.Contains(unit))
        {
            units.Remove(unit);
        }
    }
    public void GetUnitInRange(ref List<IUnit> result, Vector3 center, float range, UnitSide side)
    {
        foreach (var unit in units)
        {
            if(unit.unitSide == side) continue;
            var isInside = center.IsPositionInRange(unit.position, range, unit.boderRange);
            if (isInside)
            {
                result.Add(unit);
            }
        }
    }

    public IUnit GetNearestTower(Vector3 center)
    {
        IUnit nearestUnit = null;
        float minDistance = float.MaxValue;
        float currentDistance = float.MaxValue;
        foreach (var unit in towers)
        {
            currentDistance = center.DistanceSQR(unit.position);
            if (currentDistance < minDistance)
            {
                minDistance = currentDistance;
                nearestUnit = unit;
            }
        }

        return nearestUnit;
    }
    public void GetTowerInRange(ref List<IUnit> result, Vector3 center, float range, UnitSide side)
    {
        foreach (var unit in towers)
        {
            if(unit.unitSide == side) continue;
            var isInside = center.IsPositionInRange(unit.position, range, unit.boderRange);
            
            if (isInside)
            {
                result.Add(unit);
            }
        }
    }

}
