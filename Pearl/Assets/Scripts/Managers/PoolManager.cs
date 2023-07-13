using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;
    List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];
        for (int idx = 0; idx < pools.Length; idx++)
        {
            pools[idx] = new List<GameObject>();
        }
    }

    public GameObject Get(int idx)
    {
        GameObject select = null;
        //... 선택한 풀에 놀고 있는(비활성화) 게임 오브젝트 접근
        foreach (GameObject item in pools[idx])
        {
            if (!item.activeSelf)//비활성화 된 게임 오브젝트가 있다면?
            {
                //... 발견하면 select 변수에 할당
                select = item;
                select.SetActive(true);
                break;
            }
        }
        //... 모두 쓰고 있다면?        
        if (!select)
        {
            //... 새롭게 생성해서 select 변수에 할당
            select = Instantiate(prefabs[idx], transform);
            pools[idx].Add(select);
        }
        return select;
    }
    /*
    
    public int maxCount;
    public int enemyCount;

    public float spawnTime;
    public float curTime;
    public Transform[] spawnPoints;
    public GameObject enemy;

    

    private void Update()
    {
        if (curTime >= spawnTime && enemyCount < maxCount)
        {
            int idx = Random.Range(0, spawnPoints.Length);
            SpawnEnemy(idx);
        }
        curTime += Time.deltaTime;
    }

    public void SpawnEnemy(int idx)
    {
        curTime = 0;
        enemyCount++;
        Instantiate(enemy, spawnPoints[idx]);
    }
    */
}
