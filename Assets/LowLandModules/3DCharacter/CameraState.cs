using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraStateType
{
    NORMAL=1,
    SKILL=2

}
[System.Serializable]
public class CameraState 
{
    public CameraStateType stateType=CameraStateType.NORMAL;
    public Vector3 offset;
    public Vector3 angle;
    [Range(10,100)]
    public float fov = 60;
    public float speedFlow = 5f;
}
