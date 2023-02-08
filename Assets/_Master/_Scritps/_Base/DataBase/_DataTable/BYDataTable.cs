using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

// make file 
public class BYDataTableOgrin: ScriptableObject
{
    public virtual void ImportDataUsingNewtonJson(string textData)
    {

    }
    public virtual void ImportDataUsingJsonUtility(string textData)
    {

    }
    public virtual string GetCSVData()
    {
        return string.Empty;
    }
}
public abstract class ConfigCompare<T> : IComparer<T>
{
    public int Compare(T x, T y)
    {
        return ICompareHandle(x, y);
    }
    public abstract int ICompareHandle(T x, T y);
    public abstract T SetkeySearch(object key);
}

// import data, sort, search 
public abstract class BYDataTable <T>: BYDataTableOgrin where T: class
{
    protected ConfigCompare<T> recordCompare;
    [SerializeField]
    protected List<T> records;

 
    private void OnEnable()
    {
        InitComparison();
    }

    public abstract void InitComparison();
 
    public override void ImportDataUsingNewtonJson(string textData)
    {
        //var dicData = ConvertCsvFileToJsonObject(textData);
        //if (records != null)
        //{
        //    records.Clear();
        //}
        //else
        //{
        //    records = new List<T>();
        //}
        //Type type = typeof(T);
        //BindingFlags flag = (BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
        //foreach (var e in dicData)
        //{
        //    T record = new T();
        //    foreach (var item in e)
        //    {
        //        var field = type.GetField(item.Key, flag);
        //        field.SetValue(item.Value, record);
        //    }
        //    records.Add(record);
        //}
        //var test = ConvertCsvFileToJsonObject(textData);

         if (records!=null)
        {
            records.Clear();
        }
        else
        {
            records = new List<T>();
        }
        List<List<string>> grid = SplitCSVFile(textData);

        Type type = typeof(T);
    
        FieldInfo[] members = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
       
        for (int i=1;i<grid.Count;i++)
        {
            if (grid[i].Count <= 0)
                continue;
            string jsonText = string.Empty;
            jsonText = "{";
            for (int j = 0; j < members.Length; j++)
            {
                FieldInfo fieldInfo = members[j];
                string data = "0";
                if (j>0)
                {
                    jsonText += ",";
                }
                if (grid[i].Count <= j)
                {
                    data = default;
                }
                else
                {
                    data = grid[i][j];
                }

                jsonText += "\"" + fieldInfo.Name + "\":" + "\"" + data+ "\"";
            }
            jsonText += "}";

            T dataRecord = JsonConvert.DeserializeObject<T>(jsonText, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                Error = HandleDeserializationError
            });
            records.Add(dataRecord);
        }
        records.Sort(recordCompare);
    }
    public override void ImportDataUsingJsonUtility(string textData)
    {

        if (records != null)
        {
            records.Clear();
        }
        else
        {
            records = new List<T>();
        }
        List<List<string>> grid = SplitCSVFile(textData);

        Type type = typeof(T);

        FieldInfo[] members = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

        for (int i = 1; i < grid.Count; i++)
        {
            if (grid[i].Count <= 0)
                continue;
            string jsonText = string.Empty;
            jsonText = "{";

            for (int j = 0; j < members.Length; j++)
            {
                FieldInfo fieldInfo = members[j];
                string data = string.Empty;
                if (j > 0)
                {
                    jsonText += ",";
                }
                if (grid[i].Count <= j)
                {
                    data = default;
                }
                else
                {
                    data = grid[i][j];
                }

                jsonText += "\"" + fieldInfo.Name + "\":" + "\"" + data + "\"";
            }
            jsonText += "}";
            T dataRecord = JsonUtility.FromJson<T>(jsonText);
            records.Add(dataRecord);
        }
        records.Sort(recordCompare);
    }
    public static void HandleDeserializationError(object sender, ErrorEventArgs errorArgs)
    {
        if(errorArgs.ErrorContext.Error is FormatException error)
        {
            Debug.LogError(error.Message);
        }
        errorArgs.ErrorContext.Handled = true;
    }
    public override string GetCSVData()
    {
        string s = string.Empty;
        Type mType = typeof(T);
        FieldInfo[] fieldInfos = mType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        for (int x = 0; x < fieldInfos.Length; x++)
        {
            if (x > 0)
                s += ",";
            s += fieldInfos[x].Name;

        }
        foreach (T e in records)
        {
            s += "\n";
            for (int x = 0; x < fieldInfos.Length; x++)
            {
                if (x > 0)
                    s += ",";
                s += fieldInfos[x].GetValue(e);

            }
        }
        return s;
    }



    private List<List<string>> SplitCSVFile(string texInput)
    {
        List<List<string>> grid = new List<List<string>>();
        char lineSeperater = '\n'; // It defines line seperate character
        string[] linds = texInput.Split(lineSeperater);
        foreach(string e in linds)
        {
            string[] row = SplitCSVLine2(e);
            List<string> r = new List<string>();
            foreach(string er in row)
            {
                r.Add(er);

                //Debug.LogError(er);
            }
           
            grid.Add(r);
        }
        return grid;
    }


    // splits a CSV row 
     private string[] SplitCsvLine(string line)
    {
        return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line, @"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)", System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
                select m.Groups[1].Value).ToArray();
    }

    private string[] SplitCSVLine2(string line)
    {
        List<string> results = new List<string>();
        //@"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)"
        //  Regex regex = new Regex(@"(((.(?=[,\r\n]+))|""(.([^""]|"""")+)""|(.[^,\r\n]+)),?)");
        // MatchCollection col= regex.Matches(line);
        string partern = @"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)";
        MatchCollection col =Regex.Matches(line, partern, RegexOptions.ExplicitCapture);
        foreach(Match e in col)
        {
            if(e.Groups["x"].Length>0)
            {
                results.Add(e.Groups["x"].Value);
            }
        }
        return results.ToArray();
    }
    public T GetRecordBykeySearch(object key)
    {
        T item = recordCompare.SetkeySearch(key);

        int index= records.BinarySearch(item, recordCompare);
        //return CopyData2(records[index],item);
        return CopyData(records[index]);
    }
    private T CopyData( object data)
    {
        string s = JsonUtility.ToJson(data);

        return JsonUtility.FromJson<T>(s);
    }
    private T CopyData2(object data, T dataOut)
    {

         Type type = typeof(T);
        FieldInfo[] members = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

        for (int i = 1; i < members.Length; i++)
        {
            FieldInfo fieldInfo = members[i];
            var inputData = fieldInfo.GetValue(data);

            fieldInfo.SetValue(dataOut, inputData);
        }
        return dataOut;
    }
}
