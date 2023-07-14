using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
인게임 스탯 업그레이드 관련해서 관리하는 스크립트입니다.
*/

public class StatUpManager : MonoBehaviour
{
    CharacterBase stat;
    Ingame_Goods goods;
    GameObject NEG; // Not Enough Gold
    public TextMeshProUGUI NowLv_text, NowDMG_text, NowHP_text, NowDMGLevel_text, NowHpLevel_text, LVGoldText, DMGGoldText, HPGoldText;
    uint levelUpPoint, dmgUpPoint, hpUpPoint;
    uint LVGold, DMGGold, HPGold;
    bool flag; //NEG 중복 발생 방지

    void Start()
    {
        stat = GameObject.Find("Main_Char").GetComponent<CharacterBase>(); // 변수에 Main_Char 오브젝트의 CharacterBase 값 가져오기.
        goods = GameObject.Find("Goods").GetComponent<Ingame_Goods>();
        NEG = GameObject.Find("NEG");
        NEGDisappear();
        levelUpPoint = (uint)stat.Lv - 1; // 레벨업 포인트 초기화
        dmgUpPoint = (uint)stat.ATK; // 공격력업 포인트 초기화
        hpUpPoint = ((uint)stat.Health - 200) % 10; //체력업 포인트 초기화
        LVGoldSetting();
        DMGGoldSetting();
        HPGoldSetting();
    }


    void Update()
    {
        NowLv_text.text = levelUpPoint.ToString();
        NowDMG_text.text = stat.ATK.ToString() + "%";
        NowDMGLevel_text.text = dmgUpPoint.ToString();
        NowHpLevel_text.text = hpUpPoint.ToString();
        NowHP_text.text = (hpUpPoint*10).ToString();
    }

    public void BtnLvClick() // 레벨업 버튼 클릭
    {
        if(goods.Gold >= LVGold)
        {
            goods.Gold = goods.Gold - LVGold;
            levelUpPoint = levelUpPoint + 1;
            stat.Lv = stat.Lv + 1;
            LVGoldSetting();
        }
        else if(flag == false)
        {
            NEGAppear();
            Invoke("NEGDisappear", 1f);
        }
    }

    public void BtnDMGClick() //공격력업 버튼 클릭
    {
        if(goods.Gold >= DMGGold)
        {
            goods.Gold = goods.Gold - DMGGold;
            dmgUpPoint = dmgUpPoint + 1;
            stat.ATK = stat.ATK + 1;
            DMGGoldSetting();
        }
        else if(flag == false)
        {
            NEGAppear();
            Invoke("NEGDisappear", 1f);
        }
    }

    public void BtnHPClick() //체력업 버튼 클릭
    {
        if(goods.Gold >= HPGold)
        {
            goods.Gold = goods.Gold - HPGold;
            hpUpPoint = hpUpPoint + 1;
            stat.Health = stat.Health + 10;
            HPGoldSetting();
        }
        else if(flag == false)
        {
            NEGAppear();
            Invoke("NEGDisappear", 1f);
        }
    }

    void LVGoldSetting() //LV골드 세팅
    {
        LVGold = (levelUpPoint + 1) * 50;
        LVGoldText.text = LVGold.ToString();
    }

    void DMGGoldSetting() //DMG 골드 세팅
    {
        DMGGold = (dmgUpPoint + 1) * 50;
        DMGGoldText.text = DMGGold.ToString();
    }

    void HPGoldSetting() //HP 골드 세팅
    {
        HPGold = (hpUpPoint + 1) * 50;
        HPGoldText.text = HPGold.ToString();
    }

    void NEGAppear() //NEG 보이게
    {
        flag = true;
        NEG.SetActive(true);
    }

    void NEGDisappear() //NEG 안보이게
    {
        flag = false;
        NEG.SetActive(false);
    }
}
