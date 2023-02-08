using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ConfigHunterRecord
{
    //id	name	level	resourceType	dame	rangeAttack	moveSpeed	rof	effectId	goldBuy	leatherBuy	woodBuy	goldUpgrade	unitUpgradeId
    public int id;
    public string name;
    public int level;
    public ResourceType resourceType;
    public float dame;
    public float rangeAttack = 4;
    public float moveSpeed;
    public float rof;
    public int effectId;
    public int goldBuy;
    public int leatherBuy;
    public int woodBuy;
    public int woodUpgrade;
    public int unitUpgradeId;
}
public class ConfigHunter : BYDataTable<ConfigHunterRecord>
{
    public override void InitComparison()
    {
        recordCompare = new ConfigPrimarykeyCompare<ConfigHunterRecord>("id");
    }
    public List<ConfigHunterRecord> AllCard
    {
        get
        {
            List<ConfigHunterRecord> result = new List<ConfigHunterRecord>(records);
            return result;
        }
    }
    public List<ConfigHunterRecord> GetHunters(ResourceType type, int level)
    {
        return records.FindAll(item => item.resourceType == type && item.level == level);
    }
    public List<ConfigHunterRecord> GetHunters(ResourceType type)
    {
        return records.FindAll(item => item.resourceType == type);
    }
    public List<ConfigHunterRecord> GetHunters(int level)
    {
        return records.FindAll(item => item.level == level);
    }
}