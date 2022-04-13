using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpwaner : MonoBehaviour
{
    public static MonsterSpwaner instance;
    
    public int monsterMaxCount = 0;
    public int monsterCurCount = 0;
    public Vector2[] spawnPoint;
    private Transform monsterTr;
    public void MonsterGerate(int maxCount, Vector2[] spawnPoint)
    {
        monsterMaxCount = maxCount;
        this.spawnPoint = spawnPoint;
        StartCoroutine(StartMonsterGerate());
    }
    IEnumerator StartMonsterGerate()
    {
        if(monsterMaxCount > monsterCurCount)
        {
            int randomPoint = Random.Range(0, spawnPoint.Length);
            GameObject obj = FunctionManager.GetPrefab("Character/Monster", monsterTr);
            obj.transform.position = spawnPoint[randomPoint];
            monsterCurCount++;
        }
        yield return new WaitForFixedUpdate();
    }
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        monsterTr = GameObject.Find("Monster").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
