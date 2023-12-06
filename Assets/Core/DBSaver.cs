using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBSaver : MonoBehaviour
{
    private void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void OnDestroy()
    {
        HardJson.SaveAllDatabases();
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            HardJson.SaveAllDatabases();
        }
    }
}
