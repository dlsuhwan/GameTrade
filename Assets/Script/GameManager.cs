using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public GameObject tradeScroll;
 

    public GameObject UICanvas;
    public Transform UICanvas0;
    public Transform UICanvas1;
    public Transform UICanvas2;
    public Transform UICanvas3;

    public Transform SkillPanel;
    public Transform ExpPanel;
    public Transform ProfilePanel;
    public Transform SystemPanel;


    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    void Start()
    {
        //GameDataManager.DataSave("1#10&2#30&3#50&4#75&5#100&6#130&7#160&8#200&9#250&10#300", "LvData");
        GameDataManager.TamplateDataLoad();
        Debug.Log(GameDataManager.LevelTempalte);

        UICanvas = GameObject.Find("UICanvas");
        DataLoad();
        GameObject obj = FunctionManager.GetPrefab("Character/Dino", GameObject.Find("Player").transform);        
    }

    public void DataLoad()
    {
        if(CharacterManager.instance.playerInfo != null)
        {
            int lv = 0, hp = 0, exp = 0, money = 0;
            if(PlayerPrefs.HasKey("lv") && PlayerPrefs.HasKey("hp") && PlayerPrefs.HasKey("exp") && PlayerPrefs.HasKey("money"))
            {
                CharacterManager.instance.playerInfo.lv = lv = PlayerPrefs.GetInt("lv");
                CharacterManager.instance.playerInfo.hp = hp = PlayerPrefs.GetInt("hp");
                CharacterManager.instance.playerInfo.exp = exp = PlayerPrefs.GetInt("exp");
                CharacterManager.instance.playerInfo.money = money = PlayerPrefs.GetInt("money");
            }
            else
            {
                CharacterManager.instance.playerInfo.lv = 1;
                CharacterManager.instance.playerInfo.hp = 1;
                CharacterManager.instance.playerInfo.exp = 0;
                CharacterManager.instance.playerInfo.money = 0;
            }
            UpdateUI_ProfileState(lv, hp, exp, money);
        }
    }
    public void DataSave()
    {
        if (CharacterManager.instance.playerInfo != null)
        {
            PlayerPrefs.SetInt("lv", CharacterManager.instance.playerInfo.lv);
            PlayerPrefs.SetInt("hp", CharacterManager.instance.playerInfo.hp);
            PlayerPrefs.SetInt("exp", CharacterManager.instance.playerInfo.exp);
            PlayerPrefs.SetInt("money", CharacterManager.instance.playerInfo.money);
        }
    }
    public void UpdateUI_ProfileState(int lv, int hp, int exp, int money)
    {
        if (UICanvas0 == null)
            UICanvas0 = UICanvas.transform.Find("UICanvas0");
        
        UpdateUI_Lv(lv);
        UpdateUI_Hp(hp);
        UpdateUI_Exp(exp);
        UpdateUI_Money(money);
    }
    public void UpdateUI_Lv(int curLv)
    {
        if (ProfilePanel == null)
            ProfilePanel = UICanvas0.Find("ProfilePanel");
    }
    public void UpdateUI_Hp(int curHp)
    {
        if (ProfilePanel == null)
            ProfilePanel = UICanvas0.Find("ProfilePanel");
    }
    public void UpdateUI_Exp(int curExp)
    {
        if (ExpPanel == null)
            ExpPanel = UICanvas0.Find("ExpPanel");
    }
    public void UpdateUI_Money(int curMoney)
    {
        if (ProfilePanel == null)
            ProfilePanel = UICanvas0.Find("ProfilePanel");
    }
    
    void Update()
    {

    }


    /// <summary>
    /// ///////////////////////////////////////////////////////////
    /// </summary>
    
   
}
