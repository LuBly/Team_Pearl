using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public SpawnData[] spawnData;
    public int enemyCount;    // 현재 화면에 나타나 있는 Enemy의 수
    [SerializeField]
    private int maxCount = 30; // 화면에 나타날 수 있는 최대 마리수
    private float curTime;
    private Transform[] spawnPoints;

    private void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
    }

    private void Update()
    {
        if (curTime >= spawnData[0].spawnTime && enemyCount < maxCount)
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
public class SpawnData
{
    [Header("젠 시간")]
    public float spawnTime;
    [Header("몬스터 id")]
    public int prefabId;
    [Header("체력")]
    public int health;
    [Header("이동속도")]
    public float speed;
    [Header("공격력")]
    public float damage;
}
