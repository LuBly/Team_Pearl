using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerSkill : MonoBehaviour
{
    public GameObject database;

    [Header("Active Skill")]
    public int[] skillIdxs = new int[3];
    
    [Header("Auto Fire Skill")]
    public int[] autoSkillIdxs = new int[2];
    
    public GameObject[] skillFolder;
    public GameObject[] skillButtons;
    public GameObject[] hideSkillButtons;
    public GameObject[] textPros;
    public TextMeshProUGUI[] hideSkillTimeTexts;
    public Image[] hideSkillImages;
    public Image[] skillImages;
    public Transform attackPoint;
    public GameObject hud;
    public Transform myCanvas;

    private bool[] isCoolTime = { false, false, false };    // 스킬 사용 중인지 확인하기 위한 bool 변수
    private float[] getSkillTimes = { 0, 0, 0 };            // 다음 스킬 사용가능 시간까지 남은 시간
    private JoystickMovement[] grenadeJoysticks = new JoystickMovement[3];
    private GameObject curSkill;
    SkillData dataSkill;
    private void Awake()
    {
        dataSkill = database.GetComponent<SkillData>();
    }
    void Start()
    {
        try
        {
            for (int i = 0; i < autoSkillIdxs.Length; i++)
            {
                Instantiate(dataSkill.skill[autoSkillIdxs[i]].skillPrefabs, this.transform);
            }
        }
        catch
        {
            Debug.LogError("Skill Index Error, Skill Idx를 DB와 교차검증해봐주세요.");
        }
    
        // 이벤트 트리거 생성
        for (int i = 0; i < skillButtons.Length; i++)
        {
            EventTrigger eventTrigger = skillButtons[i].AddComponent<EventTrigger>();
            int index = i;
            EventTrigger.Entry entry_PointerClick = new EventTrigger.Entry();
            entry_PointerClick.eventID = EventTriggerType.PointerClick;
            entry_PointerClick.callback.AddListener((data) => { OnPointerClick((PointerEventData)data, index); });
            eventTrigger.triggers.Add(entry_PointerClick);

            if (dataSkill.skill[skillIdxs[i]].skillType == SkillType.grenadeAttack)
            {
                Instantiate(dataSkill.skill[skillIdxs[i]].skillUI, skillFolder[i].transform);
                skillFolder[i].AddComponent<JoystickMovement>();
                grenadeJoysticks[i] = skillFolder[i].GetComponent<JoystickMovement>();
                skillFolder[i].tag = "Skill_Grenade";

                EventTrigger.Entry entry_BeginDrag = new EventTrigger.Entry();
                entry_BeginDrag.eventID = EventTriggerType.BeginDrag;
                entry_BeginDrag.callback.AddListener((data) => { OnBeginDrag((PointerEventData)data, index); });
                eventTrigger.triggers.Add(entry_BeginDrag);

                EventTrigger.Entry entry_Drag = new EventTrigger.Entry();
                entry_Drag.eventID = EventTriggerType.Drag;
                entry_Drag.callback.AddListener((data) => { OnDrag((PointerEventData)data, index); });
                eventTrigger.triggers.Add(entry_Drag);

                EventTrigger.Entry entry_EndDrag = new EventTrigger.Entry();
                entry_EndDrag.eventID = EventTriggerType.EndDrag;
                entry_EndDrag.callback.AddListener((data) => { OnEndDrag((PointerEventData)data, index); });
                eventTrigger.triggers.Add(entry_EndDrag);
            }
        }

        // 아이콘 설정
        for (int i = 0; i < textPros.Length; i++)
        {
            hideSkillTimeTexts[i] = textPros[i].GetComponent<TextMeshProUGUI>();
            hideSkillButtons[i].SetActive(false);
        }
        for (int i = 0; i < 3; i++)
        {
            skillImages[i].sprite = dataSkill.skill[skillIdxs[i]].iconSprite;
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
        getSkillTimes[skillIdx] = dataSkill.skill[skillIdxs[skillIdx]].coolDown / 1000;
        isCoolTime[skillIdx] = true;
    }


    public void ActiveSkill(int skillIdx)
    {
        /*
         * SkillType에 따른 위치 조정이 필요하다.
         * Continuous Attack의 경우 Player 위치에서 바로 생성되면 된다.
         * Grenade Attack의 경우
         *  1. 그냥 단순 터치 = 최단거리의 몬스터(범위 내)
         *  2. 드래그 = 범위 내 직접 지정
         */
        // 아직 미개발된 내용일 경우
        if(dataSkill.skill[skillIdxs[skillIdx]].skillPrefabs == null)
        {
            Debug.Log(dataSkill.skill[skillIdxs[skillIdx]].sendMsg);
            return;
        }

        if (dataSkill.skill[skillIdxs[skillIdx]].skillType == SkillType.snipperAttack)
        {
            Instantiate(dataSkill.skill[skillIdxs[skillIdx]].skillPrefabs, myCanvas);
        }
        else
        {
            GameObject cpSkillPref = Instantiate(dataSkill.skill[skillIdxs[skillIdx]].skillPrefabs, attackPoint);
            Destroy(cpSkillPref, dataSkill.skill[skillIdxs[skillIdx]].activeTime / 1000f);
        }
    }

    public void BeginDragSkill(int skillIdx)
    {
        curSkill = Instantiate(dataSkill.skill[skillIdxs[skillIdx]].skillPrefabs, this.transform);
    }

    public void EndDragSkill(int skillIdx)
    {
        curSkill.GetComponent<GrenadeAtk>().DragSkillFire();
        Destroy(curSkill, dataSkill.skill[skillIdxs[skillIdx]].activeTime / 1000f);
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

            float time = getSkillTimes[skillIdx] / (dataSkill.skill[skillIdxs[skillIdx]].coolDown / 1000);
            hideSkillImages[skillIdx].fillAmount = time;
        }
    }

    void OnPointerClick(PointerEventData data, int idx)
    {
        // 다른 게임 조작이 진행중일 경우
        if (grenadeJoysticks[idx] != null && grenadeJoysticks[idx].isDrag)
        {
            Debug.Log("Now Dragging, no Fire");
            return;
        }
        
        ActiveSkill(idx);
        HideSkillSetting(idx);
        if(dataSkill.skill[skillIdxs[idx]].skillType == SkillType.snipperAttack)
        {
            hud.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        }
    }
    void OnBeginDrag(PointerEventData data, int idx)
    {
        BeginDragSkill(idx);
    }

    void OnDrag(PointerEventData data, int idx)
    {
        grenadeJoysticks[idx].Drag(data);
    }

    void OnEndDrag(PointerEventData data, int idx)
    {
        grenadeJoysticks[idx].EndDrag();
        EndDragSkill(idx);
        HideSkillSetting(idx);
    }
}
