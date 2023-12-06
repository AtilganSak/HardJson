using UnityEngine;

[CreateAssetMenu(fileName = "Default GameDB")]
public class DefaultGameDB : SingletonScriptableObject<DefaultGameDB>
{
    public float coin;
    public int star;        
    public int level;    
}
