using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class ConfigUnitRecord {
    //id,name,level,hp,dame,rangeAttack,rangeSwitchAttack,moveSpeed,rof,healthRegen,effectId,goldBuy,leatherbBuy,woodBuy,goldUpgrade,unitUpgradeId
    public int id;
    public string name;
    public int level;
    public int hp;
    public float dame;
    public float rangeAttack = 4;
    public float rangeSwitchAttack = 10;
    public float moveSpeed;
    public float rof;
    public float healthRegen;
    public int effectId;
    public int goldBuy;
    public int leatherBuy;
    public int woodBuy;
    public int woodUpgrade;
    public int unitUpgradeId;
}
public class ConfigUnits : BYDataTable<ConfigUnitRecord>
{
    public override void InitComparison()
    {
        recordCompare = new ConfigPrimarykeyCompare<ConfigUnitRecord>("id");
    }
    public List<ConfigUnitRecord> AllCard
    {
        get
        {
            List<ConfigUnitRecord> result = new List<ConfigUnitRecord>(records);
            return result;
        }

    }
    public List<ConfigUnitRecord> GetCardLevel1() {
        var result = records.FindAll(item => item.level == 1);
        return new List<ConfigUnitRecord>(result);
    }
    public List<ConfigUnitRecord> GetCardsLevel(int level)
    {
        var result = records.FindAll(item => item.level == level);
        return new List<ConfigUnitRecord>(result);
    }
}
