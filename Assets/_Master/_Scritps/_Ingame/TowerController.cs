using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour, IUnit
{
    public UnitSide unitSide { get => UnitSide.Ally; }
    public float boderRange { get => 5f; }
    public Vector3 position { get => transform.position; }
    public Quaternion rotation { get => transform.rotation; }
    public void UnitSpawn()
    {
        UnitsManager.instance.AddTower(this);
    }

    public void UnitDestroy()
    {
        UnitsManager.instance.RemoveTower(this);
    }

    public Vector3 Dir { get => transform.forward; set => transform.forward = value; }
    public bool IsReceiveDirective { get; set; }

    [Header("Setting parameter")] 
    public int numberBullet;
    public float rof;
    public float rangeAttack;
    public string bulletName;
    public float speed;
    [Header("Setting in scene")] 
    [SerializeField] private Transform firePoint;
    private List<IUnit> targets = new List<IUnit>();
    private float timeToAttack = .2f;
    private float currentTime = 0f;
    private bool isAttacking = false;
    private void Start()
    {
        IsReceiveDirective = false;
        currentTime = 0f;
        UnitSpawn();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime < timeToAttack && !isAttacking)
        {
            return;
        }

        targets.Clear();
        UnitsManager.instance.GetUnitInRange(ref targets, transform.position, rangeAttack, unitSide);
        if (targets.Count > 0)
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
            currentTime = 0f;
            return;
        }
        
        if (isAttacking)
        {
            if (currentTime < rof) return;
        }
        currentTime = 0f;
        targets.Sort(ComparePosition);
        for (int i = 0; i < numberBullet; i++)
        {
            if (i < targets.Count)
            {
                // bullet
                var bullet= PoolManager.instance.GetPool<BulletControl>(bulletName);
                bullet.Fire(firePoint.position, targets[i].position, speed);
            }
        }

    }


    private int ComparePosition(IUnit x, IUnit y)
    {
        return Vector3.Distance(y.position, this.transform.position) >
               Vector3.Distance(y.position, this.transform.position)? 1: -1;
    }
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeAttack);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, boderRange);
    }

    
}

// public class ComparePosition : IComparer<>
// {
//     public int CompareTo(object obj)
//     {
//         throw new NotImplementedException();
//     }
// }
