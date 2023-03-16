using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AttackData
{
    public float damage;
    public float force;
    public Transform trans;
    public IUnit unit;
    public LayerMask mask;

}
public class CharacterControl : MonoBehaviour, IUnit
{
    [SerializeField] private HealBarController healBarController;
    [SerializeField] private Transform poitToCastSkill;
    [SerializeField] private string pwBallName;
    [SerializeField] private string pwBallNameSmall;
    [Range(3,10)]
    [SerializeField] private float dashDistance = 4f;
    [Range(0.1f, 1f)]
    [SerializeField] private float timeDash = 0.1f;

    [SerializeField] private GameObject trailObj;
    private UnitData data;
    public CharacterController controller;
    public LayerMask maskBG;
    public Transform anchorFootTrackMove;
    public float rangeDetect;
    public CharacterDataBiding dataBiding;
    private Transform trans;
    private IUnit currentEnemy;
    public List<AnimationData> animaCombo;
    private Dictionary<int, AnimationData> dicAnimCombo = new Dictionary<int, AnimationData>();
    private AnimationData currentAnimationData;
    private float timeCount = 0;
    private float rof = 0;
    public float speedRotate=5;
    public float speedMove = 5;
    public float dame = 5;
    private int indexCombo = 0;
    private bool isFire_;
    public float currentHealth = 1000;
    public float maxHealth= 1000;
    
    private bool isSkilling = false;
    
    private bool IsFire
    {
        set
        {   
            if(value)
            {
                dataBiding.Attack = true;
                indexCombo++;
               
                currentAnimationData = dicAnimCombo[indexCombo];
                rof=currentAnimationData.timeAnim;
                timeCount = 0;
                //check target 
                currentEnemy = GetTarget();
                if(currentEnemy!=null)
                {
                    // thuc hien dash
                    DashTotarget();
                }
               
            }
            else
            {
                if (currentEnemy != null)// 
                {
                   
                }
                else
                {
                    indexCombo = 0;
                }
                if (indexCombo >= 4)
                {
                    indexCombo = 0;
                }
            }
            dataBiding.IndexCombo = indexCombo;
            isFire_ = value;
        }
        get
        {
            return isFire_;
        }
    }
    private int numberCloseAttack_;
    public int numberCloseAttack
    {
        set
        {
            numberCloseAttack_ = value;
        }
        get
        {
            return numberCloseAttack_;
        }
    }
    // public float speedMove = 5;
    // Start is called before the first frame update
    void Start()
    {
        
        trans = transform;
        foreach(AnimationData e in animaCombo)
        {
            dicAnimCombo.Add(e.index, e);
        }
        InputManager.instance.OnFireEvent+=OnFireEvent;
        InputManager.instance.OnSkillEvent += OnSkillHandle;
        InputManager.instance.OnDashEvent += OnDashHandle;
        UnitSpawn();
        healBarController.SetupHealth(currentHealth, 0, currentHealth);
    }

    private void OnDashHandle()
    {
        if (!isSkilling)
        {
            Dash();
        }
    }

    private void OnSkillHandle(int skillIndex)
    {
        if (!isSkilling)
        {
            switch (skillIndex)
            {
                case 1:
                    SkillOne();
                    break;
                case 2:
                    SkillTwo();
                    break;
                case 3:
                    SkillThree();
                    break;
            }
        }
    }

    void OnFireEvent()
    {
        if (IsFire || isSkilling)
            return;
        IsFire = true;
      
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 moveDir = Vector3.zero;
        // atack 
        timeCount += Time.deltaTime;
        if (IsFire)
        { 
            if (currentAnimationData.timeAttak > 0 && timeCount >= currentAnimationData.timeAttak)
            {

                IsFire = false;
            }
        }
        else if(!isSkilling)
        {
            moveDir = InputManager.moveDir;
            if (moveDir.magnitude > 0)
            {
                Quaternion q = Quaternion.LookRotation(moveDir, Vector3.up);

                Quaternion qc = trans.localRotation;

                qc = Quaternion.Slerp(qc, q, Time.deltaTime * speedRotate);

                trans.localRotation = qc;
                // trans.Translate(Vector3.forward * moveDir.magnitude * Time.deltaTime * speedMove);
                
            }
            dataBiding.SpeedMove = moveDir.magnitude;
            if (!Physics.Raycast(anchorFootTrackMove.position, -trans.up, 1, maskBG))
            {
                moveDir.y = -1f;
            }
            controller.Move(moveDir * Time.deltaTime * speedMove);
            //trans.position = InGameManager.instance.ClaimPositionInMap(trans.position);
        }
        if (timeCount >= rof)
        {
           
            IsFire = false;
            currentEnemy = null;

        }
        
    }

    private void UpLevelEffect()
    {
        var trailAfter = PoolManager.instance.GetPool<ObjectPoolControl>("UpLevelEffect");
        trailAfter.OnPlay(new DataObjectPool()
        {
            timeLife = 1f,
            position = trans.position,
        });
    }

    #region Skills

    private void Dash()
    {
        trailObj.SetActive(true);
        dataBiding.Dash = true;
        isSkilling = true;
        float myFloat = 1;
        int numberTrails = 10;
        float timeAddTral = timeDash / numberTrails;
        Debug.Log("timeAddTral " + timeAddTral);
        DOTween.To(()=> myFloat, x=> myFloat = x, 4, timeAddTral).SetLoops(numberTrails).OnStepComplete(() =>
        {
            var trailAfter = PoolManager.instance.GetPool<ObjectPoolControl>("TrailAfter");
            trailAfter.OnPlay(new DataObjectPool()
            {
                timeLife = 1.5f,
                position = trans.position,
                rotation =  Quaternion.identity
            });
        }).SetEase(Ease.Linear);
        Debug.Log("timeDash " + timeDash);
        var targetPosition = InGameManager.instance.ClaimPositionInMap(trans.position + trans.forward * dashDistance);
        trans.DOMove(targetPosition, timeDash).OnComplete(() =>
        {
            isSkilling = false;
            trailObj.SetActive(false);
        }).SetEase(Ease.Linear);
    }
    
    
    
    private void SkillOne()
    {
        dataBiding.Skill = 1;
        isSkilling = true;
    }
    
    private void SkillTwo()
    {
        dataBiding.Skill = 2;
        isSkilling = true;
    }
    private void SkillThree()
    {
        dataBiding.Skill = 3;
        isSkilling = true;
    }
    #endregion
    
    private void OnAnimatorIK(int layerIndex)
    {
        Debug.LogError("ik");
    }

    //private void OnAnimatorMove()
    //{

    //}
    // luuon duoc goi 
    public void OnEventAnimAttack()
    {
        //IsFire = false;
    }
    // se khong duoc goi khi 1 combo moi duoc play 
    public void OnEventAnimAttackEnd()
    {
        //indexCombo = 0;
        //IsFire = false;
        //dataBiding.IndexCombo = indexCombo;
    }
    //
    public IUnit GetTarget()
    {
        currentEnemy = null;
        IUnit enemy = null;

        List<EnemyTargetSelect> lstarget = GetTarget(float.MinValue);
        lstarget.Sort();
        if(lstarget.Count>0)
        {
            enemy = lstarget[0].enemyControl;
        }
       
        return enemy;
    }

    private List<IUnit> enemyInRange = new List<IUnit>();
    public List<EnemyTargetSelect> GetTarget(float dotLimit)
    {

        int enemyMask = 1 << 9;
        enemyInRange.Clear();
        UnitsManager.instance.GetUnitInRange(ref enemyInRange, trans.position, rangeDetect, unitSide );
        // Collider[] hitColliders = Physics.OverlapSphere(trans.position, rangeDetect, enemyMask);

        List<EnemyTargetSelect> lstarget = new List<EnemyTargetSelect>();
        Debug.Log("enemyInRange count " + enemyInRange.Count);
        foreach (IUnit e in enemyInRange)
        {
            Vector3 dir = e.position - trans.position;
            float dot = Vector3.Dot(trans.forward, dir.normalized);//calculator angle

            if (dot >= dotLimit)
            {
                float dis = Vector3.Distance(e.position, trans.position);
                lstarget.Add(new EnemyTargetSelect { enemyControl = e, distance = dis, angle = dot });
            }
        }
        Debug.Log("lstarget count " + lstarget.Count);
        lstarget.Sort();

        return lstarget;
    }
    private void DashTotarget()
    {
        if (currentEnemy != null)
        {
            Vector3 dir = currentEnemy.position - trans.position;
            Quaternion q = Quaternion.LookRotation(dir, Vector3.up);
            trans.localRotation = q;
            float dis = Vector3.Distance(currentEnemy.position,trans.position) + currentEnemy.boderRange;
            if(dis> currentAnimationData.dashLimit)
            {
                Vector3 posAim = currentEnemy.position - dir.normalized * currentEnemy.boderRange;
        
                trans.DOMove(posAim, currentAnimationData.timeAttak).OnComplete(AttackDamge);
            }
            else
            {
                trans.DOMove(trans.position,currentAnimationData.timeAttak).OnComplete(AttackDamge);
            }
        
        }
    }
    private void AttackDamge()
    {
        List<EnemyTargetSelect> ls = GetTarget(currentAnimationData.angleForce);
        AttackData attackData = new AttackData
        {
            damage = dame,
            force = currentAnimationData.force,
            trans = this.trans
        };
        foreach (EnemyTargetSelect e in ls)
        {
            e.enemyControl.ApplyDamage(attackData);
        }
    }
    public void CanMove()
    {
        isSkilling = false;
    }

    public void CasteSkillOne()
    {
        Debug.Log("CasteSkillOne");
        var pwBall = PoolManager.instance.GetPool<PowerBulletControl>(pwBallName);
        pwBall.Fire(poitToCastSkill.position, trans.right, 5, 10, new AttackData(){});
    }
    public void CasteSkillTwo()
    {
        Debug.Log("CasteSkillOne");
        StartCoroutine(IEfireBall());
    }

    private IEnumerator IEfireBall()
    {
        for (int j = 0; j < 4; j++)
        {
            int n = 8;
            float angle = 380 / 8;
            for (int i = 0; i < n; i++)
            {
                var q = Quaternion.LookRotation(trans.right, Vector3.up) * Quaternion.Euler(0, angle * i, 0);
                var pwBall = PoolManager.instance.GetPool<PowerBulletControl>(pwBallNameSmall);
                pwBall.Fire(trans.position, q * trans.right , 5, 30, new AttackData(){});
            }
            yield return new WaitForSeconds(.15f);   
        }
        
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(trans!=null)
        {
            UnityEditor.Handles.color = Color.yellow;
            UnityEditor.Handles.DrawWireArc(trans.position, trans.up, -trans.right, 180, rangeDetect);
        }
   
    }
#endif


    public int uuid { get; set; }
    public UnitSide unitSide { get => UnitSide.Ally; }
    public float boderRange { get => 2f; }
    public Vector3 position { get => trans.position; }
    public Quaternion rotation { get => trans.rotation; }
    public void UnitSpawn()
    {
        UnitsManager.instance.AddUnit(this);
    }

    public void UnitDestroy()
    {
    }

    public Vector3 Dir { get; set; }
    public bool IsReceiveDirective { get; set; }
    public void ApplyDamage(AttackData data)
    {
        currentHealth -= data.damage;
        healBarController.SetupHealth(currentHealth, 0, maxHealth);
    }

    public bool IsDead { get; set; }
    public void OnSetup(UnitData data)
    {
        this.data = data;
    }
}

public class EnemyTargetSelect: System.IComparable<EnemyTargetSelect>
{
    public IUnit enemyControl;
    // lay be hon 
    public float distance;
    // lay lon hon
    public float angle;

    public int CompareTo(EnemyTargetSelect other)
    {
        if(this.distance<other.distance)
        {
            return -1;
        }
        else if(this.distance>other.distance)
        {
            return 1;
        }
        else
        {
            if (this.angle > other.angle)
            {
                return 1;
            }
            else if (this.angle < other.angle)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
  
}
