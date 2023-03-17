using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]
public class SkillDefine
{
    public UnityEvent onClick;
    public float limitTime;
    public ButtonSkillControl control;
    public int levelRequire;
    [HideInInspector] public float currentTime;

    public void Start()
    {
        currentTime = limitTime;
        control.OnSetup(() =>
        {
            currentTime = limitTime;
            onClick?.Invoke();
        }, limitTime);
        DataTrigger.RegisterValueChange(DataPath.INGAME_PLAYER_LEVEL, (obj) =>
        {
            int playerLevel = (int)obj;
            control.IsLockSkill = levelRequire > playerLevel;
        });
        control.IsLockSkill = levelRequire > 1;
    }
    public void OnUpdateUI()
    {
        currentTime -= Time.deltaTime;
        control.CountDownTime = (int)currentTime;
    }
}
public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    private float _x, _y;
    private Vector2 contextMovement;
    [SerializeField] private SkillDefine[] skills;
    [SerializeField] private JoyStickInput moveJoystick;
    
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

    private void Start()
    {
        foreach (var skill in skills)
        {
            skill.Start();
        }
    }
    
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
        foreach (var skill in skills)
        {
            skill.OnUpdateUI();
        }
    }
    
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnFireAction();
        }
    }
    public void OnSkillThree(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnSkillAction(3);
    }

    
    public void OnSkillTwo(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnSkillAction(2);
    }
    public void OnSkillOne(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnSkillAction(1);
    }
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnDashEventAction();
    }

    public void OnFireAction()
    {
        OnFireEvent?.Invoke();
    }
    public void OnSkillAction(int index)
    {
        OnSkillEvent?.Invoke(index);
    }
    public void OnDashEventAction()
    {
        OnDashEvent?.Invoke();
    }
    public void OnMovement(InputAction.CallbackContext context)
    {
        contextMovement.x = context.ReadValue<Vector2>().x;
        contextMovement.y = context.ReadValue<Vector2>().y;
    }
    
}
