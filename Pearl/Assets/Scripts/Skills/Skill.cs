using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    continuousAttack, // 지속 공격 ex) 제압사격
    quickAttack,      // 즉발 공격 ex) 수류탄
}
public class Skill : MonoBehaviour
{
    /// <summary>
    /// 활성화 되었을 때 스킬 발동
    /// 범위내에 있는 모든 적들에게 피해를 주는 방식
    /// -> 1초 공격(피해 0.2초 단위) -> 1초 비활성화 -> 1초 공격(피해 0.2초 단위)
    /// skillType      : 스킬의 종류
    /// -> 지속 공격, 단일 공격, 비네트 공격, 기본 공격 강화, etc
    /// enableTime     : 활성화 시간
    /// attackTime     : 피해를 주는 시간 단위
    /// damage         : 틱 당 데미지
    /// knockbackPower : 틱 당 넉백 정도
    /// </summary>
    public SkillType skillType;
    
    // 항상 사용
    public float damage;
    public float knockbackPower;

    // continuousAttack 에서만 사용
    public float enableTime;
    public float attackTime;
    private void OnEnable()
    {
        switch (skillType)
        {
            case SkillType.continuousAttack:
                StartCoroutine(activeSkill());
                break;
            case SkillType.quickAttack:
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
            case SkillType.quickAttack:
                break;
        }
    }
    IEnumerator activeSkill()
    {
        // 1초 공격 피해 (기존 Collider active 상태)
        yield return new WaitForSeconds(enableTime);
        // 1초 이후 Collider Off
        gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        // 1초 대기 후 Collider On
        yield return new WaitForSeconds(enableTime);
        gameObject.GetComponent<PolygonCollider2D>().enabled = true;

    }
}