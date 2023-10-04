using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.TextCore.Text;

[Serializable]
public class ContinuousAtk
{
    /// <summary>
    /// 활성화 되었을 때 스킬 발동
    /// 범위내에 있는 모든 적들에게 피해를 주는 방식
    /// -> 1초 공격(피해 0.2초 단위) -> 1초 비활성화 -> 1초 공격(피해 0.2초 단위)
    /// enableTime     : 활성화 시간
    /// attackTime     : 피해를 주는 시간 단위
    public float enableTime;
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

public enum SkillType
{
    continuousAttack, // 지속 공격 ex) 제압사격
    grenadeAttack,    // 즉발 공격 ex) 수류탄
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
    // 항상 사용
    public float damage;
    public float knockbackPower;
    [HideInInspector] public float attackTime;  

    private void FixedUpdate()
    {
        if (skillType == SkillType.grenadeAttack && grenadeAtk.skillJoystickMovement.isDrag == true)
        {
            // 방향 (JoyVec)
            // 크기 L2 = L1*R/r
            grenadeAtk.attackPoint.transform.position = 
                grenadeAtk.skillRange.transform.position 
                + grenadeAtk.skillJoystickMovement.joyVec 
                * grenadeAtk.skillJoystickMovement.stickDistance
                / 20f;
        }
        
    }

    private void OnEnable()
    {
        switch (skillType)
        {
            case SkillType.continuousAttack:
                attackTime = continuousAtk.attackTime;
                StartCoroutine(activeSkill());
                break;
            case SkillType.grenadeAttack:
                bool isDrag = grenadeAtk.skillJoystickMovement.isDrag;
                // Drag 공격
                if (isDrag)
                {
                    Debug.Log("Drag");
                    grenadeAtk.skillRange.SetActive(true);
                    grenadeAtk.attackPoint.SetActive(true);
                    grenadeAtk.attackPoint.GetComponent<CapsuleCollider2D>().enabled = false;
                }

                // 일반 공격
                else
                {
                    if (grenadeAtk.scanner.nearestTarget)
                    {
                        Debug.Log("idle");
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
        }
        
    }
    private void OnDisable()
    {
        switch (skillType)
        {
            case SkillType.continuousAttack:
                StopCoroutine(activeSkill());
                break;
            case SkillType.grenadeAttack:
                grenadeAtk.attackPoint.SetActive(false);
                grenadeAtk.skillImpact.SetActive(false);
                grenadeAtk.skillRange.SetActive(false);
                StopCoroutine(activeInstantSkill());
                grenadeAtk.attackPoint.GetComponent<CapsuleCollider2D>().enabled = true;
                break;
        }
    }

    public void DragSkillFire()
    {
        Debug.Log("DragSkillAttack");
        StartCoroutine(activeInstantSkill());
    }

    IEnumerator activeInstantSkill()
    {
        grenadeAtk.attackPoint.GetComponent<CapsuleCollider2D>().enabled = true;
        grenadeAtk.skillImpact.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        grenadeAtk.attackPoint.GetComponent<CapsuleCollider2D>().enabled = false;
    }

    IEnumerator activeSkill()
    {
        // 1초 공격 피해 (기존 Collider active 상태)
        yield return new WaitForSeconds(continuousAtk.enableTime);
        // 1초 이후 Collider Off
        gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        // 1초 대기 후 Collider On
        yield return new WaitForSeconds(continuousAtk.enableTime);
        gameObject.GetComponent<PolygonCollider2D>().enabled = true;

    }
}