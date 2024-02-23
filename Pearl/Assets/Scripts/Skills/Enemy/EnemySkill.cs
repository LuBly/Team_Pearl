using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkill : MonoBehaviour
{
    public GameObject[] skills;
    [Header("위에 있을 수록(idx가 작을 수록) 스킬의 우선순위가 높습니다.")]
    public int[] prioritySkillIdxs;
    
    private bool[] isCoolTime;
    private float[] getSkillTimes;
    private bool isSkillActive = false;

    private void Start()
    {
        isCoolTime = new bool[skills.Length];
        getSkillTimes = new float[skills.Length];
    }

    private void Update()
    {
        if(!isSkillActive)
        {
            for(int idx = 0; idx < skills.Length; idx++)
            {
                if (skills[idx].GetComponent<Skill>().skillType == SkillType.rushAtk)
                {
                    if (true)//isPlayerInRange
                    {
                        ActiveSkillSetting(idx);
                        StartCoroutine(activeSkill(idx));
                        break;
                    }
                }
            }
        }
    }

    private void ActiveSkillSetting(int skillIdx)
    {
        skills[skillIdx].SetActive(true);
        getSkillTimes[skillIdx] = skills[skillIdx].GetComponent<Skill>().coolDown;
        isCoolTime[skillIdx] = true;
    }

    IEnumerator activeSkill(int skillIdx)
    {
        yield return null;

        if (getSkillTimes[skillIdx] > 0)
        {
            getSkillTimes[skillIdx] -= Time.deltaTime;

            if (getSkillTimes[skillIdx] < 0)
            {
                getSkillTimes[skillIdx] = 0;
                isCoolTime[skillIdx] = false;
            }
        }
    }
}
