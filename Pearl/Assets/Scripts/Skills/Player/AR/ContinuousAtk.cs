using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ContinuousAtk : Skill
{
    [Header("활성화 시간")]
    public float enableTime;

    [Header("피해를 주는 시간 단위")]
    public float attackTerm;

    bool isSkillAttack = true;
    // Start is called before the first frame update
    void Start()
    {
        CheckStop();
        skillType = SkillType.continuousAttack;
        StartCoroutine(ActiveContinuousSkill());
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // 스킬 피격
        if (collision.CompareTag("Enemy"))
        {
            if(isSkillAttack)
                StartCoroutine(skillAttack(collision));
        }
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
        EndSkill();
    }

    IEnumerator ActiveContinuousSkill()
    {
        // 1초 공격 피해 (기존 Collider active 상태)
        yield return new WaitForSeconds(enableTime);
        // 1초 이후 Collider Off
        GetComponent<PolygonCollider2D>().enabled = false;
        // 1초 대기 후 Collider On
        yield return new WaitForSeconds(enableTime);
        GetComponent<PolygonCollider2D>().enabled = true;
    }
    IEnumerator skillAttack(Collider2D collision)
    {
        isSkillAttack = false;
        collision.GetComponentInParent<Enemy>().TakeDamage(damage, knockbackPower);
        yield return new WaitForSeconds(attackTerm);
        isSkillAttack = true;
    }
}
