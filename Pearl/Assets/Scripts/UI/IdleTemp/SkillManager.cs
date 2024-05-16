using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

/*
스킬 숙련도 관련 스크립트입니다.
(Skill UI에 삽입)
*/

public class SkillManager : MonoBehaviour
{
    public GoodsBase gBase; // 재화
    public CharacterBase cBase; // 캐릭터 능력치
    public TextMeshProUGUI statText, nextText, resetText; // 가변 텍스트(현재 스텟, 다음 강화 스텟, 초기화 비용)
    public GameObject alert; // 첫 21강 강화 경고 창
    public int ARLv, SGLv, SRLv; // 각 총기 숙련도 레벨
    public bool firstTWup; // 첫 21강 확인 용 변수(21강 달성 이후, 초기화 해도 초기화 비용이 0이 되지 않게 하기 위함)

    int now; // 현재 총기(100 = 돌격 / 200 = 산탄 / 300 = 저격)
    int upgradeCost, refundCost, resetCost; // 업그레이드 가격, 환불될 마력석, 초기화 비용
    bool alertOn; // 알림창에서 강화 시도를 위한 bool 변수

    List<Dictionary<string, object>> skillLvTable; // 숙련도 레벨 시트

    void Start()
    {
        skillLvTable = CSVReader.Read("ProficiencyLevel");
    }

    void SkillPanelUpdate(int skill, string gun) // 숙련도 판넬 업데이트 함수
    {
        switch(gun)
        {
            case "돌격소총":
                statText.text = gun + " 공격력 +" + skill;
                break;
            case "산탄총":
                statText.text = gun + " 공격력 +" + skill * 3; // 산탄총은 스킬 레벨 * 3이 증가한 공격력
                break;
            case "저격소총":
                statText.text = gun + " 공격력 +" + skill * 6; // 저격소총은 스킬 레벨 * 6이 증가한 공격력
                break;
        }
        nextText.text = skillLvTable[skill][gun].ToString().Replace("%n", "\n"); // 다음 레벨 능력치 안내 텍스트 업데이트
    }

    public void BtnSkillClick() // 메인 화면에서 숙련도 버튼 클릭 화면 디폴트를 돌격소총으로 설정하기 위함
    {
        now = 100;
        SkillPanelUpdate(ARLv, "돌격소총");
    }

    public void BtnARClick() // 돌격소총 선택
    {
        string gun = EventSystem.current.currentSelectedGameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;
        now = 100;
        SkillPanelUpdate(ARLv, gun);
    }

    public void BtnSRClick() // 저격소총 선택
    {
        string gun = EventSystem.current.currentSelectedGameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;
        now = 300;
        SkillPanelUpdate(SRLv, gun);
    }

    public void BtnSGClick() // 산탄총 선택
    {
        string gun = EventSystem.current.currentSelectedGameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;
        now = 200;
        SkillPanelUpdate(SGLv, gun);
    }

    public void UpgradeSkill() // 숙련도 강화 버튼 클릭
    {
        if(now == 100) // 돌격소총일 경우
        {
            if(ARLv == 20 && !alertOn && !firstTWup) // 21강 시도 시, 경고 창 생성
            {
                alert.GetComponent<UIMove>().SetParentPos();
                ChangeAlertCondition();
            }
            else
            {
                ARLv++;
                cBase.SkillLvUpdate();
                if(ARLv > 20) firstTWup = true;
                SkillPanelUpdate(ARLv, "돌격소총");
            }
        }
        else if(now == 200) // 산탄총일 경우
        {
            if(SGLv == 20 && !alertOn && !firstTWup)
            {
                alert.GetComponent<UIMove>().SetParentPos();
                ChangeAlertCondition();
            }
            else
            {
                SGLv++;
                cBase.SkillLvUpdate();
                if(SGLv > 20) firstTWup = true;
                SkillPanelUpdate(SGLv, "산탄총");
            } 
        }
        else // 저격소총일 경우
        {
            if(SRLv == 20 && !alertOn && !firstTWup)
            {
                alert.GetComponent<UIMove>().SetParentPos();
                ChangeAlertCondition();
            }
            else
            {
                SRLv++;
                cBase.SkillLvUpdate();
                if(SRLv > 20) firstTWup = true;
                SkillPanelUpdate(SRLv, "저격소총");
            }
        }
    }

    public void BtnResetClick() // 리셋 버튼 클릭
    {
        if(now == 100)
        {
            resetCost = (int)skillLvTable[ARLv]["ResetCost"]; // 초기화 비용 불러오기
            if(!firstTWup) resetCost = 0; // 첫 21레벨 달성 이전까지는 초기화 비용 0
            resetText.text = "돌격소총의 숙련도를 초기화 하시겠습니까?" + "\n" + "초기화 비용 : " + resetCost + "<sprite=0>"; // 초기화 안내 문구 설정 sprite=0 : 크리스탈 이미지
        }
        else if(now == 200)
        {
            resetCost = (int)skillLvTable[SGLv]["ResetCost"];
            if(!firstTWup) resetCost = 0;
            resetText.text = "산탄총의 숙련도를 초기화 하시겠습니까?" + "\n" + "초기화 비용 : " + resetCost + "<sprite=0>";
        }
        else
        {
            resetCost = (int)skillLvTable[SRLv]["ResetCost"];
            if(!firstTWup) resetCost = 0;
            resetText.text = "저격소총의 숙련도를 초기화 하시겠습니까?" + "\n" + "초기화 비용 : " + resetCost + "<sprite=0>";
        }
    }

    public void ResetYesClick() // 리셋 확인 버튼 클릭
    {
        if(now == 100)
        {
            if(gBase.crystal >= resetCost)
            {
                refundCost = (int)skillLvTable[ARLv]["RefundCost"];
                ARLv = 0;
                cBase.SkillLvUpdate();
                gBase.crystal -= resetCost;
                gBase.manaStone += refundCost;
                SkillPanelUpdate(ARLv, "돌격소총");
            }
        }
        else if(now == 200)
        {
            if(gBase.crystal >= resetCost)
            {
                refundCost = (int)skillLvTable[SGLv]["RefundCost"];
                SGLv = 0;
                cBase.SkillLvUpdate();
                gBase.crystal -= resetCost;
                gBase.manaStone += refundCost;
                SkillPanelUpdate(SGLv, "산탄총");
            }
        }
        else
        {
            if(gBase.crystal >= resetCost)
            {
                refundCost = (int)skillLvTable[SRLv]["RefundCost"];
                SRLv = 0;
                cBase.SkillLvUpdate();
                gBase.crystal -= resetCost;
                gBase.manaStone += refundCost;
                SkillPanelUpdate(SRLv, "저격소총");
            }
        }
        
    }

    public void ChangeAlertCondition() // alertOn 컨디션 변경 함수
    {
        alertOn = alertOn ? false : true;
    }
}
