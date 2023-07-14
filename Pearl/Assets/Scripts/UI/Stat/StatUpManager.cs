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
    IngameGoods goods;
    GameObject neg; // Not Enough Gold
    public TextMeshProUGUI nowLvText, nowDmgText, nowHpText, nowDmgLevelText, nowHpLevelText, lvGoldText, dmgGoldText, hpGoldText;
    uint levelUpPoint, dmgUpPoint, hpUpPoint;
    uint lvGold, dmgGold, hpGold;
    bool flag; //NEG 중복 발생 방지

    void Start()
    {
        stat = GameObject.Find("MainChar").GetComponent<CharacterBase>(); // 변수에 MainChar 오브젝트의 CharacterBase 값 가져오기.
        goods = GameObject.Find("Goods").GetComponent<IngameGoods>();
        neg = GameObject.Find("Neg");
        NegDisappear();
        levelUpPoint = (uint)stat.Lv - 1; // 레벨업 포인트 초기화
        dmgUpPoint = (uint)stat.atk; // 공격력업 포인트 초기화
        hpUpPoint = ((uint)stat.health - 200) % 10; //체력업 포인트 초기화
        LvGoldSetting();
        DmgGoldSetting();
        HpGoldSetting();
    }


    void Update()
    {
        nowLvText.text = levelUpPoint.ToString();
        nowDmgText.text = stat.atk.ToString() + "%";
        nowDmgLevelText.text = dmgUpPoint.ToString();
        nowHpLevelText.text = hpUpPoint.ToString();
        nowHpText.text = (hpUpPoint*10).ToString();
    }

    public void BtnLvClick() // 레벨업 버튼 클릭
    {
        if(goods.gold >= lvGold)
        {
            goods.gold = goods.gold - lvGold;
            levelUpPoint = levelUpPoint + 1;
            stat.Lv = stat.Lv + 1;
            LvGoldSetting();
        }
        else if(flag == false)
        {
            NegAppear();
            Invoke("NegDisappear", 1f);
        }
    }

    public void BtnDmgClick() //공격력업 버튼 클릭
    {
        if(goods.gold >= dmgGold)
        {
            goods.gold = goods.gold - dmgGold;
            dmgUpPoint = dmgUpPoint + 1;
            stat.atk = stat.atk + 1;
            DmgGoldSetting();
        }
        else if(flag == false)
        {
            NegAppear();
            Invoke("NegDisappear", 1f);
        }
    }

    public void BtnHpClick() //체력업 버튼 클릭
    {
        if(goods.gold >= hpGold)
        {
            goods.gold = goods.gold - hpGold;
            hpUpPoint = hpUpPoint + 1;
            stat.health = stat.health + 10;
            HpGoldSetting();
        }
        else if(flag == false)
        {
            NegAppear();
            Invoke("NegDisappear", 1f);
        }
    }

    void LvGoldSetting() //LV골드 세팅
    {
        lvGold = (levelUpPoint + 1) * 50;
        lvGoldText.text = lvGold.ToString();
    }

    void DmgGoldSetting() //DMG 골드 세팅
    {
        dmgGold = (dmgUpPoint + 1) * 50;
        dmgGoldText.text = dmgGold.ToString();
    }

    void HpGoldSetting() //HP 골드 세팅
    {
        hpGold = (hpUpPoint + 1) * 50;
        hpGoldText.text = hpGold.ToString();
    }

    void NegAppear() //NEG 보이게
    {
        flag = true;
        neg.SetActive(true);
    }

    void NegDisappear() //NEG 안보이게
    {
        flag = false;
        neg.SetActive(false);
    }
}
