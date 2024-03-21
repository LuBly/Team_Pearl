using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkill : MonoBehaviour
{

    [Header("위에 있을 수록(idx가 작을 수록) 스킬의 우선순위가 높습니다.")]
    public GameObject[] skills;
    public bool isSkillActive = false;

    private bool[] isCoolTime;
    private float[] getSkillTimes;
    private void Start()
    {
        isCoolTime = new bool[skills.Length];
        getSkillTimes = new float[skills.Length];
    }

    private void Update()
    {
        if (isSkillActive) return;
        
        // 시전 스킬 선택
        /*
         배열 인덱스가 낮을 수록 우선순위 높음.
         */
        for(int idx = 0; idx < skills.Length; idx++)
        {
            if (isCoolTime[idx]) continue;
            else
            {
                ActiveSkillSetting(idx);
                ActiveSkillCoolTime(idx);
                break;
            }
        }
    }
    private void ActiveSkillCoolTime(int idx)
    {
        if (isCoolTime[idx])
        {
            StartCoroutine(ActiveSkill(idx));
        }
    }

    private void ActiveSkillSetting(int skillIdx)
    {
        isSkillActive = skills[skillIdx].GetComponent<Skill>().isStop;
        skills[skillIdx].SetActive(true);
        getSkillTimes[skillIdx] = skills[skillIdx].GetComponent<Skill>().coolDown;
        isCoolTime[skillIdx] = true;
    }

    IEnumerator ActiveSkill(int skillIdx)
    {
        Debug.Log(skillIdx +" 쿨 시작");
        yield return new WaitForSecondsRealtime(getSkillTimes[skillIdx]);

        Debug.Log(skillIdx + " 쿨 끝");
        isCoolTime[skillIdx] = false;
    }
}
