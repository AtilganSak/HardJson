using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class HardJson
{
    private const string ext = ".dat";

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
    public static void SaveJsonToStreamingFile(object _data, bool _is_crypto = false)
    {
        string path = Path.Combine(FileDataPath(), _data.ToString() + ext);
        string json_text = JsonUtility.ToJson(_data);
        if (_is_crypto)
            json_text = CryptoHelper.Encrypt(json_text, "-EnKey*-");

#if UNITY_EDITOR
        if (!Directory.Exists("Assets/StreamingAssets"))
        {
            Directory.CreateDirectory("Assets/StreamingAssets");
        }
#endif
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
            SaveJsonToFile(new T());
        }
        string get_json_text = File.ReadAllText(_path);
        if (_is_crypto)
            get_json_text = CryptoHelper.Decrypt(get_json_text, "-EnKey*-");

        return JsonUtility.FromJson<T>(get_json_text);        
    }
    public static T GetJsonFromStreaming<T>(bool _is_crypto = false) where T : new()
    {
        string _path = FileDataPath() + typeof(T).ToString() + ext;
        string get_json_text = "";

        if (!File.Exists(_path))
        {
            SaveJsonToStreamingFile(new T());
        }
#if UNITY_EDITOR || UNITY_IOS
        get_json_text = File.ReadAllText(_path);
#elif UNITY_ANDROID
            WWW reader = new WWW (_path);
            while (!reader.isDone) { }
            get_json_text = reader.text;
#endif        
        if (_is_crypto)
            get_json_text = CryptoHelper.Decrypt(get_json_text, "-EnKey*-");

        return (T)JsonUtility.FromJson(get_json_text, typeof(T));
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
        GameDB.Instance.Save();
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