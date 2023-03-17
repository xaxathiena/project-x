using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
public class ButtonSkillControl : MonoBehaviour
{
    private float limitTime;
    private Action onClickButtonEvent;
    public Image countDownImg;
    public TextMeshProUGUI timeDowndown;
    public GameObject lockObj;

    public void OnSetup(Action onClickCallback, float limitTime)
    {
        onClickButtonEvent = onClickCallback;
        this.limitTime = limitTime;
    }
    public int CountDownTime
    {
        set
        {
            timeDowndown.gameObject.SetActive(value > 0);
            countDownImg.gameObject.SetActive(value > 0);
            timeDowndown.text = value.ToString();
            if (value >= 0) countDownImg.fillAmount = value / limitTime;
        }
    }

    public bool IsLockSkill
    {
        set => lockObj.SetActive(value);
    }

    public void OnClick()
    {
        onClickButtonEvent?.Invoke();
    }
}
