using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatManagerTemp : MonoBehaviour
{
    public CharacterBase cBase;
    public TextMeshProUGUI statText, LvText, hpText, speedText;

    void Start()
    {
        StatUpdate();
        LvTextUpdate();
        HpTextUpdate();
        SpeedTextUpdate();
    }

    public void StatUpdate()
    {
        string nowGun;
        int advancedAtk;
        if(cBase.id / 100 == 1) 
        {
            nowGun = "돌격소총";
            advancedAtk = 1;
        }
        else if(cBase.id / 100 == 2) 
        {
            nowGun = "산탄총";
            advancedAtk = 3;
        }
        else
        {
            nowGun = "저격소총";
            advancedAtk = 6;
        }
        
        string nowGunAtk;
        if(cBase.gunAtk == 0) nowGunAtk = cBase.gunAtk2.ToString() + "%";
        else nowGunAtk = cBase.gunAtk.ToString();

        float totalAtk = cBase.gunSkill * advancedAtk * (1 + ((float)cBase.atk + (float)cBase.gunAtk2) / 100) + cBase.gunAtk;

        statText.text =
        "캐릭터 레벨 " + cBase.Lv + "\n\n" +
        "레벨 추가 공격력 " + cBase.atk + "%" +"\n\n" +
        nowGun + " 숙련도 " + cBase.gunSkill + "\n\n" +
        "숙련도 추가 공격력 +" + ((int)cBase.gunSkill * advancedAtk) + "\n\n" +
        "장착 " + nowGun + " 공격력 " + nowGunAtk + "\n\n" +
        "총 공격력 " + totalAtk +"\n\n" +
        "체력 " + cBase.health + "\n\n" +
        "이동속도 " + cBase.moveSpeed + "\n\n" +
        "치명타 확률 " + cBase.critical + "\n\n" +
        "치명타 데미지 " + cBase.criticalDmg;
    }

    void LvTextUpdate()
    {
        LvText.text =
        "<size=48>레벨</size> " + "\n" +
        "Lv." + cBase.Lv + "\n" +
        "공격력 +" + cBase.atk +"%";
    }
    
    void HpTextUpdate()
    {
        hpText.text =
        "<size=48>최대 체력</size> " + "\n" +
        "Lv." + cBase.health / 10 + "\n" +
        "최대 체력 +" + cBase.health;
    }

    void SpeedTextUpdate()
    {
        speedText.text =
        "<size=48>이동속도</size> " + "\n" +
        "Lv." + cBase.moveSpeed + "\n" +
        "이동속도 +" + cBase.moveSpeed;
    }

    public void BtnLvUpClick()
    {
        //보유 골드 확인
        //골드 부족 시 문구 출력
        cBase.Lv += 1;
        cBase.atk += 1;
        LvTextUpdate();
        StatUpdate();
    }

    public void BtnHPUpClick()
    {
        //보유 골드 확인
        //골드 부족 시 문구 출력
        cBase.health += 10;
        HpTextUpdate();
        StatUpdate();
    }

    public void BtnSpeedUpClick()
    {
        //보유 골드 확인
        //골드 부족 시 문구 출력
        cBase.moveSpeed += 1;
        SpeedTextUpdate();
        StatUpdate();
    }
}
