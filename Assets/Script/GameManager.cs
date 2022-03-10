using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
            string skill = "";
            if(PlayerPrefs.HasKey("lv") && PlayerPrefs.HasKey("hp") && PlayerPrefs.HasKey("exp") && PlayerPrefs.HasKey("money"))
            {
                CharacterManager.instance.playerInfo.lv = lv = PlayerPrefs.GetInt("lv");
                CharacterManager.instance.playerInfo.hp = hp = PlayerPrefs.GetInt("hp");
                CharacterManager.instance.playerInfo.exp = exp = PlayerPrefs.GetInt("exp");
                CharacterManager.instance.playerInfo.money = money = PlayerPrefs.GetInt("money");
                CharacterManager.instance.playerInfo.skill = skill = PlayerPrefs.GetString("skill");
            }
            else
            {
                CharacterManager.instance.playerInfo.lv = 1;
                CharacterManager.instance.playerInfo.hp = 1;
                CharacterManager.instance.playerInfo.exp = 0;
                CharacterManager.instance.playerInfo.money = 0;
                CharacterManager.instance.playerInfo.skill = "&&";
            }
            UpdateUI_ProfileState(lv, hp, exp, money, skill);
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
            PlayerPrefs.SetString("skill", CharacterManager.instance.playerInfo.skill);
        }
    }
    public void UpdateUI_ProfileState(int lv, int hp, int exp, int money, string skill)
    {
        if (UICanvas0 == null) UICanvas0 = UICanvas.transform.Find("UICanvas0");
        if (UICanvas1 == null) UICanvas1 = UICanvas.transform.Find("UICanvas1");
        if (UICanvas2 == null) UICanvas2 = UICanvas.transform.Find("UICanvas2");
        if (UICanvas3 == null) UICanvas3 = UICanvas.transform.Find("UICanvas3");


        UpdateUI_Lv(lv);
        UpdateUI_Hp(hp);
        UpdateUI_Exp(exp);
        UpdateUI_Money(money);
        UpdateUI_Skill(skill);
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
    public void UpdateUI_Skill(string curSkill)
    {
        if (SkillPanel == null)
            SkillPanel = UICanvas0.Find("SkillPanel");

        string[] mySkills = curSkill.Split('&');

        for(int i=0; i<mySkills.Length; i++)
        {
            int btnIndex = i;
            if(!string.IsNullOrEmpty(mySkills[i]))
            {
                //skill data == icon#time&icon#time&icon#time
                SkillPanel.Find($"Button{btnIndex}/FilledImage").gameObject.SetActive(false);
                SkillPanel.Find($"Button{btnIndex}").GetComponent<Button>().onClick.RemoveAllListeners();
                SkillPanel.Find($"Button{btnIndex}").GetComponent<Button>().onClick.AddListener(delegate() {
                    SkillPanel.Find($"Button{btnIndex}/FilledImage").gameObject.SetActive(true);
                    StartCoroutine(SkillUse(SkillPanel.Find($"Button{btnIndex}/FilledImage"), float.Parse(mySkills[i].Split('#')[1])));
                });
            }
            else
            {
                SkillPanel.Find($"Button{btnIndex}/FilledImage").gameObject.SetActive(true);
                SkillPanel.Find($"Button{btnIndex}").GetComponent<Button>().onClick.RemoveAllListeners();
                SkillPanel.Find($"Button{btnIndex}").GetComponent<Button>().onClick.AddListener(delegate () {
                    FunctionManager.instacne.TextMessage("능력이 부족합니다.", "Prefabs/TextMessage", UICanvas1.transform);
                    
                });
            }
        }
    }
    IEnumerator SkillUse(Transform filledImg, float time)
    {
        filledImg.GetComponent<Image>().fillAmount = 0f;
        while (filledImg.GetComponent<Image>().fillAmount < 1)
        {
            filledImg.GetComponent<Image>().fillAmount += Time.deltaTime / time; 
            yield return new WaitForSeconds(Time.deltaTime / time);
            Debug.Log(DateTime.Now.ToString("HH:mm:ss"));
            Debug.Log(Time.deltaTime / time);
        }
        yield break;
    }


    void Update()
    {

    }


    /// <summary>
    /// ///////////////////////////////////////////////////////////
    /// </summary>
    
   
}
