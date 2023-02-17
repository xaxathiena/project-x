using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;

public class UnitsManager : Singleton<UnitsManager>
{
    public List<IUnit> towers = new List<IUnit>();
    public List<IUnit> units = new List<IUnit>();
    private int currentUUID = 1;
    public void AddTower(IUnit tower)
    {
        tower.uuid = currentUUID++;
        towers.Add(tower);
    }
    public void RemoveTower(IUnit tower)
    {
        tower.IsDead = true;
        var towerIndex = towers.FindIndex(i => i.uuid == tower.uuid);
        if (towerIndex != -1)
        {
            towers.RemoveAt(towerIndex);
        }
    }

    public void AddUnit(IUnit unit)
    {
        unit.uuid = currentUUID++;
        units.Add(unit);
    }
    public void RemoveUnit(IUnit unit)
    {
          unit.IsDead = true;
        var unitIndex = units.FindIndex(i => i.uuid == unit.uuid);
        if (unitIndex != -1)
        {
            units.RemoveAt(unitIndex);
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
