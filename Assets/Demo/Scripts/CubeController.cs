using UnityEngine;

public class CubeController : MonoBehaviour
{
    public float force = 250;
    
    Rigidbody[] cubes;

    private void OnEnable()
    {
        cubes = GetComponentsInChildren<Rigidbody>();
    }        
    private void AddForce()
    {
        for (int i = 0; i < cubes.Length; i++)
        {
            cubes[i].AddForce(cubes[i].transform.up * force);
        }
    }
    private void LoadCubeDatas()
    {
        if (TransformDB.Instance.transformDatas != null && TransformDB.Instance.transformDatas.Length > 0)
        {
            for (int i = 0; i < cubes.Length; i++)
            {
                cubes[i].transform.position = TransformDB.Instance.transformDatas[i].position;
                cubes[i].transform.rotation = TransformDB.Instance.transformDatas[i].rotation;
            }
        }
    }
    private void SaveCubeTransforms()
    {
        TransformData[] datas = new TransformData[cubes.Length];
        for(int i = 0;i < cubes.Length; i++)
        {
            datas[i].position = cubes[i].transform.position;
            datas[i].rotation = cubes[i].transform.rotation;
        }
        TransformDB.Instance.transformDatas = datas;
        TransformDB.Instance.Save();
    }
    private void OnGUI()
    {
        GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
        myButtonStyle.fontSize = 35;
        if (GUI.Button(new Rect(0, 0, 200, 100), "Add Force", myButtonStyle))
        {
            AddForce();
        }
        if (GUI.Button(new Rect(0,102,200,100),"Save", myButtonStyle))
        {


            SaveCubeTransforms();
        }
        if (GUI.Button(new Rect(0, 202, 200, 100), "Load", myButtonStyle))
        {
            LoadCubeDatas();
        }
        if (GUI.Button(new Rect(0, 302, 200, 100), "Clear", myButtonStyle))
        {            
            TransformDB.Instance.transformDatas = null;
            TransformDB.Instance.Save();
        }
    }
}
