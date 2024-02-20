using System.Collections;
using UnityEngine;
using System;
using TMPro;

[Serializable]
public class AutoAtk
{
    public Scanner scanner;
    [Header("적 인식 범위")]
    public float skillRange;
    [Header("공격과 공격 사이 Delay")]
    public float attackDelay;
    [Header("사용할 BulletPrefab Idx")]
    public int bulletIdx;
}

public enum SkillType
{
    defaultAtk,
    continuousAttack, // 지속 공격 ex) 제압사격
    grenadeAttack,    // 즉발 공격 ex) 수류탄
    snipperAttack,    // 범위 선택 공격 ex) 포격요청
    autoAttack,       // 자동 공격 ex) 드론 공격
}
public class Skill : MonoBehaviour
{

    /// skillType      : 스킬의 종류
    /// -> 지속 공격, 단일 공격, 비네트 공격, 기본 공격 강화, etc
    /// enableTime     : 활성화 시간
    /// attackTime     : 피해를 주는 시간 단위
    /// damage         : 틱 당 데미지
    /// knockbackPower : 틱 당 넉백 정도
    /// </summary>
    [HideInInspector][SerializeField] public SkillType skillType;
    [HideInInspector][SerializeField] private SnipperAtk snipperAtk;
    [HideInInspector][SerializeField] private AutoAtk autoAtk;
    [HideInInspector][SerializeField] public GameManager gameManager;

    // 항상 사용
    public float damage;
    public float knockbackPower;
    public LayerMask enemyLayer;
    public bool isStopFire;

    // 자동공격 때 사용할 쿨타임
    float curTime;

    

    private void Awake()
    {
        gameManager = GameObject.FindWithTag("GM").GetComponent<GameManager>();
        switch (skillType)
        {
            case SkillType.autoAttack:
                autoAtk.scanner.scanRange = autoAtk.skillRange;
                autoAtk.scanner.targetLayer = enemyLayer;
                curTime = 0;
                //Dev Debugging
                transform.Find("SkillRange").GetComponent<Transform>().localScale = new Vector3(autoAtk.skillRange * 4, autoAtk.skillRange * 4, 1);
                break;
        }
    }

    private void Start()
    {
        if (isStopFire)
        {
            gameManager.isStopFire = true;
        }
        switch (skillType)
        {
            case SkillType.autoAttack:
                break;
        }
    }

    private void Update()
    {
        switch (skillType)
        {
            case SkillType.autoAttack:
                curTime += Time.deltaTime;
                if (curTime >= autoAtk.attackDelay)
                {
                    // 스캐너에 걸리는 몬스터가 있다면 
                    if (autoAtk.scanner.nearestTarget != null)
                    {
                        Debug.Log("Fire");
                        DronFire(autoAtk.scanner.nearestTarget);
                        curTime = 0f;
                    }
                }
                break;
        }
    }
    private void OnDestroy()
    {
        gameManager.isStopFire = false;
    }
    
    public void DronFire(Transform target)
    {
        Vector3 targetPos = target.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;
        Transform bullet = GameObject.FindWithTag("GM")
                                     .GetComponent<GameManager>().pool
                                     .SkillBulletGet(autoAtk.bulletIdx).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.down, dir);
        bullet.GetComponent<Bullet>().Init(damage, 0, knockbackPower, 3f, dir);
    }
}