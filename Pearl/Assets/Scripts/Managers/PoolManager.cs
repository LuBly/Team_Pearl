using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    List<GameObject>[] enemyPools;

    public GameObject[] bulletPrefabs;
    List<GameObject>[] bulletPools;

    public GameObject[] skillBulletPrefabs;
    List<GameObject>[] skillBulletPools;

    private void Awake()
    {
        enemyPools = new List<GameObject>[enemyPrefabs.Length];
        for (int idx = 0; idx < enemyPools.Length; idx++)
        {
            enemyPools[idx] = new List<GameObject>();
        }

        bulletPools = new List<GameObject>[bulletPrefabs.Length];
        for (int idx = 0; idx < bulletPools.Length; idx++)
        {
            bulletPools[idx] = new List<GameObject>();
        }

        skillBulletPools = new List<GameObject>[skillBulletPrefabs.Length];
        for (int idx = 0; idx < skillBulletPools.Length; idx++)
        {
            skillBulletPools[idx] = new List<GameObject>();
        }

    }

    public GameObject EnemyGet(int idx)
    {
        GameObject select = null;
        //... 선택한 풀에 놀고 있는(비활성화) 게임 오브젝트 접근
        foreach (GameObject item in enemyPools[idx])
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
            select = Instantiate(enemyPrefabs[idx], transform);
            enemyPools[idx].Add(select);
        }
        return select;
    }

    public GameObject BulletGet(int idx)
    {
        GameObject select = null;
        //... 선택한 풀에 놀고 있는(비활성화) 게임 오브젝트 접근
        foreach (GameObject item in bulletPools[idx])
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
            select = Instantiate(bulletPrefabs[idx], transform);
            bulletPools[idx].Add(select);
        }
        return select;
    }

    public GameObject SkillBulletGet(int idx)
    {
        GameObject select = null;
        //... 선택한 풀에 놀고 있는(비활성화) 게임 오브젝트 접근
        foreach (GameObject item in skillBulletPools[idx])
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
            select = Instantiate(skillBulletPrefabs[idx], transform);
            skillBulletPools[idx].Add(select);
        }
        return select;
    }
}
