using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class UnitRecord
{
    [JsonProperty, SerializeField] private int id;
    [JsonProperty, SerializeField] private string name;
    [JsonProperty, SerializeField] private UnitType type;
    [JsonProperty, SerializeField] private float dame;
    [JsonProperty, SerializeField] private float rof;
    [JsonProperty, SerializeField] private float health;
    [JsonProperty, SerializeField] private float detectRange;
    [JsonProperty, SerializeField] private float attackRange;
    [JsonProperty, SerializeField] private float speedMove;
    [JsonProperty, SerializeField] private float boderRange;
    [JsonProperty, SerializeField] private string bulletName;
    [JsonProperty, SerializeField] private float bulletSpeed;
    
    public int ID => id;
    public string Name => name;
    public UnitType Type => type;
    public float Dame => dame;
    public float Rof => rof;
    public float Health => health;
    public float DetectRange => detectRange;
    public float AttackRange => attackRange;
    public float SpeedMove => speedMove;
    public string BulletName => bulletName;
    public float BulletSpeed => bulletSpeed;
    public float BoderRange => boderRange;
}
public class ConfigUnit : BYDataTable<UnitRecord>
{
    public override void InitComparison()
    {
        recordCompare = new ConfigPrimarykeyCompare<UnitRecord>("id");
    }
}

public enum UnitType
{
    mele,
    ranger
}