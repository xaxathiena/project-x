using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ObjectPoolBaseControl : ObjectPoolControl
{
    private GameObject mainObj;
    [SerializeField]
    private float duration = 1.5f;

    private DataObjectPool data;
    public override void OnPlay(DataObjectPool data)
    {
        this.data = data;
        if (mainObj == null)
        {
            mainObj = Instantiate(prefab, transform);
            mainObj.transform.localPosition = Vector3.zero;
            mainObj.transform.localRotation = Quaternion.identity;
        }

        transform.position = this.data.position;
        transform.rotation = this.data.rotation;
        Observable.Timer(TimeSpan.FromSeconds(this.data.timeLife)).Subscribe(_ =>
        {
            gameObject.SetActive(false);
        });
    }
}