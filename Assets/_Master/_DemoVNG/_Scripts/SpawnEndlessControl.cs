using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnEndlessControl : MonoBehaviour
{
    [SerializeField] private int[] enemiesID;
    [SerializeField] private float timeToSpawnUnits;
    [SerializeField] private float timeUpgradeLevel;
    [SerializeField] private int startEnemyNumber = 3;
    [SerializeField] private int numberInscreasePerLevel = 3;
    private float currentTimeToUpgradeLevel ;
    private float currentTimeToSpawnUnits ;
    private int currentNumberSpawn;

    private void Start()
    {
        currentNumberSpawn = startEnemyNumber;
        currentTimeToSpawnUnits = 0;
        currentTimeToUpgradeLevel = 0;
    }

    private void Update()
    {
        currentTimeToUpgradeLevel += Time.deltaTime;
        currentTimeToSpawnUnits += Time.deltaTime;
        if (currentTimeToUpgradeLevel > timeUpgradeLevel)
        {
            currentNumberSpawn += numberInscreasePerLevel;
            currentTimeToUpgradeLevel = 0;
        }

        if (currentTimeToSpawnUnits > timeToSpawnUnits)
        {
            currentTimeToSpawnUnits = 0;
            SpawnUnits();
        }
    }

    private void SpawnUnits()
    {
        for (int j = 0; j < currentNumberSpawn; j++)
        {
            Vector3 position = InGameManager.instance.GetPositionInBoder();
            var configUnit = ConfigManager.instance.ConfigUnit.GetRecordBykeySearch(enemiesID[Random.Range(0,enemiesID.Length)]);
            // var enemy = PoolManager.instance.GetPool<ObjectPoolControl>(configUnit.Name);
            var enemy = Instantiate(Resources.Load("_Units/" + configUnit.Name) as GameObject);
            for (int i = 0; i < currentNumberSpawn; i++)
            {
                SpawnUnit(position, enemy, configUnit);
            }
        }
        
    }
    private  void SpawnUnit(Vector3 position, GameObject unitObject, UnitRecord record)
    {
        var iUnit = unitObject.GetComponent<IUnit>();
        unitObject.transform.position = position;
        iUnit.OnSetup(new UnitData()
        {
            config = record
        });
        iUnit.UnitSpawn();
    }
    private  void SpawnUnit(Vector3 position, ObjectPoolControl unitObject, UnitRecord record)
    {
        unitObject.OnPlay(new DataObjectPool()
        {
            position = position,
        });
        var iUnit = unitObject.MainObj.GetComponent<IUnit>();
        iUnit.OnSetup(new UnitData()
        {
            config = record
        });
    }
}
