using StateMachine;
using UnityEngine;

public class SnakeletAttackState : IState
{
        public SnakeletControl parent;

        private float currentTime;
        private float rof = 0.3f;
        public bool canTransitionToSelf { get; }
        public void Initialize(FSMSystem parent, params object[] datas)
        {
            currentTime = 0f;
        }

        public void OnEnter(params object[] data)
        {
            currentTime = 0f;
            parent.dataBinding.Speed = 0;
            parent.dataBinding.Attack = true;
            parent.agent.enabled = false;
            Debug.Log("Obstackle enableed = " + true);
            parent.obsTackle.enabled = true;
            var bullet = PoolManager.instance.GetPool<BulletControl>(parent.bulletName);
            bullet.Fire(parent.fireStartPivot.position, parent.CurrentTarget.position, parent.bulletSpeed, new AttackData()
            {
                damage = 10,
                unit = parent.CurrentTarget
            });
        }

        public void OnEnterFromSameState(params object[] data)
        {
        }

        public void OnUpdate()
        {
            currentTime += Time.deltaTime;
            
            if (parent.CurrentTarget == null)
            {
                parent.GotoIdle();
            }
            else
            {
                var isInside = parent.position.IsPositionInRange(parent.CurrentTarget.position, parent.attackRange,
                    parent.CurrentTarget.boderRange);
                if (currentTime > parent.rof)
                {
                    if (isInside)
                    {
                        var dir = (parent.CurrentTarget.position - this.parent.transform.position).normalized;
                        var q = Quaternion.LookRotation(dir, Vector3.up);
                        this.parent.transform.localRotation = Quaternion.Lerp(this.parent.transform.localRotation,q, Time.deltaTime*10);
                        currentTime = 0;
                        this.parent.dataBinding.Speed = 0f;
                        parent.dataBinding.Attack = true;
                        var bullet = PoolManager.instance.GetPool<BulletControl>(parent.bulletName);
                        bullet.Fire(parent.fireStartPivot.position, parent.CurrentTarget.position, parent.bulletSpeed, new AttackData()
                        {
                            damage = 10,
                            unit = parent.CurrentTarget
                        });
                    }
                    else
                    {
                        parent.GotoIdle();
                    }
                }
            }
        }

        public void OnLateUpdate()
        {
        }

        public void OnFixedUpdate()
        {
            if (parent.CurrentTarget != null)
            {
                var dir = parent.CurrentTarget.position - parent.position; 
                var q = Quaternion.LookRotation(dir.normalized, Vector3.up);
                this.parent.transform.localRotation =
                    Quaternion.Lerp(this.parent.transform.localRotation, q, Time.deltaTime * 10);
            }
        }

        public void OnExit()
        {
            Debug.Log("Exit spawn");
        }

        public void Dispose()
        {
        }

        public void OnDrawGizmos()
        {
            
        }
}