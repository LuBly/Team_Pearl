using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public SpawnData[] spawnData;

    private int enemyCount;
    private float curTime;
    private Transform[] spawnPoints;

    private void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
    }

    private void Update()
    {
        if (curTime >= spawnData[0].spawnTime && enemyCount < spawnData[0].maxCount)
        {
            Spawn();
            enemyCount++;
            curTime = 0;
        }
        curTime += Time.deltaTime;
    }

    private void Spawn()
    {
        int idx = Random.Range(1, spawnPoints.Length);
        GameObject enemy = GameManager.instance.pool.Get(spawnData[0].prefabId);
        enemy.transform.position = spawnPoints[idx].position;
        enemy.GetComponent<Enemy>().Init(spawnData[0]);
    }
}
[System.Serializable]
public class SpawnData //2차원 배열 [Stage][gen]_스테이지(입장시 선택)/1젠, 2젠, 3젠~~ (시간 단위로 소환)
{
    [Header("젠 시간")]
    public float spawnTime;
    [Header("최대 소환 마리수")]
    public int maxCount;
    [Header("몬스터 id")]
    public int prefabId;
    [Header("체력")]
    public int health;
    [Header("이동속도")]
    public float speed;
}
