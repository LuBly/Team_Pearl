using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int enemyId;
    public int maxCount;
    public int enemyCount;

    public float spawnTime;
    public float curTime;

    public Transform[] spawnPoints;

    private void Update()
    {
        if (curTime >= spawnTime && enemyCount < maxCount)
        {
            int idx = Random.Range(0, spawnPoints.Length);
            GameObject enemy = GameManager.instance.pool.Get(enemyId);
            enemy.transform.position = spawnPoints[idx].position;
            enemyCount++;
            curTime = 0;
        }
        curTime += Time.deltaTime;
    }
}
