using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;

public class UnitsManager : Singleton<UnitsManager>
{
    [SerializeField] private int initPlayerLevel;
    [SerializeField] private int expPerLevel;
    [SerializeField] private int expPerLevelPlus;
    private float currentPlayerExp;
    private float currentNextPlayerExp = 0;
    public List<IUnit> towers = new List<IUnit>();
    public List<IUnit> units = new List<IUnit>();
    private int currentUUID = 1;
    private int numberEnemyDead = 0;
    private int numberEnemyAlive = 0;
    private int currentPlayerLevel;
    private int NumberEnemyDead
    {
        set
        {
            numberEnemyDead = value;
            numberEnemyDead.TriggerEventData(DataPath.INGAME_ENEMY_DEAD);
        }
        get => numberEnemyDead;
    }
    private int NumberEnemyAlive
    {
        set
        {
            numberEnemyAlive = value;
            numberEnemyAlive.TriggerEventData(DataPath.INGAME_ENEMY_ALIVE);
        }
        get => numberEnemyAlive;
    }

    protected override void OnAwake()
    {
        DataTrigger.RegisterValueChange(DataPath.GAME_STATUS, value =>
        {
            GameStatus status = (GameStatus)value;
            if (status == GameStatus.StartGame || status == GameStatus.EndGame)
            {
                ResetParameter();
            }
        });
    }
    
    private void ResetParameter()
    {
        foreach (var u in towers)
        {
            GameObject.DestroyImmediate(u.obj);
        }
        foreach (var u in units)
        {
            GameObject.DestroyImmediate(u.obj);
        }
        towers.Clear();
        units.Clear();
        
        currentNextPlayerExp = 0;
        currentUUID = 1;
        numberEnemyDead = 0;
        numberEnemyAlive = 0;
        currentPlayerLevel = initPlayerLevel;
        
        SetPlayerNextLevel();
        currentPlayerLevel.TriggerEventData(DataPath.INGAME_PLAYER_UPLEVEL);
    }
    private void Start()
    {
        currentPlayerLevel = initPlayerLevel;
        SetPlayerNextLevel();
    }

    private void SetPlayerNextLevel()
    {
        currentPlayerExp = 0;
        currentNextPlayerExp += expPerLevel + currentPlayerLevel * expPerLevelPlus;
        currentPlayerExp.TriggerEventData(DataPath.INGAME_PLAYER_EXP);
        currentNextPlayerExp.TriggerEventData(DataPath.INGAME_PLAYER_NEXT_EXP);
    }
    private void AddExpPlayer(int exp)
    {
        currentPlayerExp += exp;
        if (currentPlayerExp >= currentNextPlayerExp)
        {
            SetPlayerNextLevel();
            currentPlayerLevel++;
            currentPlayerLevel.TriggerEventData(DataPath.INGAME_PLAYER_UPLEVEL);
        }
        currentPlayerExp.TriggerEventData(DataPath.INGAME_PLAYER_EXP);
    }
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
        if (unit.unitSide == UnitSide.Enemy)
        {
            NumberEnemyAlive++;
        }
    }
    public void RemoveUnit(IUnit unit)
    {
        unit.IsDead = true;
        units.Remove(unit);
        NumberEnemyDead++;
        NumberEnemyAlive--;
        AddExpPlayer(400);
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
