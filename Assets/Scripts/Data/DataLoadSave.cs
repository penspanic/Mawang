using System;
using System.Security.Cryptography;
using UnityEngine;

static class DataLoadSave
{
    static byte[] secret;

    static DataLoadSave()
    {
        MD5 md5Hash = new MD5CryptoServiceProvider();
        secret = md5Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes("마 왕 의 그 녀 들"));
    }

    public static void SetString(string key, string value)
    {
        // Hide '_key' string.  
        MD5 md5Hash = MD5.Create();
        byte[] hashData = md5Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(key));
        string hashKey = System.Text.Encoding.UTF8.GetString(hashData);

        // Encrypt '_value' into a byte array  
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(value);

        // Eecrypt '_value' with 3DES.  
        TripleDES des = new TripleDESCryptoServiceProvider();
        des.Key = secret;
        des.Mode = CipherMode.ECB;
        ICryptoTransform xform = des.CreateEncryptor();
        byte[] encrypted = xform.TransformFinalBlock(bytes, 0, bytes.Length);

        // Convert encrypted array into a readable string.  
        string encryptedString = Convert.ToBase64String(encrypted);

        // Set the ( key, encrypted value ) pair in regular PlayerPrefs.  
        PlayerPrefs.SetString(hashKey, encryptedString);

    }

    public static string GetString(string key)
    {
        // Hide '_key' string.  
        MD5 md5Hash = MD5.Create();
        byte[] hashData = md5Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(key));
        string hashKey = System.Text.Encoding.UTF8.GetString(hashData);

        // Retrieve encrypted '_value' and Base64 decode it.  
        string _value = PlayerPrefs.GetString(hashKey);
        byte[] bytes = Convert.FromBase64String(_value);

        // Decrypt '_value' with 3DES.  
        TripleDES des = new TripleDESCryptoServiceProvider();
        des.Key = secret;
        des.Mode = CipherMode.ECB;
        ICryptoTransform xform = des.CreateDecryptor();
        byte[] decrypted = xform.TransformFinalBlock(bytes, 0, bytes.Length);

        // decrypte_value as a proper string.  
        string decryptedString = System.Text.Encoding.UTF8.GetString(decrypted);

        return decryptedString;
    }

    public static void SetInt(string key, int value)
    {
        SetString(key, value.ToString());
    }

    public static void SetBool(string key, bool value)
    {
        SetString(key, value.ToString());
    }

    public static int GetInt(string key)
    {
        return Int32.Parse(GetString(key));
    }

    public static bool GetBool(string key)
    {
        return bool.Parse(GetString(key));
    }

    public static void SetFloat(string key, float value)
    {
        SetString(key, value.ToString());
    }

    public static float GetFloat(string key)
    {
        return float.Parse(GetString(key));
    }

    public static bool HasKey(string key)
    {
        // Hide '_key' string.  
        MD5 md5Hash = MD5.Create();
        byte[] hashData = md5Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(key));
        string hashKey = System.Text.Encoding.UTF8.GetString(hashData);

        return PlayerPrefs.HasKey(hashKey);
    }

}