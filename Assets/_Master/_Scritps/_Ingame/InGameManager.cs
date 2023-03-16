using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
    [SerializeField] private Vector2 xBoder;
    [FormerlySerializedAs("yBoder")] [SerializeField] private Vector2 zBoder;
    
    public IUnit MotherTreePosition => theMortherTree;

    public void Start()
    {
        currentTime = 0;
        if (characterPrefab != null)
        {
            var go = Instantiate(characterPrefab);
            go.transform.position = characterPosition.position;
            targetCamera.target = go.transform;
            targetCamera.isFlowTarget = true;
        }
    }

    public Vector3 ClaimPositionInMap(Vector3 position)
    {
        position.x = Mathf.Clamp(position.x, xBoder.x + 0.5f, xBoder.y -0.5f);
        position.z = Mathf.Clamp(position.z, zBoder.x + 0.5f, zBoder.y - 0.5f);
        return position;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(new Vector3(xBoder.x, 1, zBoder.x), new Vector3(xBoder.x, 1, zBoder.y));
        Gizmos.DrawLine(new Vector3(xBoder.y, 1, zBoder.x), new Vector3(xBoder.y, 1, zBoder.y));
        Gizmos.DrawLine(new Vector3(xBoder.x, 1, zBoder.x), new Vector3(xBoder.y, 1, zBoder.x));
        Gizmos.DrawLine(new Vector3(xBoder.x, 1, zBoder.y), new Vector3(xBoder.y, 1, zBoder.y));
        
        
        
        Gizmos.DrawLine(new Vector3(xBoder.x, 3, zBoder.x), new Vector3(xBoder.x, 3, zBoder.y));
        Gizmos.DrawLine(new Vector3(xBoder.y, 3, zBoder.x), new Vector3(xBoder.y, 3, zBoder.y));
        Gizmos.DrawLine(new Vector3(xBoder.x, 3, zBoder.x), new Vector3(xBoder.y, 3, zBoder.x));
        Gizmos.DrawLine(new Vector3(xBoder.x, 3, zBoder.y), new Vector3(xBoder.y, 3, zBoder.y));
    }
}
