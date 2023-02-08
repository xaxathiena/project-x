using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class SpawnTowerRecord
{
    //id	name	idWaves	attackRange	dame	dps	heath
    [JsonProperty, SerializeField] private int id;
    [JsonProperty, SerializeField] private string name;
    [JsonProperty, SerializeField, JsonConverter(typeof(JsonConverterList<int>))]
    private List<int> idWaves;
    [JsonProperty, SerializeField] private float attackRange;
    [JsonProperty, SerializeField] private float dame;
    [JsonProperty, SerializeField] private float dps;
    [JsonProperty, SerializeField] private float heath;
    
    public int Id => id;
    public string Name => name;
    public List<int> IdWaves => idWaves;
    public float AttackRange => attackRange;
    public float Dame => dame;
    public float DPS => dps;
    public float Heath => heath;
}

public class ConfigSpawnTower : BYDataTable<SpawnTowerRecord>
{
    public override void InitComparison()
    {
        recordCompare = new ConfigPrimarykeyCompare<SpawnTowerRecord>("id");
    }
}
