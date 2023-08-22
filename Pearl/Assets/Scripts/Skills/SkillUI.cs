using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillUI : MonoBehaviour
{
    public Weapon weapon;
    public GameObject[] hideSkillButtons;
    public GameObject[] textPros;
    public TextMeshProUGUI[] hideSkillTimeTexts;
    public Image[] hideSkillImages;
    public SkillData[] skillDatas;
    
    private bool[] isHideSkills = {false, false, false};    // 스킬 사용 중인지 확인하기 위한 bool 변수
    private float[] getSkillTimes = {0, 0, 0};              // 스킬 사용 이후 지난 시간
    private int weaponId;
    void Awake()
    {
        weaponId = weapon.gameObject.GetComponent<Weapon>().id;
    }
    void Start()
    {   
        for(int i = 0; i < textPros.Length; i++)
        {
            hideSkillTimeTexts[i] = textPros[i].GetComponent<TextMeshProUGUI>();
            hideSkillButtons[i].SetActive(false);
        }
        for(int i = 0; i < 3; i++)
        {
            skillDatas[weaponId].skills[i].SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        HideSkillCheck();
    }

    // 버튼을 눌렀을 때, 쿨타임이 도는 UI 활성화
    public void HideSkillSetting(int skillIdx)
    {
        hideSkillButtons[skillIdx].SetActive(true);
        getSkillTimes[skillIdx] = skillDatas[weaponId].skillCoolTimes[skillIdx];
        isHideSkills[skillIdx] = true;
    }

    
    public void ActiveSkill(int skillIdx)
    {
        if(isHideSkills[skillIdx])
        {
            Debug.Log(weaponId + " " + skillIdx);
        }
    }

    private void HideSkillCheck()
    {
        if(isHideSkills[0])
        {
            StartCoroutine(SkillTimeCheck(0));
        }

        if(isHideSkills[1])
        {
            StartCoroutine(SkillTimeCheck(1));
        }

        if(isHideSkills[2])
        {
            StartCoroutine(SkillTimeCheck(2));
        }
    }
    
    IEnumerator SkillTimeCheck(int skillIdx)
    {
        yield return null;

        if(getSkillTimes[skillIdx] > 0)
        {
            getSkillTimes[skillIdx] -= Time.deltaTime;

            if(getSkillTimes[skillIdx] < 0)
            {
                getSkillTimes[skillIdx] = 0;
                isHideSkills[skillIdx] = false;
                hideSkillButtons[skillIdx].SetActive(false);
            }

            hideSkillTimeTexts[skillIdx].text = getSkillTimes[skillIdx].ToString("00");

            float time = getSkillTimes[skillIdx] / skillDatas[weaponId].skillCoolTimes[skillIdx];
            hideSkillImages[skillIdx].fillAmount = time;
        }
    }
}

// 총기 종류별 스킬 쿨타임을 지정할 Data배열
[System.Serializable]
public class SkillData
{
    [Header("총기 종류별 스킬 쿨타임")]
    public float[] skillCoolTimes;             // 스킬의 총 쿨타임
    public GameObject[] skills;                // 활성화 할 게임 오브젝트
}
