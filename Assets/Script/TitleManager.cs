using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TitleManager : MonoBehaviour
{
    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        CheckNickName();

       
    }
    void CheckNickName()
    {
        Transform nickNameObj = canvas.transform.Find("NickName");
        if (PlayerPrefs.HasKey("NickName"))
        {
            nickNameObj.gameObject.SetActive(false);
        }
        else
        {
            nickNameObj.gameObject.SetActive(true);
        }
        canvas.transform.Find("StartBtn").GetComponent<Button>().onClick.RemoveAllListeners();
        canvas.transform.Find("StartBtn").GetComponent<Button>().onClick.AddListener(delegate () {
            string nickNmae = nickNameObj.GetComponent<InputField>().text;
            
            if (nickNameObj.gameObject.activeSelf && string.IsNullOrEmpty(nickNmae))
            {
                CheckNickName();
                FunctionManager.instacne.TextMessage("닉네임이 필요합니다.", "Prefabs/TextMessage", canvas.transform);
            }
            else
            {
                PlayerPrefs.SetString("NickName", nickNmae);
                SceneManager.LoadScene("Main");
            }

        });
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
