using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionManager : MonoBehaviour
{
    public static GameObject GetPrefab(string path, Transform parent)
    {
        GameObject obj = Resources.Load<GameObject>(path) as GameObject;
        GameObject prefabObj = Instantiate(obj, parent);
        
        return prefabObj;
    }
}
