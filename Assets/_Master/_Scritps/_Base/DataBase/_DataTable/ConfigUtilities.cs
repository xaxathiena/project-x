using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
public class ConfigUtilities 
{

}

public class ConfigPrimarykeyCompare<T2> : ConfigCompare<T2> where T2 : class, new()
{
    FieldInfo keyField;
    public ConfigPrimarykeyCompare(string keyFieldName)
    {
        keyField = typeof(T2).GetField(keyFieldName,  BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public );
    }
    public override int ICompareHandle(T2 x, T2 y)
    {
        var val_x = keyField.GetValue(x);
        var val_y = keyField.GetValue(y);

        if (val_x == null && val_y == null)
        {
            return 0;
        }
        else if (val_x != null && val_y == null)
        {
            return 1;
        }
        else if (val_x == null && val_y != null)
        {
            return -1;
        }
        else
        {
            return ((IComparable)val_x).CompareTo(val_y);
        }
    }
    public override T2 SetkeySearch(object key)
    {
        T2 data = new T2();
        keyField.SetValue(data, key);
        return data;
    }
}