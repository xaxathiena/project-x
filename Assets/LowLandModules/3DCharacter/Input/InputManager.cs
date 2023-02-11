using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    [SerializeField]
    private JoyStickInput moveJoystick;
    public static Vector3 moveDir = Vector3.zero;
    public event Action OnFireHandle;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
    private float _x, _y;
    // Update is called once per frame
    void Update()
    {
        _x = Input.GetAxisRaw("Horizontal") + moveJoystick.moveDir.x;
        _y = Input.GetAxisRaw("Vertical") + moveJoystick.moveDir.y;
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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            OnFireHandle?.Invoke();
        }
    }
}
