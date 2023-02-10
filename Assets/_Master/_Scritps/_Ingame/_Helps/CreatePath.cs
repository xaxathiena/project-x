using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CreatePath : MonoBehaviour
{
    [FormerlySerializedAs("childs")] [SerializeField] private List<Transform> children = new List<Transform>();

    private void Reset()
    {
        GetChildrem();
    }

    private void GetChildrem()
    {
        foreach (Transform child in transform)
        {
            children.Add(child);
        }
    }
    public void OnDrawGizmos()
    {
        if (children == null)
        {
            children = new List<Transform>();
        }

        if (children.Count > 1)
        {
            for (int i = 1; i < children.Count; i++)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(children[i-1].position, children[i].position);
            }
        }
    }
}
