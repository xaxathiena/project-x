using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class ConfigSystemUnitRecord
{
    //id	name	resourceType	level

    public int id;
    public string name;
    public ResourceType resourceType;
    public int level;
}
public class ConfigSystemUnit : BYDataTable<ConfigSystemUnitRecord>
{
    public override void InitComparison()
    {
        recordCompare = new ConfigPrimarykeyCompare<ConfigSystemUnitRecord>("id");
    }
    public List<ConfigSystemUnitRecord> AllCard
    {
        get
        {
            List<ConfigSystemUnitRecord> result = new List<ConfigSystemUnitRecord>(records);
            return result;
        }

    }
    public List<ConfigSystemUnitRecord> GetSystemUnits(ResourceType type, int level)
    {
        var result = records.FindAll(item => item.level == level && item.resourceType == type);
        return new List<ConfigSystemUnitRecord>(result);
    }
    public List<ConfigSystemUnitRecord> GetCardLevel1()
    {
        var result = records.FindAll(item => item.level == 1);
        return new List<ConfigSystemUnitRecord>(result);
    }
    public List<ConfigSystemUnitRecord> GetCardsLevel(int level)
    {
        var result = records.FindAll(item => item.level == level);
        return new List<ConfigSystemUnitRecord>(result);
    }
}
