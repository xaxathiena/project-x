using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class Security
{
    public static string GetMD5Working(string input)
    {
        MD5 md5 = MD5.Create();

        byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
        StringBuilder sb = new StringBuilder();
        for (int j = 0; j < hash.Length; j++)
        {
            sb.Append(hash[j].ToString("X2"));
        }

        return sb.ToString().ToLower();
    }
}
