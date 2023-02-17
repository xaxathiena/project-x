using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ArcherDefaultCotrol : MonoBehaviour, IUnit
{
    [SerializeField] private string bulletName;
    [SerializeField] private Transform positionSpawnArror;
    [SerializeField]private Transform arrowTarget;
    [SerializeField] private HealBarController healBarController;
    public int uuid { get; set; }
    public UnitSide unitSide { get => UnitSide.Ally; }
    public float boderRange { get => 2f; }
    public Vector3 position { get => transform.position; }
    public Quaternion rotation { get => transform.rotation; }

    public ArcherDefaultDataBinding dataBinding;
    Vector3 moveDir = Vector3.zero;
    private Transform trans;
    public float speedRotate=5;
    public float speedMove = 5;
    public CharacterController controller;
    public Transform anchorFootTrackMove;
    public LayerMask maskBG;
    public float rof = 0.2f;
    private bool isFire;
    private float timeCount;
    public float arrowSpeed;
    public float rangeDetect;
    private IUnit currentEnemy;
    private Quaternion targetDir;
    public float currentHealth = 1000;
    public float maxHealth= 1000;
    private bool IsFire
    {
        set
        {
            if (value)
            {
                timeCount = 0;
                dataBinding.OnFire = true;
                var bullet = PoolManager.instance.GetPool<BulletControl>(bulletName);
                currentEnemy = GetTarget();
                if(currentEnemy!=null)
                 {
                    var dir = (currentEnemy.position - trans.position).normalized;
                    Quaternion q = Quaternion.LookRotation(dir, Vector3.up);
                    targetDir = q;
                    trans.localRotation = q;
                    bullet.Fire(positionSpawnArror.position,currentEnemy.position, arrowSpeed, 
                        new AttackData()
                        {
                            damage =  50,
                            unit = currentEnemy
                        }  
                    );
                }
                else
                {
                    bullet.Fire(positionSpawnArror.position, arrowTarget.position,arrowSpeed, new AttackData()
                        {
                            damage =  10,
                            unit = null
                        }  
                    );
                }
            }

            isFire = value;
        }
        get => isFire;
    }

    private IEnumerator  Start()
    {
        trans = transform;  
        InputManager.instance.OnFireHandle+=OnFireHandle;
        yield return null;
        targetDir = trans.localRotation;
        UnitSpawn();
        healBarController.SetupHealth(maxHealth, 0, currentHealth);
    }

    private void OnFireHandle()
    {
        if (IsFire) return;
        IsFire = true;
    }

    private void Update()
    {
        timeCount += Time.deltaTime;
        moveDir = InputManager.moveDir;
        Quaternion qc = trans.localRotation;
        
        if (moveDir.magnitude > 0)
        {
            Quaternion q = Quaternion.LookRotation(moveDir, Vector3.up);
            targetDir = q;
            qc = Quaternion.Slerp(qc, q, Time.deltaTime * speedRotate);
            trans.localRotation = qc;
        }
        dataBinding.SpeedMove = moveDir.magnitude;  
        if (!Physics.Raycast(anchorFootTrackMove.position, -trans.up, 1, maskBG))
        {
            moveDir.y = -1f;
        }
        controller.Move(moveDir * Time.deltaTime * speedMove);
            dataBinding.SpeedMove = controller.velocity.magnitude;
        if (timeCount >= rof)
        {
            IsFire = false;
            // currentEnemy = null;
        }
        //Rotation
        qc = Quaternion.Slerp(qc, targetDir, Time.deltaTime * speedRotate);
        trans.localRotation = qc;
    }

    public void UnitSpawn()
    {
        UnitsManager.instance.AddUnit(this);
    }

    public void UnitDestroy()
    {
        UnitsManager.instance.RemoveUnit(this);
    }

    public Vector3 Dir { get; set; }
    public bool IsReceiveDirective { get; set; }
    public void ApplyDamage(AttackData data)
    {
        currentHealth -= data.damage;
        healBarController.SetupHealth(currentHealth, 0, maxHealth);
    }

    public bool IsDead { get; set; }

    public IUnit GetTarget()
    {
        currentEnemy = null;
        IUnit enemy = null;

        List<EnemyTargetSelect> lstarget = GetTargetInRange();
        lstarget.Sort();
        if(lstarget.Count>0)
        {
            enemy = lstarget[0].enemyControl;
        }
       
        return enemy;
    }
    private List<IUnit> enemyInRange = new List<IUnit>();
    public List<EnemyTargetSelect> GetTargetInRange()
    {

        int enemyMask = 1 << 9;
        enemyInRange.Clear();
        UnitsManager.instance.GetUnitInRange(ref enemyInRange, trans.position, rangeDetect, unitSide );
        
        // Collider[] hitColliders = Physics.OverlapSphere(trans.position, rangeDetect, enemyMask);

        List<EnemyTargetSelect> lstarget = new List<EnemyTargetSelect>();
        foreach (IUnit e in enemyInRange)
        {
            float dis = Vector3.Distance(e.position, trans.position);
            lstarget.Add(new EnemyTargetSelect { enemyControl = e, distance = dis });
        }
        lstarget.Sort();
        return lstarget;
    }
    private void OnDrawGizmos()
    {
        if(trans!=null)
        {
            UnityEditor.Handles.color = Color.yellow;
        }
   
    }
}
