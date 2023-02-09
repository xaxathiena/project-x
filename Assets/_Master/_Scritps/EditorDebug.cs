using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorDebug : MonoBehaviour
{
    public Vector3 size;
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(this.transform.position - Vector3.down * (size.y/2), size);
    }
}
