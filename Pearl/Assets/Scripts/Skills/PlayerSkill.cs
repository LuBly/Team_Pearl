using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkill : MonoBehaviour
{
    public GameObject database;

    public int[] skillIdxs = new int[3];

    public GameObject[] hideSkillButtons;
    public GameObject[] textPros;
    public TextMeshProUGUI[] hideSkillTimeTexts;
    public Image[] hideSkillImages;
    public Image[] SkillImages;

    private bool[] isCoolTime = { false, false, false };    // 스킬 사용 중인지 확인하기 위한 bool 변수
    private float[] getSkillTimes = { 0, 0, 0 };            // 다음 스킬 사용가능 시간까지 남은 시간

    NewSkillData dataSkill;
    private void Awake()
    {
        dataSkill = database.GetComponent<NewSkillData>();
    }
    void Start()
    {
        
        for (int i = 0; i < textPros.Length; i++)
        {
            hideSkillTimeTexts[i] = textPros[i].GetComponent<TextMeshProUGUI>();
            hideSkillButtons[i].SetActive(false);
        }
        for (int i = 0; i < 3; i++)
        {
            Debug.Log(skillIdxs[i]);
            SkillImages[i].sprite = dataSkill.skill[skillIdxs[i]].iconSprite;
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
        //getSkillTimes[skillIdx] = skillData.skillCoolTimes[skillIdx] + skillData.skillActiveTimes[skillIdx];
        isCoolTime[skillIdx] = true;
    }


    public void ActiveSkill(int skillIdx)
    {
        //skillController.ActiveSkill(skillIdx);
    }

    public void DragSkill(int skillIdx)
    {
        //skillController.DragSkill(skillIdx);
    }

    public void EndDragSkill(int skillIdx)
    {
        //skillController.EndDragSkill(skillIdx);
    }

    private void HideSkillCheck()
    {
        if (isCoolTime[0])
        {
            StartCoroutine(SkillTimeCheck(0));
        }

        if (isCoolTime[1])
        {
            StartCoroutine(SkillTimeCheck(1));
        }

        if (isCoolTime[2])
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

        if (getSkillTimes[skillIdx] > 0)
        {
            getSkillTimes[skillIdx] -= Time.deltaTime;

            if (getSkillTimes[skillIdx] < 0)
            {
                getSkillTimes[skillIdx] = 0;
                isCoolTime[skillIdx] = false;
                hideSkillButtons[skillIdx].SetActive(false);
            }

            hideSkillTimeTexts[skillIdx].text = getSkillTimes[skillIdx].ToString("00");

            //float time = getSkillTimes[skillIdx] / (skillData.skillCoolTimes[skillIdx] + skillData.skillActiveTimes[skillIdx]);
            //hideSkillImages[skillIdx].fillAmount = time;
        }
    }
}
