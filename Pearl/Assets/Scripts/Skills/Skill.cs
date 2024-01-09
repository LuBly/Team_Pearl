using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using System.Runtime.CompilerServices;

[Serializable]
public class ContinuousAtk
{
    /// <summary>
    /// 활성화 되었을 때 스킬 발동
    /// 범위내에 있는 모든 적들에게 피해를 주는 방식
    /// -> 1초 공격(피해 0.2초 단위) -> 1초 비활성화 -> 1초 공격(피해 0.2초 단위)
    /// enableTime     : 활성화 시간
    /// attackTime     : 피해를 주는 시간 단위
    [Header("활성화 시간")]
    public float enableTime;
    [Header("피해를 주는 시간 단위")]
    public float attackTime;
}

[Serializable]
public class GrenadeAtk
{
    public Scanner scanner;
    public GameObject skillRange;
    public GameObject attackPoint;
    public GameObject skillImpact;
    public JoystickMovement skillJoystickMovement;
}

[Serializable]
public class SnipperAtk
{
    [Header("공격 범위")]
    public float attackRange;
    [Header("탄창")]
    public int fullAmmo;
    public int curAmmo;
    [Header("영역 전개 시간")]
    public float castTime;

    [Header("느려지는 정도 _ 낮을 수록 많이 느려집니다.")]
    [Range(0f, 1f)]
    public float targetDeltaTime;

    public GameObject fireEffect;
    public TextMeshProUGUI ammoText;
    public Image fillImage;
}

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
    autoAttack,
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
    [HideInInspector][SerializeField] private ContinuousAtk continuousAtk;
    [HideInInspector][SerializeField] private GrenadeAtk grenadeAtk;
    [HideInInspector][SerializeField] private SnipperAtk snipperAtk;
    [HideInInspector][SerializeField] private AutoAtk autoAtk;

    // 항상 사용
    public float damage;
    public float knockbackPower;
    public LayerMask enemyLayer;
    [HideInInspector] public float attackTime;

    // 자동공격 때 사용할 쿨타임
    float curTime;
    

    private void Awake()
    {
        switch (skillType)
        {
            case SkillType.grenadeAttack :
                grenadeAtk.scanner = GameObject.FindWithTag("Player").GetComponent<Scanner>();
                grenadeAtk.skillJoystickMovement = GameObject.FindWithTag("Skill_Grenade").GetComponent<JoystickMovement>();
                break;

            case SkillType.autoAttack:
                autoAtk.scanner.scanRange = autoAtk.skillRange;
                autoAtk.scanner.targetLayer = enemyLayer;
                //Dev Debugging
                transform.Find("SkillRange").GetComponent<Transform>().localScale = new Vector3(autoAtk.skillRange * 4, autoAtk.skillRange * 4, 1);
                break;
        }
    }

    private void Start()
    {
        switch (skillType)
        {
            case SkillType.continuousAttack:
                attackTime = continuousAtk.attackTime;
                StartCoroutine(ActiveContinuousSkill());
                break;

            case SkillType.grenadeAttack:
                bool isDrag = grenadeAtk.skillJoystickMovement.isDrag;
                // Drag 공격
                if (isDrag)
                {
                    grenadeAtk.skillRange.SetActive(true);
                    grenadeAtk.attackPoint.SetActive(true);
                    grenadeAtk.attackPoint.GetComponent<CapsuleCollider2D>().enabled = false;
                }

                // 일반 공격
                else
                {
                    if (grenadeAtk.scanner.nearestTarget)
                    {
                        grenadeAtk.attackPoint.SetActive(true);
                        grenadeAtk.skillImpact.SetActive(true);
                        grenadeAtk.attackPoint.transform.position = grenadeAtk.scanner.nearestTarget.position;

                        //쿨타임 돌게 신호 전달
                    }
                    else
                    {
                        Debug.Log("No Enemy");
                    }
                }
                break;

            case SkillType.snipperAttack:
                snipperAtk.curAmmo = snipperAtk.fullAmmo;
                snipperAtk.fillImage = this.GetComponent<Image>();
                StartCoroutine(ActiveArea());
                break;

            case SkillType.autoAttack:
                curTime = autoAtk.attackDelay;
                break;
        }
    }

    private void Update()
    {
        switch (skillType)
        {
            case SkillType.grenadeAttack:
                if (grenadeAtk.skillJoystickMovement.isDrag == true)
                {
                    // 방향 (JoyVec)
                    // 크기 L2 = L1*R/r
                    grenadeAtk.attackPoint.transform.position =
                        grenadeAtk.skillRange.transform.position
                        + grenadeAtk.skillJoystickMovement.joyVec
                        * grenadeAtk.skillJoystickMovement.stickDistance
                        / 40f;
                }
                break;

            case SkillType.snipperAttack:
                snipperAtk.ammoText.text = snipperAtk.curAmmo.ToString() + " / " + snipperAtk.fullAmmo.ToString();
                if (snipperAtk.curAmmo == 0)
                {
                    DeActivateSkill();
                }
                break;

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
        switch (skillType)
        {
            case SkillType.continuousAttack:
                StopCoroutine(ActiveContinuousSkill());
                break;
            case SkillType.grenadeAttack:
                grenadeAtk.attackPoint.SetActive(false);
                grenadeAtk.skillImpact.SetActive(false);
                grenadeAtk.skillRange.SetActive(false);
                StopCoroutine(ActiveInstantSkill());
                grenadeAtk.attackPoint.GetComponent<CapsuleCollider2D>().enabled = true;
                break;
        }
    }

    public void TouchPointAttack()
    {
        Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        snipperAtk.curAmmo--;
        // touchPos에 이펙트 생성 및 데미지
        Instantiate(snipperAtk.fireEffect, touchPos, Quaternion.identity, transform);

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(touchPos, snipperAtk.attackRange, enemyLayer);
        foreach(Collider2D hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponentInChildren<Enemy>();
            if(enemy != null)
            {
                enemy.TakeDamage(damage, knockbackPower);
            }
        }
    }
    public void DeActivateSkill()
    {
        Destroy(this.gameObject);
        GameObject.FindWithTag("GM")
                  .GetComponent<GameManager>().hud
                  .GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        Time.timeScale = 1f;
    }
    public void DragSkillFire()
    {
        StartCoroutine(ActiveInstantSkill());
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


    // 즉발 공격, 데미지 처리 공식을 변경해야할 수 있다.
    IEnumerator ActiveInstantSkill()
    {
        grenadeAtk.attackPoint.GetComponent<CapsuleCollider2D>().enabled = true;
        grenadeAtk.skillImpact.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        grenadeAtk.attackPoint.GetComponent<CapsuleCollider2D>().enabled = false;
    }

    IEnumerator ActiveContinuousSkill()
    {
        // 1초 공격 피해 (기존 Collider active 상태)
        yield return new WaitForSeconds(continuousAtk.enableTime);
        // 1초 이후 Collider Off
        gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        // 1초 대기 후 Collider On
        yield return new WaitForSeconds(continuousAtk.enableTime);
        gameObject.GetComponent<PolygonCollider2D>().enabled = true;

    }

    // 포격요청 Area 선언 및 시간 정지
    IEnumerator ActiveArea()
    {
        float elapsedTime = 0f;
        float startFillAmount = 0f;
        float targetFillAmount = 1.1f; // 목표 fillAmount

        Time.timeScale = snipperAtk.targetDeltaTime;

        while (elapsedTime < snipperAtk.castTime)
        {
            snipperAtk.fillImage.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, elapsedTime / snipperAtk.castTime);
            elapsedTime += Time.deltaTime * (1 / Time.timeScale); // Time.timeScale 고려
            yield return null; // 한 프레임 기다립니다.
        }
    }
}