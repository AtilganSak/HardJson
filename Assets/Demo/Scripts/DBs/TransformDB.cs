using UnityEngine;

public class TransformDB : DB<TransformDB>
{    
    public TransformData[] transformDatas;

    public TransformDB()
    {
        transformDatas = new TransformData[0];
    }
}
[System.Serializable]
public struct TransformData
{
    public int id;
    public Vector3 position;
    public Quaternion rotation;
}
