using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class BulletControl : MonoBehaviour
{
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 targetPosition;
    public GameObject bulletPrefab;
    public GameObject muzzlePrefab;
    public GameObject impactPrefab;
    public AnimationCurve curve;
    public float heightPlus;
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

    // private void OnEnable()
    // {
    //     gameObject.SetActive(true);
    //     Fire(startPosition, targetPosition, speed);
    // }
    private AttackData data;
    private bool isMove = false;
    private float speed;
    private Vector3 realPosition;
    private float originalDistance;
    public void Fire(Vector3 startPosition, Vector3 targetPosition, float speed, AttackData data)
    {
        isMove = false;
        this.data = data;
        this.startPosition = startPosition;
        this.targetPosition = targetPosition;
        this.speed = speed;
        gameObject.SetActive(true);
        if (realBulletParticle == null)
        {
            var realBullet = Instantiate(bulletPrefab, transform);
            realBulletParticle = realBullet.GetComponent<ParticleSystem>();
            var realMuzzle = Instantiate(muzzlePrefab, transform);
            realMuzzle.gameObject.SetActive(false);
            realMuzzleParticle = realMuzzle.GetComponent<ParticleSystem>();
            var realImpact = Instantiate(impactPrefab, transform);
            realImpactParticle = realImpact.GetComponent<ParticleSystem>();
            realImpact.gameObject.SetActive(false);
        }
        
        realBulletParticle.transform.localPosition= Vector3.zero;
        realMuzzleParticle.transform.localPosition= Vector3.zero;
        realImpactParticle.transform.localPosition= Vector3.zero;
        realBulletParticle.transform.localScale= Vector3.one;
        realMuzzleParticle.transform.localScale= Vector3.one;
        realImpactParticle.transform.localScale= Vector3.one;
        transform.position = startPosition;
        
        var dir = (targetPosition -startPosition).normalized;
        var q = Quaternion.LookRotation(dir, Vector3.up);
        transform.localRotation = q;
        realBulletParticle.transform.localRotation= Quaternion.identity;
        var distance = Vector3.Distance(targetPosition, this.startPosition);
        originalDistance = transform.position.DistanceSQR(targetPosition);;
        realPosition = transform.position;
        isMove = true;
    }
    public void Spawn()
    {
        this.data = data;
        this.startPosition = startPosition;
        this.targetPosition = targetPosition;
        gameObject.SetActive(true);
        if (realBulletParticle == null)
        {
            var realBullet = Instantiate(bulletPrefab, transform);
            realBulletParticle = realBullet.GetComponent<ParticleSystem>();
            var realMuzzle = Instantiate(muzzlePrefab, transform);
            realMuzzle.gameObject.SetActive(false);
            realMuzzleParticle = realMuzzle.GetComponent<ParticleSystem>();
            var realImpact = Instantiate(impactPrefab, transform);
            realImpactParticle = realImpact.GetComponent<ParticleSystem>();
            realImpact.gameObject.SetActive(false);
        }
        
        realBulletParticle.transform.localPosition= Vector3.zero;
        realMuzzleParticle.transform.localPosition= Vector3.zero;
        realImpactParticle.transform.localPosition= Vector3.zero;
        realBulletParticle.transform.localScale= Vector3.one;
        realMuzzleParticle.transform.localScale= Vector3.one;
        realImpactParticle.transform.localScale= Vector3.one;
    }

    private float distanceSQR;
    private void Update()
    {
        if (isMove)
        {
            distanceSQR = transform.position.DistanceSQR(targetPosition);
            realPosition = Vector3.Lerp(realPosition, targetPosition, Time.deltaTime * speed);
            
            transform.position = Vector3.Lerp(transform.position, 
                realPosition + Vector3.up * curve.Evaluate(distanceSQR / originalDistance), Time.deltaTime * speed);
            if (transform.position.DistanceSQR(targetPosition) < 0.01f)
            {
                var q = Quaternion.LookRotation((startPosition - targetPosition).normalized);
                //Done
                realMuzzleParticle.gameObject.SetActive(true);
                realMuzzleParticle.transform.rotation = q;
                realImpactParticle.gameObject.SetActive(true);
                realMuzzleParticle.Play();
                realImpactParticle.Play();
                this.data.unit?.ApplyDamage(this.data);
                StartCoroutine(IEWaitingImpactHide());
                isMove = false;
                // gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator IEWaitingImpactHide()
    {
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);
    }
    private IEnumerator IERun(float duration, Quaternion q)
    {
        bool isDone = false;
        float currentTime = 0f;
        transform.DOMove(targetPosition, duration).SetEase(Ease.Linear).OnUpdate(() =>
        {
            currentTime += Time.deltaTime;
            curve.Evaluate(currentTime / duration);
        }).OnComplete(() =>
        {
            realMuzzleParticle.gameObject.SetActive(true);
            realMuzzleParticle.transform.rotation = q;
            realImpactParticle.gameObject.SetActive(true);
            realMuzzleParticle.Play();
            realImpactParticle.Play();
            isDone = true;
            this.data.unit?.ApplyDamage(this.data);
        }); 
        yield return new WaitUntil(() => isDone);
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
