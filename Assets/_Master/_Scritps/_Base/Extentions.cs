using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public static class Extentions
{
    public static string GetValueByKey(this string json, string key)
    {
        if(key.Contains("/")) {
            return json.GetValueByPath(key);
        }
        string value = string.Empty;
        try
        {
            var jsonTemp = JObject.Parse(json);

            if (jsonTemp[key] != null)
            {
                try
                {
                    value = (string)jsonTemp[key];
                }
                catch (System.Exception ex)
                {
                    value = jsonTemp[key].ToString();
                }
            }
        }
        catch (System.Exception)
        {
            value = string.Empty;
        }

        return value;
    }
    public static string GetValueByPath(this string json, string path)
    {
        try
        {
            var keyJson = path.Split('/');
            string value = json;
            JObject jsonTemp;
            for (int i = 0; i < keyJson.Length; i++)
            {
                jsonTemp = JObject.Parse(value);
                if (jsonTemp[keyJson[i]] != null)
                {
                    try
                    {
                        value = (string)jsonTemp[keyJson[i]];
                    }
                    catch (System.Exception ex)
                    {
                        value = jsonTemp[keyJson[i]].ToString();
                    }
                }
                else
                {
                    Debug.LogError("Error at key " + keyJson[i]);
                    return string.Empty;
                }
            }
            return value;
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Ex: " + ex.Message);
            return string.Empty;
        }

    }
}
