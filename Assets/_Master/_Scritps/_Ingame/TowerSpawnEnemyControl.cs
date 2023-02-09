using System;
using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

public class TowerSpawnEnemyControl : MonoBehaviour
{
    [SerializeField] private Transform enemiesPositionSpawn;
    [SerializeField] private GameObject prefabTest;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            var newUnit = Instantiate(prefabTest, enemiesPositionSpawn);
            //newUnit.
        }
    }
}
