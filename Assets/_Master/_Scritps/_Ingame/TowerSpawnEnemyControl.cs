using System;
using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;
using UnityEngine.Serialization;

public class TowerSpawnEnemyControl : MonoBehaviour
{
    [SerializeField] private Transform[] melePositionSpawn;
    [SerializeField] private Transform[] rangerPositionSpawn;
    [SerializeField] private int waveID;
    private WavesRecord _wavesRecord;
    private bool isStart = false;
    private float currentTimeWait = 0f;

    private void Start()
    {
        BootLoader.OnLoadConfigCompleteEvent += () =>
        {
            _wavesRecord = ConfigManager.instance.ConfigWaves.GetRecordBykeySearch(waveID);
            currentTimeWait = _wavesRecord.WaveDuration;
            StartWave();
        };
    }

    private void StartWave()
    {
        if (_wavesRecord != null)
        {
            isStart = true;
        }
        else
        {
            Debug.LogError("Wave with id: " + waveID +" not found" );
        }
    }

    private void Update()
    {
        if (isStart)
        {
            currentTimeWait += Time.deltaTime;

            if (currentTimeWait > _wavesRecord.WaveDuration)
            {
                currentTimeWait = 0;
                SpawnUnitHandle();
            }
        }
    }
    private void SpawnUnitHandle()
    {
        IUnit iUnit;
        for (int i = 0; i < rangerPositionSpawn.Length; i++)
        {
            int idUnit;
            if (_wavesRecord.RangerUnits.Count > 0)
            {
                idUnit = _wavesRecord.RangerUnits.GetRandom();
            }
            else
            {
                idUnit = _wavesRecord.MeleUnits.GetRandom();
            }
            //get random unit id
            var configUnit = ConfigManager.instance.ConfigUnit.GetRecordBykeySearch(idUnit);
            if (configUnit != null)
            {
                var newUnit = Instantiate(Resources.Load("_Units/" + configUnit.Name) as GameObject);
                newUnit.transform.position = rangerPositionSpawn[i].position;
                newUnit.transform.forward = rangerPositionSpawn[i].right;
                iUnit = newUnit.GetComponent<IUnit>();
                iUnit.OnSetup(new UnitData()
                {
                    config = configUnit
                });
                UnitsManager.instance.AddUnit(iUnit);
            }
        }
        for (int i = 0; i < melePositionSpawn.Length; i++)
        {
            int idUnit = _wavesRecord.MeleUnits.GetRandom();
            //get random unit id
            var configUnit = ConfigManager.instance.ConfigUnit.GetRecordBykeySearch(idUnit);
            if (configUnit != null)
            {
                var newUnit = Instantiate(Resources.Load("_Units/" + configUnit.Name) as GameObject);
                newUnit.transform.position = melePositionSpawn[i].position;
                newUnit.transform.forward = melePositionSpawn[i].right;
                iUnit = newUnit.GetComponent<IUnit>();
                iUnit.OnSetup(new UnitData()
                {
                    config = configUnit
                });
                UnitsManager.instance.AddUnit(iUnit);
            }
        }
    }
}
