using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    [SerializeField]
    private JoyStickInput moveJoystick;

    private Vector2 contextMovement;
    public static Vector3 moveDir = Vector3.zero;
    public event Action OnFireEvent;
    public event Action<int> OnSkillEvent;
    public event Action OnDashEvent;
    // [SerializeField]SimpleControls m_Controls;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        // m_Controls = new SimpleControls();
        // m_Controls.gameplay.
    }
    private float _x, _y;

    public void OnFire(InputAction.CallbackContext context)
    {
        Debug.LogWarning("OnFire - out");
        if (context.performed)
        {
            Debug.LogWarning("OnFire - in");
            OnFireEvent?.Invoke();
        }
    }
    public void OnSkillThree(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnSkillEvent?.Invoke(3);
    }
    public void OnSkillTwo(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnSkillEvent?.Invoke(2);
    }
    public void OnSkillOne(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnSkillEvent?.Invoke(1);
    }
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnDashEvent?.Invoke();
    }
    public void OnMovement(InputAction.CallbackContext context)
    {
        contextMovement.x = context.ReadValue<Vector2>().x;
        contextMovement.y = context.ReadValue<Vector2>().y;
        // _x = context.ReadValue<Vector2>().x + moveJoystick.moveDir.x;
        // if(_x>0.1f)
        // {
        //     _x = 1;
        // }
        // else if(_x<-0.1f)
        // {
        //     _x = -1;
        // }
        // _y = context.ReadValue<Vector2>().y + moveJoystick.moveDir.y;
        // if (_y > 0.1f)
        // {
        //     _y = 1;
        // }
        // else if (_y < -0.1f)
        // {
        //     _y = -1;
        // }
        // moveDir.x = _x;
        // moveDir.z = _y;
    }
    // Update is called once per frame
    #region Old version
    void Update()
    {
        _x = contextMovement.x +  moveJoystick.moveDir.x;
        _y =  contextMovement.y + moveJoystick.moveDir.y;
        if(_x>0.1f)
        {
            _x = 1;
        }
        else if(_x<-0.1f)
        {
            _x = -1;
        }
        if (_y > 0.1f)
        {
            _y = 1;
        }
        else if (_y < -0.1f)
        {
            _y = -1;
        }
        moveDir.x = _x;
        moveDir.z = _y;
    }
    

    #endregion
    
}
