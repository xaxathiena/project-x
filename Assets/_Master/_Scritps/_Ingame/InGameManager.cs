using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

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
    [SerializeField] private Vector2 zBoder;
    
    
    public IUnit MotherTreePosition => theMortherTree;
    protected override void OnAwake()
    {
        DataTrigger.RegisterValueChange(DataPath.GAME_STATUS, (value) =>
        {
            GameStatus status = (GameStatus)value;
            if (status == GameStatus.StartGame)
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
        });
    }

    public void Start()
    {
        
    }
    
    
    
    public Vector3 ClaimPositionInMap(Vector3 position)
    {
        position.x = Mathf.Clamp(position.x, xBoder.x + 0.5f, xBoder.y -0.5f);
        position.z = Mathf.Clamp(position.z, zBoder.x + 0.5f, zBoder.y - 0.5f);
        return position;
    }

    public Vector3 GetPositionInBoder()
    {
        Vector3 position = Vector3.zero;
        position.y = 0;
        int pos = Random.Range(0, 4);
        float border = 5;
        switch (pos)
        {
            case 0:
                position.x = xBoder.x + border;
                position.z = Random.Range(zBoder.x + border, zBoder.y - border);
                break;
            case 1:
                position.x = xBoder.y - border;
                position.z = Random.Range(zBoder.x + border, zBoder.y - border);
                break;
            case 2:
                position.x = Random.Range(xBoder.x + border, xBoder.y -border);
                position.z = zBoder.x + border;
                break;
            case 3:
                position.x = Random.Range(xBoder.x + border, xBoder.y - border);
                position.z = zBoder.y - border;
                break;
        }
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
