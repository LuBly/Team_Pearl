using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
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
}
