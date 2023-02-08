using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;

[Serializable]
public class PlayerData
{
    public string playerName;
    public string avatarName;
    public PlayerInventory inventory;
}

[Serializable]
public class PlayerInventory
{
    public int chip = 1000;

    public PlayerInventory(string chip)
    {
        int.TryParse(chip, out this.chip);
    }
}

public static class DataUtilities
{

    public static string ToKey(this object data)
    {
        return "K_" + data.ToString();
    }
   
}