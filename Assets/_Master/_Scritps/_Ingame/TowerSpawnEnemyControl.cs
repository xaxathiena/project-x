using System;
using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

public class TowerSpawnEnemyControl : MonoBehaviour
{
    [SerializeField] private Transform[] enemiesPositionSpawn;
    [SerializeField] private UnitDefaultControl prefabTest;

    public void Awake()
    {
        InGameManager.TimeToSpawnUnitsEvent += SpawnUnitHandle;
    }
    private void SpawnUnitHandle()
    {
        for (int i = 0; i < enemiesPositionSpawn.Length; i++)
        {
            var newUnit = Instantiate(prefabTest);
            newUnit.transform.position = enemiesPositionSpawn[i].position;
            newUnit.transform.forward = enemiesPositionSpawn[i].right;
            UnitsManager.instance.AddUnit(newUnit);
        }
    }
}
