using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Linq;

[Serializable]
public class WavesRecord 
{
    // [JsonProperty, SerializeField, JsonConverter(typeof(JsonConverterList<CardType>))]
    //id	idUnits 
    [JsonProperty, SerializeField] private int id;
    
    [JsonProperty, SerializeField, JsonConverter(typeof(JsonConverterList<int>))]
    private List<int> idUnits;

    public int ID => id;
    public List<int> IDUnits => idUnits;
}

public class JsonConverterList<T>: JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var data = (List<int>)value;
        string rs = "";
        for (int i = 0; i < data.Count; i++)
        {
            rs += data[i];
            if (i < data.Count - 1)
            {
                rs += ",";
            }
        }
        writer.WriteValue(rs);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        Debug.Log((string)reader.Value);
        var listData = ((string)reader.Value).Split(',');
        var returnValue = new List<T>();
        for (int i = 0; i < listData.Length; i++)
        {
            if (string.IsNullOrEmpty(listData[i]))
                continue;
            try
            {
                returnValue.Add((T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(listData[i]));
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return returnValue;
            }
        }
        return returnValue;
    }

    public override bool CanConvert(Type objectType)
    {
        return true;
    }
}


public class ConfigWaves : BYDataTable<WavesRecord>
{
    public override void InitComparison()
    {
        recordCompare = new ConfigPrimarykeyCompare<WavesRecord>("id");
    }
}
