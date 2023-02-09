using System.Collections.Generic;
using UnityEngine;

public class ThirdCameraControl : MonoBehaviour
{
    public Camera cam_;
    public Transform target;
    public Transform aimTarget;
    private Transform trans;
    private float horizontal, vertical;
    public float Horizontal
    {
        get
        {
            return horizontal;
        }
    }
    [Range(0, 2f)]
    public float speedHorizontal = 0.9f;
    [Range(0, 2f)]
    public float speedVertical = 0.9f;
    [Range(0, 50f)]
    public float rangeAim = 25f;
    public List<CameraState> cameraStates;
    private Dictionary<CameraKey, CameraState> dicCamState = new Dictionary<CameraKey, CameraState>();
    private CameraState currentState_;
    public CameraKey currentKeyState;
    private Vector3 pivot_, rotateOffset_;
    private float fov_;
    private Vector3 motorPosition;
    private Quaternion motorRotation;
    // Start is called before the first frame update
    void Start()
    {

        trans = transform;
        foreach (CameraState e in cameraStates)
        {
            dicCamState.Add(e.key, e);
        }
        currentState_ = dicCamState[currentKeyState];
        motorRotation = target.localRotation;
        motorPosition = target.position;
    }

    private void LateUpdate()
    {
        Vector3 deltaMouse = InputManager.deltaMouse;
        horizontal = Mathf.Lerp(horizontal, horizontal + deltaMouse.x * speedHorizontal, Time.deltaTime * 120);
        vertical = Mathf.Lerp(vertical, vertical - deltaMouse.y * speedVertical + InputManager.cameraDamping, Time.deltaTime * 120);
        if (!currentState_.isFreeRotate)
        {
            horizontal = Mathf.Clamp(horizontal, currentState_.min_X, currentState_.max_X);
            vertical = Mathf.Clamp(vertical, currentState_.min_Y, currentState_.max_Y);
        }

        Quaternion qRotation = Quaternion.Euler(vertical, horizontal, 0);
        //
        Vector3 posTarget = target.position;
        motorPosition = Vector3.Lerp(motorPosition, posTarget, Time.deltaTime * 10);
        motorRotation = Quaternion.Slerp(motorRotation, qRotation, Time.deltaTime * 10);
        // add rotation target

        //
        Vector3 newTransPivot = motorRotation * pivot_ + motorPosition;

        trans.position = newTransPivot;
        Vector3 posAimTarget = motorPosition + Vector3.up * 1.5f + motorRotation * Vector3.forward * rangeAim;

        aimTarget.position = posAimTarget;
        pivot_ = Vector3.Lerp(pivot_, currentState_.pivot_, Time.deltaTime * currentState_.speedUpdate);
        rotateOffset_ = Vector3.Lerp(rotateOffset_, currentState_.rotation_, Time.deltaTime * currentState_.speedUpdate);
        if (InputManager.isZoom)
        {
            fov_ = Mathf.Lerp(fov_, currentState_.fov_Zoom, Time.deltaTime * currentState_.speedUpdate*2);
        }
        else
            fov_ = Mathf.Lerp(fov_, currentState_.fov, Time.deltaTime * currentState_.speedUpdate);
        cam_.fieldOfView = fov_;
        cam_.transform.localEulerAngles = rotateOffset_;
        trans.LookAt(posAimTarget);
        // @Tai: tham khao
        //Vector3 dir = posAimTarget - trans.position;
        //trans.forward = dir;
    }
    // Update is called once per frame
    // public void ChangeCameraState(PlayerMode playeMode, CoverType coverType, float sideMove, bool aim)
    // {
    //     CameraKey key = CameraKey.NORMAL;
    //     if (playeMode == PlayerMode.COVER)
    //     {
    //         if (coverType == CoverType.COVER_H)
    //         {
    //             if (sideMove > 0)
    //             {
    //                 if (!aim)
    //                     key = CameraKey.COVER_H_RIGHT;
    //                 else
    //                     key = CameraKey.COVER_H_RIGHT_AIM;
    //             }
    //             else
    //             {
    //                 if (!aim)
    //                     key = CameraKey.COVER_H_LEFT;
    //                 else
    //                     key = CameraKey.COVER_H_LEFT_AIM;
    //             }
    //         }
    //         else
    //         {
    //             if (sideMove > 0)
    //             {
    //                 if (!aim)
    //                     key = CameraKey.COVER_L_RIGHT;
    //                 else
    //                     key = CameraKey.COVER_LOW_RIGHT_AIM;
    //             }
    //             else
    //             {
    //                 if (!aim)
    //                     key = CameraKey.COVER_L_LEFT;
    //                 else
    //                     key = CameraKey.COVER_LOW_LEFT_AIM;
    //             }
    //         }
    //     }
    //
    //     currentKeyState = key;
    //     currentState_ = dicCamState[key];
    // }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            currentState_ = dicCamState[CameraKey.NORMAL];
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            currentState_ = dicCamState[CameraKey.COVER_L_LEFT];
        }
    }
}
