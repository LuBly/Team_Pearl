using System.Collections;
using UnityEngine;

public class Boss0_Skill1 : Skill
{
    public EnemySkill enemySkill;
    public float skillDelay;
    private void OnEnable()
    {
        Debug.Log("skill1 active");
        StartCoroutine(ActiveSkill());
    }

    private IEnumerator ActiveSkill()
    {
        yield return new WaitForSeconds(skillDelay);
        EndSkill1();
    }

    private void EndSkill1()
    {
        enemySkill.isSkillActive = false;
        EndSkill();
        StopAllCoroutines();
        gameObject.SetActive(false);
    }


}
