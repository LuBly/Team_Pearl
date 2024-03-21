using System.Collections;
using UnityEngine;

public class Boss0_Skill3 : Skill
{
    public EnemySkill enemySkill;
    public float skillDelay;
    private void OnEnable()
    {
        Debug.Log("skill3 active");
        StartCoroutine(ActiveSkill());
    }

    private IEnumerator ActiveSkill()
    {
        yield return new WaitForSeconds(skillDelay);
        EndSkill3();
    }

    private void EndSkill3()
    {
        enemySkill.isSkillActive = false;
        EndSkill();
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
}
