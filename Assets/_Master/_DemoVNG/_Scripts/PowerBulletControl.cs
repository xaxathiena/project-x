using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PowerBulletControl : MonoBehaviour
{
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float timeAlive = 5f;
    [SerializeField] private LayerMask collisionExplodeMask;
    /// <summary>
    /// Bullet
    /// </summary>
    private ParticleSystem realBulletParticle;
    /// <summary>
    /// Opposite of forward direction
    /// </summary>
    private ParticleSystem realMuzzleParticle;
    /// <summary>
    /// Explosion
    /// </summary>
    private ParticleSystem realImpactParticle;
    private AttackData data;
    private bool isMove = false;
    private float speed;
    private Vector3 realPosition;
    private float originalDistance;
    private BulletTriggerControl bulletTriggerControl;
    public float currentAliveTime;
    
    public GameObject bulletPrefab;
    public GameObject muzzlePrefab;
    public GameObject impactPrefab;
    public AnimationCurve curve;
    public float heightPlus;
    public void Fire(Vector3 startPosition, Vector3 direction, float timeAlive, float speed, AttackData data)
    {
        this.currentAliveTime = 0;
        this.isMove = false;
        this.data = data;
        this.startPosition = startPosition;
        this.direction = direction;
        this.timeAlive = timeAlive;
        
        
        this.speed = speed;
        gameObject.SetActive(true);
        if (realBulletParticle == null)
        {
            var realBullet = Instantiate(bulletPrefab, transform);
            realBulletParticle = realBullet.GetComponent<ParticleSystem>();
            bulletTriggerControl = realBullet.GetComponent<BulletTriggerControl>();
            bulletTriggerControl.OnTriggerEnterEvent = OnTriggerEnterHandle;
            if (muzzlePrefab != null)
            {
                var realMuzzle = Instantiate(muzzlePrefab, transform);
                realMuzzleParticle = realMuzzle.GetComponent<ParticleSystem>();
                realMuzzleParticle.transform.localPosition= Vector3.zero;
                realMuzzleParticle.transform.localScale= Vector3.one;
                realMuzzle.gameObject.SetActive(false);
            }

            if (impactPrefab != null)
            {
                var realImpact = Instantiate(impactPrefab, transform);
                realImpactParticle = realImpact.GetComponent<ParticleSystem>();
                realImpactParticle.transform.localPosition= Vector3.zero;
                realImpactParticle.transform.localScale= Vector3.one;
                realImpact.gameObject.SetActive(false);
            }
        }
        
        realBulletParticle.transform.localPosition= Vector3.zero;
        
        transform.position = startPosition;
        
        var dir =this.direction;
        var q = Quaternion.LookRotation(dir, Vector3.up);
        transform.localRotation = q;
        realBulletParticle.transform.localRotation= Quaternion.identity;
        realPosition = transform.position;
        isMove = true;
    }
    private float distanceSQR;
    private void Update()
    {
        if (isMove)
        {
            currentAliveTime += Time.deltaTime;
            if (currentAliveTime >= this.timeAlive)
            {
                isMove = false;
                gameObject.SetActive(false);
            }

            realPosition += direction * Time.deltaTime * speed;
            transform.position = Vector3.Lerp(transform.position, realPosition , Time.deltaTime * speed);
        }
    }

    private void OnTriggerEnterHandle(Collider other)
    {
        Debug.Log("Trigger" + other.name);
        if(((1 << other.gameObject.layer) & data.mask) != 0)
        {
            other.gameObject.GetComponent<IUnit>()?.ApplyDamage(this.data);
        }
    }

    private IEnumerator IEWaitingImpactHide()
    {
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);
    }

    public void OnDrawGizmos()
    {
        // Gizmos.color = Color.red;
        // Gizmos.DrawSphere(transform.position, 1f);
        // Gizmos.DrawSphere(targetPosition, 1f);
    }
}
