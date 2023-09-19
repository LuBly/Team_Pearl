using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillUI : MonoBehaviour
{
    public GameObject[] hideSkillButtons;
    public GameObject[] textPros;
    public SkillController skillController;
    public TextMeshProUGUI[] hideSkillTimeTexts;
    public Image[] hideSkillImages;
    public SkillData skillData;
    
    private bool[] isHideSkills = {false, false, false};    // 스킬 사용 중인지 확인하기 위한 bool 변수
    private float[] getSkillTimes = {0, 0, 0};              // 스킬 사용 이후 지난 시간
    private int weaponId;                                   // 활성화할 무기의 종류 skillController에서 호출
    void Awake()
    {
        weaponId = DataManager.Instance.id;
    }
    void Start()
    {
        skillData = skillController.skillDatas[weaponId];
        for(int i = 0; i < textPros.Length; i++)
        {
            hideSkillTimeTexts[i] = textPros[i].GetComponent<TextMeshProUGUI>();
            hideSkillButtons[i].SetActive(false);
        }
        for(int i = 0; i < 3; i++)
        {
            skillData.skillIcons[i].SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        HideSkillCheck();
    }

    /// <summary>
    /// 버튼을 눌렀을 때, 쿨타임이 도는 UI 활성화 
    /// Skill Time : 쿨타임 + 스킬 지속 시간
    /// </summary>
    /// <param name="skillIdx"></param>
    public void HideSkillSetting(int skillIdx)
    {
        hideSkillButtons[skillIdx].SetActive(true);
        getSkillTimes[skillIdx] = skillData.skillCoolTimes[skillIdx] + skillData.skillActiveTimes[skillIdx];
        isHideSkills[skillIdx] = true;
    }

    
    public void ActiveSkill(int skillIdx)
    {
        if(isHideSkills[skillIdx])
        {
            skillController.ActiveSkill(skillIdx);
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
    /// <summary>
    /// 스킬 쿨타임 동안 Skill CoolDown을 나타내줄 UI
    /// </summary>
    /// <param name="skillIdx"></param>
    /// <returns></returns>
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

            float time = getSkillTimes[skillIdx] / skillData.skillCoolTimes[skillIdx] + skillData.skillActiveTimes[skillIdx];
            hideSkillImages[skillIdx].fillAmount = time;
        }
    }
}
