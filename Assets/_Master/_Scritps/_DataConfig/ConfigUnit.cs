using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
public class UnitRecord
{
    [JsonProperty, SerializeField] private int id;
    [JsonProperty, SerializeField] private string name;
    [JsonProperty, SerializeField] private float dame;
    [JsonProperty, SerializeField] private float dps;
    [JsonProperty, SerializeField] private float health;
    [JsonProperty, SerializeField] private float detectRange;
    [JsonProperty, SerializeField] private float attackRange;
    
    public int ID => id;
    public string Name => name;
    public float Dame => dame;
    public float DPS => dps;
    public float Health => health;
    public float DetectRange => detectRange;
    public float AttackRange => attackRange;
}
public class ConfigUnit : BYDataTable<UnitRecord>
{

    public override void InitComparison()
    {
        recordCompare = new ConfigPrimarykeyCompare<UnitRecord>("id");
    }
}
