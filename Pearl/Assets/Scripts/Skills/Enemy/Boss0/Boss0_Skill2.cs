using System.Collections;
using UnityEngine;

public class Boss0_Skill2 : Skill
{
    public EnemySkill enemySkill;
    public float skillDelay;
    private void OnEnable()
    {
        Debug.Log("skill2 active");
        StartCoroutine(ActiveSkill());
    }

    private IEnumerator ActiveSkill()
    {
        yield return new WaitForSeconds(skillDelay);
        EndSkill2();
    }

    private void EndSkill2()
    {
        enemySkill.isSkillActive = false;
        EndSkill();
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
}
