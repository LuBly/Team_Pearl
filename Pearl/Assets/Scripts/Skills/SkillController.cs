using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스킬에 대한 전반적인 정보를 다루는 Script
public class SkillController : MonoBehaviour
{   
    public SkillData[] skillDatas;
    private int weaponId;

    private void Awake()
    {
        weaponId = DataManager.Instance.id;
    }
    
    public void ActiveSkill(int skillIdx)
    {
        // 선택된 스킬을 SkillCoolTime 만큼 켰다가 끄기
        // Coroutine 사용
        // 활성화 -> 지속시간만큼 대기 -> 비활성화 -> 쿨타임 활성화
        //skillDatas[weaponId].skills[skillIdx].SetActive(true);
        StopCoroutine(UseSkill(skillIdx));
        StartCoroutine(UseSkill(skillIdx));
    }
    
    
    IEnumerator UseSkill(int skillIdx)
    {
        skillDatas[weaponId].skills[skillIdx].SetActive(true);
        yield return new WaitForSeconds(skillDatas[weaponId].skillActiveTimes[skillIdx]);
        skillDatas[weaponId].skills[skillIdx].SetActive(false);
    }
}

// 총기 종류별 스킬 쿨타임을 지정할 Data배열
[System.Serializable]
public class SkillData
{
    [Header("총기 종류별 스킬 쿨타임")]
    public float[] skillCoolTimes;             // 스킬의 총 쿨타임
    public float[] skillActiveTimes;           // 스킬 지속시간
    public GameObject[] skillIcons;            // 활성화 할 게임 오브젝트(UI)
    public GameObject[] skills;                // 활성화 할 게임 오브젝트(Skill)
}
