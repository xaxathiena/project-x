using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public enum GunType
{
    HandGun = 1,
    AutoGun = 2,
    Shotgun = 3,
    HeavyGun = 4
}

[Serializable]
public class ConfigGunRecord
{
    //id    name    icon    gunType cost
    public int id;
    public string name;
    public string icon;
    public GunType gunType;  
    public int cost;
}

public class ConfigGun : BYDataTable<ConfigGunRecord>
{
    public override void InitComparison()
    {
        recordCompare = new ConfigPrimarykeyCompare<ConfigGunRecord>("id");
    }
    public List<int> GetAllID()
    {
        int[] ids = records
                     .Select(i => i.id)
                     .ToArray();

        List<int> ls = new List<int>();
        ls.AddRange(ids);
        return ls;
    }
}
