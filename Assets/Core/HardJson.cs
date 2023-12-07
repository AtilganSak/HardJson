using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class HardJson
{
    private const string ext = ".dat";

    private static List<object> dBs = new();

    public static void SaveJsonToFile(object _data, bool _is_crypto = false)
    {
        string _path = Path.Combine(Application.persistentDataPath, _data.ToString() + ext);
        string json_text = JsonUtility.ToJson(_data);
        if (_is_crypto)
            json_text = CryptoHelper.Encrypt(json_text, "-EnKey*-");
        File.WriteAllText(_path, json_text);
    }
    public static void SaveJsonToFile(object _data, string _path, bool _is_crypto = false)
    {
        string path = Path.Combine(_path, _data.ToString() + ext);
        string json_text = JsonUtility.ToJson(_data);
        if (_is_crypto)
            json_text = CryptoHelper.Encrypt(json_text, "-EnKey*-");
        File.WriteAllText(path, json_text);
    }    
    public static async void SaveJsonToFileAsync(object _data, bool _is_crypto = false, Action _callback = null)
    {
        string _path = Path.Combine(Application.persistentDataPath, _data.ToString() + ext);
        string json_text = JsonUtility.ToJson(_data);
        if (_is_crypto)
            json_text = CryptoHelper.Encrypt(json_text, "-EnKey*-");
        await File.WriteAllTextAsync(_path, json_text).ContinueWith((x) =>
        {
            if (x.IsCompleted)
            {
                if (_callback != null)
                    _callback.Invoke();
            }
        });
    }
    public static T GetJsonToFile<T>(bool _is_crypto = false) where T : new()
    {
        string _path = Path.Combine(Application.persistentDataPath, typeof(T).ToString() + ext);
        if (!File.Exists(_path))
        {
            var data = new T();
            SaveJsonToFile(data);
            dBs.Add(data);
            //DBService dBService = data as DBService;
            //if (dBService != null)
        }
        string get_json_text = File.ReadAllText(_path);
        if (_is_crypto)
            get_json_text = CryptoHelper.Decrypt(get_json_text, "-EnKey*-");

        return JsonUtility.FromJson<T>(get_json_text);        
    }    
    private static string FileDataPath()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            return "jar:file://" + Application.dataPath + "!/assets/";
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            return Application.dataPath + "/Raw/";
        }
        else
        {
            return Application.dataPath + "/StreamingAssets/";
        }
    }
    //When a new DB is created it needs to be added here. But only inhereted classes from DB.
    public static void SaveAllDatabases()
    {
        if (dBs.Count > 0)
        {
            foreach (var item in dBs)
            {
                SaveJsonToFile(item);
            }
        }
    }
#if UNITY_EDITOR
    [MenuItem("Tools/Delete Database")]
    private static void DeleteDataBase()
    {
        DeleteAllDatabases();
    }
    [MenuItem("Tools/Delete Database And Prefs")]
    private static void DeleteDataBaseAndPrefs()
    {
        DeleteAllDatabases();

        PlayerPrefs.DeleteAll();
    }
    //When a new DB is created it needs to be added here. But only inhereted classes from DB.
    private static void DeleteAllDatabases()
    {         
        DeleteFile(Path.Combine(Application.persistentDataPath, GameDB.Instance.ToString() + ext));
        DeleteFile(Path.Combine(Application.persistentDataPath, TransformDB.Instance.ToString() + ext));

        /*
         * If you want to find all classes automatically, you can close the line above and open the code snippet below.
         * But keep in mind that this code snippet will create performance issues. 
         * Because the reflection method is used, it checks all scripts in your project.
        */

        //var allTypes = Assembly.GetExecutingAssembly().GetTypes();
        //var derivedTypes = allTypes
        //    .Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericType && t.BaseType != null &&
        //                t.BaseType.IsGenericType &&
        //                t.BaseType.GetGenericTypeDefinition() == typeof(DB<>));

        //foreach (var derivedType in derivedTypes)
        //{
        //    DeleteFile(Path.Combine(Application.persistentDataPath, derivedType.ToString() + ext));
        //}
    }
    [MenuItem("Tools/Open DB Folder")]
    private static void OpenDBFolder()
    {
        System.Diagnostics.Process.Start(Application.persistentDataPath);
    }
    private static void DeleteFile(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
#endif
}