using System;

/// <summary>
/// If you want to keep the database file on the device temporarily, you need to inherit from this class.
/// </summary>
/// <typeparam name="T"></typeparam>
public class DB<T> where T : class, new()
{
    static T _instance;
    public static T Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = HardJson.GetJsonToFile<T>();              
            }
            return _instance;
        }
    }
    public void Save()
    {
        HardJson.SaveJsonToFile(_instance);        
    }
    public void SaveAsync(Action callback)
    {
        HardJson.SaveJsonToFileAsync(_instance, _callback: callback);
    }
}