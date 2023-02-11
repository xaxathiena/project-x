using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class JoyStickInput : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public RectTransform borderRect;
    public RectTransform knod;
    public float distanceKnod = 229f;//chieu rong cua Joystick
    public Vector2 moveDir = Vector2.zero;
    private float radius =0f;
    // Start is called before the first frame update
    void Start()
    {
        radius = borderRect.sizeDelta.x / 2;//Ban kinh cua Joystick
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        moveDir = Vector2.zero;
    }
    private float _y = 0f, _x = 0f;
    public void OnDrag(PointerEventData eventData)
    {
        moveDir = Vector2.zero;
        Vector2 pos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(borderRect, eventData.position, eventData.pressEventCamera, out pos);

        _x = pos.x / radius;
        _x = Mathf.Clamp(_x, -1f, 1f);

        _y = pos.y / radius;

        _y = Mathf.Clamp(_y, -1f, 1f);
        moveDir.x = _x;
        moveDir.y = _y;
        knod.anchoredPosition = new Vector2(moveDir.x * radius, moveDir.y * radius);

        float cur_Move_Knod = knod.anchoredPosition.magnitude;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        moveDir = Vector2.zero;
    }

    

    // Update is called once per frame
    void Update()
    {
        if(moveDir.magnitude <=0)
            knod.anchoredPosition = new Vector2(InputManager.moveDir.normalized.x * radius, InputManager.moveDir.normalized.z * radius);
    }
}
