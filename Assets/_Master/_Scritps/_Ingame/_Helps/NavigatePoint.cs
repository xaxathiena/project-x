using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NavigatePoint : MonoBehaviour
{
    [SerializeField] private LayerMask mask;
    [SerializeField] private Transform direction;
    public void OnTriggerEnter(Collider other)
    {
        //if (( mask & (1 << layer)) != 0)
        if (mask == (mask | (1 << other.gameObject.layer)))
        {
           var iunit= other.gameObject.GetComponent<IUnit>();
            Debug.Log("ontrigger enter " + other.gameObject.name);
           iunit.Dir = direction.forward;
        }
    }

    public void OnDrawGizmos()
    {
        if (direction != null)
        {
            Handles.color = Color.white;
            Handles.DrawLine(direction.position, direction.position+  direction.forward * 2);
        }
    }

    #region Mannual physic
/*

    private bool CheckInSideArea()
    {
        Bounds b = new Bounds(transform.position, size);
        var position = checkPoint.position - transform.position;
        return b.Contains(new Vector3(transform.forward * position.z, 0, transform.right * position.x));
        if (checkPoint == null) return false;
        Vector3 pointRight = transform.position +  transform.right * size.x / 2;
        Vector3 pointLeft = transform.position  - transform.right * size.x / 2;
        Vector3 pointUp = transform.position +  transform.forward * size.z / 2;
        Vector3 pointDown = transform.position - transform.forward * size.z / 2;

        if (checkPoint.position.z > pointRight.z 
            && checkPoint.position.z < pointLeft.z
            && checkPoint.position.x > pointDown.x
            && checkPoint.position.x < pointUp.x 
           )
        {
            return true;
        }
        
        
        return false;
    }

    public void OnDrawGizmos()
    {
        
        #region Draw point
        //draw point
        Handles.color = Color.blue;
        Vector3 pointRight = transform.position +  transform.right * size.x / 2;
        Vector3 pointLeft = transform.position  - transform.right * size.x / 2;
        Vector3 pointUp = transform.position +  transform.forward * size.z / 2;
        Vector3 pointDown = transform.position - transform.forward * size.z / 2;
        
        
        Handles.DrawWireCube(pointRight, Vector3.one * 0.1f);
        Handles.DrawLine(Vector3.zero, transform.right);
        Handles.color = Color.green;
        Handles.DrawWireCube(pointLeft, Vector3.one * 0.1f);
        
        Handles.color = Color.yellow;
        Handles.DrawWireCube(pointUp, Vector3.one * 0.1f);
        Handles.DrawLine(Vector3.zero, transform.up);
        
        Handles.color = Color.black;
        Handles.DrawWireCube(pointDown, Vector3.one * 0.1f);
        Handles.DrawLine(Vector3.zero, transform.forward);
        #endregion

        
        var currentMatrix = Handles.matrix; 
        Handles.matrix = transform.localToWorldMatrix;
        if(!CheckInSideArea())
        {
            Handles.color = Color.white;
        }
        else
        {
            Handles.color = Color.red;
        }
        Handles.DrawWireCube(Vector3.zero, size);
        Handles.color = Color.white;
        if(direction != null)
            Handles.DrawLine(Vector3.zero, direction.forward.normalized * 3f);

        Handles.matrix = currentMatrix;
    }
    
*/
    #endregion
    
}
