using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 targetPosition;
    public GameObject bulletPrefab;
    public GameObject muzzlePrefab;
    public GameObject impactPrefab;
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
    public void Fire(Vector3 startPosition, Vector3 targetPosition, float speed, AttackData data)
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
        transform.position = startPosition;
        
        var dir = (targetPosition -startPosition).normalized;
        var q = Quaternion.LookRotation(dir, Vector3.up);
        transform.localRotation = q;
        realBulletParticle.transform.localRotation= Quaternion.identity;
        var distance = Vector3.Distance(targetPosition, this.startPosition);
        InGameManager.instance.StartCoroutine(IERun(distance / speed, 
            Quaternion.LookRotation((startPosition - targetPosition ).normalized)));
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
    private IEnumerator IERun(float duration, Quaternion q)
    {
        bool isDone = false;
        transform.DOMove(targetPosition, duration).SetEase(Ease.Linear).OnComplete(() =>
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
