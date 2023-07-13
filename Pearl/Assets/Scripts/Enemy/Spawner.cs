using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int prefabId;
    public int maxCount;
    public int enemyCount;

    public float spawnTime;
    public float curTime;

    private Transform[] spawnPoints;

    void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
    }

    private void Update()
    {
        if (curTime >= spawnTime && enemyCount < maxCount)
        {
            int idx = Random.Range(1, spawnPoints.Length);
            GameObject enemy = GameManager.instance.pool.Get(prefabId);
            enemy.transform.position = spawnPoints[idx].position;
            enemyCount++;
            curTime = 0;
        }
        curTime += Time.deltaTime;
    }
}
