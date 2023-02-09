using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public static Vector3 moveDir;
    public static Vector3 deltaMouse;
    private Vector3 originPosMouse;
    public float sensitive = 0.2f;
    public static event Action<int> onChangeGun;
    public static event Action<bool> OnFire;
    public static event Action OnZoom;
    public static event Action OnReload;
    public static float cameraDamping = 0;
    public static bool isReload = false;
    public static bool isZoom = false;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    public void OnFireAction(bool isFire)
    {
        if (!isReload)
            OnFire?.Invoke(isFire);
    }
    public void OnChangeGunAction(int index)
    {
        onChangeGun?.Invoke(index);
    }
    // Update is called once per frame
    void Update()
    {
        moveDir.x += Input.GetAxisRaw("Horizontal");
        moveDir.z+= Input.GetAxisRaw("Vertical");
        deltaMouse = Vector3.zero;

        if (!IsOverUIObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                originPosMouse = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                deltaMouse = Input.mousePosition - originPosMouse;
                originPosMouse = Input.mousePosition;

            }
            else if (Input.GetMouseButtonUp(0))
            {
                originPosMouse = Vector3.zero;
            }
            deltaMouse *= sensitive;
        }
       
        // 
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (!isReload)
                OnFire?.Invoke(true);
        }
      
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            OnFire?.Invoke(false);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(!isReload)
                OnReload?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
          
                OnZoom?.Invoke();
        }
    }
    private bool IsOverUIObject()
    {
        if(EventSystem.current!=null)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> resutls = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, resutls);
            return resutls.Count > 0;
        }
        return false;
    }
}
