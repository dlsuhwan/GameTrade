using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject tradeScroll;
    void Start()
    {
        GameDataManager.DataSave("1#100&2#1000&3#1000", "data");
        //GameDataManager.DataLoad("data");
        TradeMarket();
        //GameObject obj = FunctionManager.GetPrefab("Character/Dino", GameObject.Find("Player").transform);
    }

    public void TradeMarket()
    {
        if (tradeScroll == null)
            tradeScroll = GameObject.Find("Canvas/Scroll");

        string data = GameDataManager.DataLoad("data");
        string[] datas = data.Split('&');

        Transform grid = tradeScroll.transform.Find("Grid");

        for(int i=0; i<3; i++)
        {
            GameObject obj = FunctionManager.GetPrefab("Prefabs/TradeButton", grid);
            obj.transform.Find("Button/Text").GetComponent<TMP_Text>().text = datas[i].Split('#')[0];
            obj.transform.Find("Image/Text").GetComponent<TMP_Text>().text = datas[i].Split('#')[1];

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
