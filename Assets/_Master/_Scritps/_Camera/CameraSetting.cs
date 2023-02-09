using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 [System.Serializable]
public class CameraState
{
    public CameraKey key;
    public Vector3 pivot_;
    public Vector3 rotation_;
    [Range(10,90)]
    public float fov = 45;
    [Range(0, 40)]
    public float fov_Zoom = 25;
    public bool isFreeRotate = false;
    public float min_X = -60;
    public float max_X = 60;
    public float min_Y = -45;
    public float max_Y = 45;
    public float speedUpdate = 6;
}
public enum CameraKey
{
    NORMAL,
    COVER_L_LEFT,
    COVER_L_RIGHT,
    COVER_H_LEFT,
    COVER_H_RIGHT,
    COVER_LOW_RIGHT_AIM ,
    COVER_H_LEFT_AIM,
    COVER_H_RIGHT_AIM,
    COVER_LOW_LEFT_AIM
}
public class CameraSetting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
