using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAtk : Skill
{
    [Header("적 인식 범위")]
    public float skillRange;
    [Header("공격과 공격 사이 Delay")]
    public float attackDelay;
    [Header("사용할 BulletPrefab Idx")]
    public int bulletIdx;

    // 자동공격 때 사용할 쿨타임
    private float curTime;
    private Scanner scanner;
    private void Awake()
    {
        scanner = GetComponent<Scanner>();
        curTime = 0;
        //Dev Debugging
        transform.Find("SkillRange").GetComponent<Transform>().localScale = new Vector3(skillRange * 4, skillRange * 4, 1);
    }
    private void Start()
    {
        skillType = SkillType.autoAttack;
        scanner.scanRange = skillRange;
        scanner.targetLayer = enemyLayer;
    }
    // Update is called once per frame
    private void Update()
    {
        curTime += Time.deltaTime;
        if (curTime >= attackDelay)
        {
            // 스캐너에 걸리는 몬스터가 있다면 
            if (scanner.nearestTarget != null)
            {
                DronFire(scanner.nearestTarget);
                curTime = 0f;
            }
        }
    }

    private void DronFire(Transform target)
    {
        Vector3 targetPos = target.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;
        Transform bullet = GameObject.FindWithTag("GM")
                                     .GetComponent<GameManager>().pool
                                     .SkillBulletGet(bulletIdx).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.down, dir);
        bullet.GetComponent<Bullet>().Init(damage, 0, knockbackPower, 3f, dir);
    }
}
