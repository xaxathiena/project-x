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
            if (Vector3.Distance(unit.position, center) - unit.boderRange <= range)
            {
                result.Add(unit);
            }
        }
    }
    public void GetTowerInRange(ref List<IUnit> result, Vector3 center, float range, UnitSide side)
    {
        foreach (var unit in towers)
        {
            if(unit.unitSide == side) continue;
            if (Vector3.Distance(unit.position, center)- unit.boderRange <= range)
            {
                result.Add(unit);
            }
        }
    }

}
