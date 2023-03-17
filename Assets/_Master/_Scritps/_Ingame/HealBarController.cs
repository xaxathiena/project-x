using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealBarController : MonoBehaviour
{
    [SerializeField] private Image realHealthImg;
    [SerializeField] private Image delayHealthImg;
    // Start is called before the first frame update
    [SerializeField] private float maxTimeDelay = 0.5f;
    [SerializeField] private float speedFill = 10f;
    [SerializeField] private bool isAlwayShow = false;
    private float currentTimeDelay = 0f;
    private bool isStartDelay = false;
    private float currentRatio;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void InitHealBar()
    {
        
        realHealthImg.fillAmount = 1f;
        delayHealthImg.fillAmount = 1f;
        if (isAlwayShow)
        {
            gameObject.SetActive(true);
            //UnityLifeCircle.instance.RegisterUnityEvent(UnityEventType.Update, OnDeplay);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    public void SetupHealth(float currentHealth, float minHealth, float maxHealth)
    {
        currentRatio = currentHealth / maxHealth;
        realHealthImg.fillAmount = currentRatio;
        currentTimeDelay = 0f;
        if (!isStartDelay)
        {
            isStartDelay = true;
            // if (!isAlwayShow)
            // {
            //     gameObject.SetActive(true);
            //     UnityLifeCircle.instance.RegisterUnityEvent(UnityEventType.Update, OnDeplay);
            // }
        }
    }

    private void Update()
    {
        currentTimeDelay += Time.deltaTime;
        if (currentTimeDelay > maxTimeDelay)
        {
            delayHealthImg.fillAmount = Mathf.Lerp(delayHealthImg.fillAmount, currentRatio, Time.deltaTime * speedFill);
            if (Mathf.Approximately(delayHealthImg.fillAmount ,currentRatio))
            {
                isStartDelay = false;
                // if (!isAlwayShow)
                // {
                //     gameObject.SetActive(false);
                //     UnityLifeCircle.instance.UnRegisterUnityEvent(UnityEventType.Update, OnDeplay);
                // }
            }
        }
        if(transform !=null)
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.back,mainCamera.transform.rotation * Vector3.down);
    }
}
