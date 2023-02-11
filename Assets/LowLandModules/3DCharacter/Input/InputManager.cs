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
    public static Vector3 moveDir = Vector3.zero;
    public event Action OnFireHandle;
    [SerializeField]SimpleControls m_Controls;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        m_Controls = new SimpleControls();
        // m_Controls.gameplay.
    }
    private float _x, _y;

    public void OnFire(InputAction.CallbackContext context)
    {
        OnFireHandle?.Invoke();
    }
    public void OnMovement(InputAction.CallbackContext context)
    {
        _x = context.ReadValue<Vector2>().x;
        if(_x>0.1f)
        {
            _x = 1;
        }
        else if(_x<-0.1f)
        {
            _x = -1;
        }
        _y = context.ReadValue<Vector2>().y;
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
        Debug.Log(moveDir);
    }
    // Update is called once per frame
    #region Old version
    // void Update()
    // {
    //     _x = Input.GetAxisRaw("Horizontal") + moveJoystick.moveDir.x;
    //     _y = Input.GetAxisRaw("Vertical") + moveJoystick.moveDir.y;
    //     if(_x>0.1f)
    //     {
    //         _x = 1;
    //     }
    //     else if(_x<-0.1f)
    //     {
    //         _x = -1;
    //     }
    //     if (_y > 0.1f)
    //     {
    //         _y = 1;
    //     }
    //     else if (_y < -0.1f)
    //     {
    //         _y = -1;
    //     }
    //     moveDir.x = _x;
    //     moveDir.z = _y;
    //     if(Input.GetKeyDown(KeyCode.Space))
    //     {
    //         OnFireHandle?.Invoke();
    //     }
    // }
    

    #endregion
    
}
