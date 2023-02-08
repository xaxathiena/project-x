using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ConfigResourceRecord {
    //id name    resourceType level   capacity timeReborn  gatheringDameRequest amoutReturn requestLevel

    public int id;
    public string name;
    public ResourceType resourceType;
    public int level;
    public int capacity;
    public float timeReborn;
    public int gatheringDameRequest;
    public int amoutReturn;
    public int requestLevel;
}

public class ConfigResource : BYDataTable<ConfigResourceRecord>
{
    public override void InitComparison()
    {
        recordCompare = new ConfigPrimarykeyCompare<ConfigResourceRecord>("id");
    }
    public List<ConfigResourceRecord> AllCard
    {
        get
        {
            List<ConfigResourceRecord> result = new List<ConfigResourceRecord>(records);
            return result;
        }
    }
    public List<ConfigResourceRecord> GetResources(ResourceType type, int level)
    {
        return records.FindAll(item => item.resourceType == type && item.level == level);
    }
    public List<ConfigResourceRecord> GetResources(ResourceType type)
    {
        return records.FindAll(item => item.resourceType == type);
    }
}
public enum ResourceType
{
    WOOD = 0,
    LEATHER = 1,
    GOLD = 2
}