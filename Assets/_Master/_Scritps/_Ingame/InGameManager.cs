using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : Singleton<InGameManager>
{
    public static Action TimeToSpawnUnitsEvent;
    // Start is called before the first frame update
    [SerializeField] private TowerController theMortherTree;
    [SerializeField] private float timeToSpawn = 20f;
    [SerializeField] private float currentTime;
    [SerializeField] private bool isSpawn = false;
    [SerializeField] private int maxUnitSpawn = 3;
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private Transform characterPosition;
    [SerializeField] private TargetCamControl targetCamera;
    public IUnit MotherTreePosition => theMortherTree;

    public void Start()
    {
        currentTime = 0;
        if(isSpawn )
            TimeToSpawnUnitsEvent?.Invoke();
        var go = Instantiate(characterPrefab);
        go.transform.position = characterPosition.position;
        targetCamera.target = go.transform;
        targetCamera.isFlowTarget = true;
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > timeToSpawn)
        {
            currentTime = 0f;
            if(UnitsManager.instance.units.Count < maxUnitSpawn && isSpawn )
                TimeToSpawnUnitsEvent?.Invoke();
        }
    }
}
