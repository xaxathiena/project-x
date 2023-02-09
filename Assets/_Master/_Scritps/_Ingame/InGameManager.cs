using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : Singleton<InGameManager>
{
    public static Action TimeToSpawnUnitsEvent;
    // Start is called before the first frame update
    [SerializeField] private Transform theMortherTree;
    [SerializeField] private float timeToSpawn = 20f;
    [SerializeField] private float currentTime;
    public Vector3 MotherTreePosition => theMortherTree.position;

    public void Start()
    {
        currentTime = 0;
        TimeToSpawnUnitsEvent?.Invoke();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > timeToSpawn)
        {
            currentTime = 0f;
            TimeToSpawnUnitsEvent?.Invoke();
        }
    }
}
