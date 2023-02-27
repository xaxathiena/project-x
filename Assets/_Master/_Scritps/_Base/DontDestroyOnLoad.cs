using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private static DontDestroyOnLoad _instance;
    public static DontDestroyOnLoad Instance => _instance;
    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);
    }   
}
