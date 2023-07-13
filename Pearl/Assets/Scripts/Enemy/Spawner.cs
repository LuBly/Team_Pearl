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
    public float spawnTime;
    public int maxCount;

    public int prefabId;
    public int health;
    public float speed;
}
