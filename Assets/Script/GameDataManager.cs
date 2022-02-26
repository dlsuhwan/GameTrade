using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text;
using System.Security.Cryptography;
using System;

public enum thing
{
    index,
    value,

    max,
}

[System.Serializable]
public class Things
{
    public string index;
    public string value;
}
public class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return UnityEngine.JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instanse;

    public static string key = "tradeGame";


    public static Dictionary<string, string> LevelTempalte = new Dictionary<string, string>();

    private void Awake()
    {
        if (instanse == null)
            instanse = this;
    }
    public static void DataSave(string strData, string fileName)
    {
        string[] strDatas = strData.Split('&');
        Things[] datas = new Things[strDatas.Length];

        for (int i=0; i<datas.Length; i++)
        {
            datas[i] = new Things();
            datas[i].index = Encrypt(strDatas[i].Split('#')[(int)thing.index], key);
            datas[i].value = Encrypt(strDatas[i].Split('#')[(int)thing.value], key);
        }
        
        //Json 데이터로 만들기
        string jsonData = JsonHelper.ToJson(datas, true);

        File.WriteAllText(Application.persistentDataPath + $"/{fileName}.json", jsonData);    ///Resources  //유니티 에디터
        //File.WriteAllText(Application.persistentDataPath +  $"/{fileName}.json", jsonData);    ///Resources  //모바일 에디터
    }
    public static void DataLoad(string fileName, Dictionary<string, string> dic)
    {
        //Mobile
        string jsonData = File.ReadAllText(Application.persistentDataPath + $"/{fileName}.json");   ///Resources

        //PC
        //string jsonData = File.ReadAllText(Application.dataPath + $"/{fileName}.json"); 

        if (string.IsNullOrEmpty(jsonData)) return;

        Things[] data = JsonHelper.FromJson<Things>(jsonData);
        
        string[] value = new string[(int)thing.max];
        string[] values = new string[data.Length];

        for (int i = 0; i < data.Length; i++)
        {
            value[(int)thing.index] = Decrypt((data[i].index), key);
            value[(int)thing.value] = Decrypt((data[i].value), key);
            
            if(dic.ContainsKey(value[(int)thing.index]))
            {
                dic[value[(int)thing.index]] = value[(int)thing.value];
            }
            else
            {
                dic.Add(value[(int)thing.index], value[(int)thing.value]);
            }
        }
    }
    
    public static void TamplateDataLoad()
    {
        DataLoad("LvData", LevelTempalte);
    }

    public static string Decrypt(string textToDecrypt, string key)
    {
        RijndaelManaged rijndaelCipher = new RijndaelManaged();

        rijndaelCipher.Mode = CipherMode.CBC;
        rijndaelCipher.Padding = PaddingMode.PKCS7;
        rijndaelCipher.KeySize = 128;
        rijndaelCipher.BlockSize = 128;

        byte[] encryptedData = Convert.FromBase64String(textToDecrypt);
        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
        byte[] keyBytes = new byte[16];
        int len = pwdBytes.Length;

        if (len > keyBytes.Length)
            len = keyBytes.Length;

        Array.Copy(pwdBytes, keyBytes, len);

        rijndaelCipher.Key = keyBytes;
        rijndaelCipher.IV = keyBytes;

        byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);

        return Encoding.UTF8.GetString(plainText);
    }



    public static string Encrypt(string textToEncrypt, string key)
    {
        RijndaelManaged rijndaelCipher = new RijndaelManaged();

        rijndaelCipher.Mode = CipherMode.CBC;
        rijndaelCipher.Padding = PaddingMode.PKCS7;
        rijndaelCipher.KeySize = 128;
        rijndaelCipher.BlockSize = 128;

        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
        byte[] keyBytes = new byte[16];

        int len = pwdBytes.Length;

        if (len > keyBytes.Length)
            len = keyBytes.Length;

        Array.Copy(pwdBytes, keyBytes, len);

        rijndaelCipher.Key = keyBytes;
        rijndaelCipher.IV = keyBytes;

        ICryptoTransform transform = rijndaelCipher.CreateEncryptor();

        byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);

        return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
    }
}
