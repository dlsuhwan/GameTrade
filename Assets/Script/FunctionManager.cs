using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunctionManager : MonoBehaviour
{
    public static FunctionManager instacne;
    private void Awake()
    {
        instacne = this;
    }
    public static GameObject GetPrefab(string path, Transform parent)
    {
        GameObject obj = Resources.Load<GameObject>(path) as GameObject;
        GameObject prefabObj = Instantiate(obj, parent);
        
        return prefabObj;
    }
    public void TextMessage(string textString, string path, Transform parent)
    {
        GameObject obj = Resources.Load<GameObject>(path) as GameObject;
        GameObject prefabObj = Instantiate(obj, parent);
        prefabObj.transform.Find("Text").GetComponent<Text>().text = textString;

        StartCoroutine(ImgAlphaToZero(prefabObj.transform));
        StartCoroutine(TextAlphaToZero(prefabObj.transform.Find("Text")));
        
    }
    IEnumerator ImgAlphaToZero(Transform tr, float delay = 1f)
    {
        while (tr.GetComponent<Image>().color.a > 0)
        {
            Color color = tr.GetComponent<Image>().color;
            color.a -= Time.deltaTime * delay;
            tr.GetComponent<Image>().color = color;
            yield return new WaitForFixedUpdate();
        }
    }
    IEnumerator TextAlphaToZero(Transform tr, float delay = 1f)
    {
        while (tr.GetComponent<Text>().color.a > 0)
        {
            Color color = tr.GetComponent<Text>().color;
            color.a -= Time.deltaTime * delay;
            tr.GetComponent<Text>().color = color;
            yield return new WaitForFixedUpdate();
        }
    }
}
