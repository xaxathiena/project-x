using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectMoveAround : MonoBehaviour
{
   public Transform target;
   public Transform objectFollow;
   public float startAngle;
   public float distance;
   public float speedMove;
   public float speedFollow;
   public float rangAttack = 5;
   private float currentAngle;
   private List<IUnit> enemies = new List<IUnit>();
   private bool isAttacking = false;
   private float rof =.7f;
   private float currentFireTime = 0;
   private float timeAttack = 0.3f;
   private AttackData dataAttack;

   private void OnEnable()
   {
      objectFollow.position = transform.position;
   }

   private void Start()
   {
      currentAngle = startAngle;
      currentFireTime = rof;
      dataAttack = new AttackData()
      {
         damage = 1000
      };
   }

   public void OnMove()
   {
      var pos =target.position +  Quaternion.Euler(new Vector3(0, currentAngle, 0)) * (Vector3.one * distance);
      pos.y = target.position.y;
      transform.position = pos;
      if (!isAttacking && currentFireTime >= rof)
      {
         enemies.Clear();
         UnitsManager.instance.GetUnitInRange(ref enemies, transform.position, rangAttack, UnitSide.Ally);
         if (enemies.Count > 0)
         {
            currentFireTime = 0f;
            isAttacking = true;
            objectFollow.DOMove(enemies[0].position, timeAttack).OnComplete(() =>
            {
               enemies[0].ApplyDamage(dataAttack);
               isAttacking = false;
            });

         }
         else
         {
            objectFollow.position = Vector3.Lerp(objectFollow.position,pos, Time.deltaTime * speedFollow);
         }
      }
   }

   private void Update()
   {
      currentAngle += Time.deltaTime * speedMove;
      currentAngle = currentAngle >= 360f? 0: currentAngle;
      currentFireTime += Time.deltaTime;
      OnMove();
   }
}
