using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArSkill0 : MonoBehaviour
{
    /// <summary>
    /// 활성화 되었을 때 스킬 발동
    /// 범위내에 있는 모든 적들에게 피해를 주는 방식
    /// -> 1초 공격(피해 0.2초 단위) -> 1초 비활성화 -> 1초 공격(피해 0.2초 단위)
    /// enableTime     : 활성화 시간
    /// attackTime     : 피해를 주는 시간 단위
    /// damage         : 틱 당 데미지
    /// knockbackPower : 틱 당 넉백 정도
    /// </summary>
    public float enableTime = 1f;
    public float attackTime = 0.2f;
    public float damage = 1f;
    public float knockbackPower = 0.5f;
    private void OnEnable()
    {
        StartCoroutine(Skill());
    }
    private void OnDisable()
    {
        StopCoroutine(Skill());
    }
    IEnumerator Skill()
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