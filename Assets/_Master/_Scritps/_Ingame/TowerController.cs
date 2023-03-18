using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TowerController : MonoBehaviour, IUnit
{
    public HealBarController healBarController;
    public int uuid { get; set; }
    public UnitSide unitSide { get => UnitSide.Ally; }
    public float boderRange { get => 5f; }
    public Vector3 position { get => transform.position; }
    public Quaternion rotation { get => transform.rotation; }
    public float dame;
    

    public Vector3 Dir { get => transform.forward; set => transform.forward = value; }
    public bool IsReceiveDirective { get; set; }
    public void ApplyDamage(AttackData data)
    {
        currentHealth -= data.damage;
        healBarController.SetupHealth(currentHealth, 0, maxHealth);
    }

    public bool IsDead { get; set; }
    public void OnSetup(UnitData data)
    {
        
    }

    public GameObject obj { get => gameObject; }

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
    private float currentHealth = 1000000;
    private float maxHealth = 1000000;
    private void Start()
    {
        IsReceiveDirective = false;
        currentTime = 0f;
        UnitSpawn();
        healBarController.SetupHealth(currentHealth, 0, maxHealth);
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
        currentTime = -0.25f;
        targets.Sort(ComparePosition);
        var quene = DOTween.Sequence();
        quene.Append(
            transform.DOScale(Vector3.one * 0.6f, 0.05f)
        ).Append(
                transform.DOScale(Vector3.one, 0.2f).OnComplete(() =>
                {
                    for (int i = 0; i < numberBullet; i++)
                    {
                        if (i < targets.Count)
                        {
                            // bullet
                            var bullet = PoolManager.instance.GetPool<BulletControl>(bulletName);
                            bullet.Fire(firePoint.position, targets[i].position, speed, new AttackData()
                            {
                                damage = dame,
                                unit = targets[i]
                            });
                        }
                    }
                })
        );
        quene.Play();
    }


    private int ComparePosition(IUnit x, IUnit y)
    {
        return y.position.DistanceSQR(this.transform.position) > 
               x.position.DistanceSQR(this.transform.position)? 1: -1;
    }
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeAttack);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, boderRange);
    }

    public void UnitSpawn()
    {
        UnitsManager.instance.AddTower(this);
    }

    public void UnitDestroy()
    {
        UnitsManager.instance.RemoveTower(this);
    }
}

// public class ComparePosition : IComparer<>
// {
//     public int CompareTo(object obj)
//     {
//         throw new NotImplementedException();
//     }
// }
