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
    public TextMeshProUGUI NowLv_text, NowDMG_text, NowHP_text;
    int levelUpPoint, dmgUpPoint, hpUpPoint;

    void Start()
    {
        stat = GameObject.Find("Main_Char").GetComponent<CharacterBase>(); // 변수에 Main_Char 오브젝트의 CharacterBase 값 가져오기.
        levelUpPoint = stat.Lv - 1; // 레벨업 포인트 초기화
        dmgUpPoint = stat.ATK - 10; // 공격력업 포인트 초기화
        hpUpPoint = stat.Health - 200; //체력업 포인트 초기화
    }


    void Update()
    {
        NowLv_text.text = levelUpPoint.ToString();
        NowDMG_text.text = dmgUpPoint.ToString();
        NowHP_text.text = hpUpPoint.ToString();
    }

    public void BtnLvClick() // 레벨업 버튼 클릭
    {
        levelUpPoint = levelUpPoint + 1;
        stat.Lv = stat.Lv + 1;
    }

    public void BtnDMGClick() //공격력업 버튼 클릭
    {
        dmgUpPoint = dmgUpPoint + 1;
        stat.ATK = stat.ATK + 1;
    }

    public void BtnHPClick() //체력업 버튼 클릭
    {
        hpUpPoint = hpUpPoint + 1;
        stat.Health = stat.Health + 10;
    }
}
