using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
방치화면 적 관련 스크립트입니다.
*/


public class IdleEnemy : MonoBehaviour
{
    CharacterBase stat; // 캐릭터 스탯
    Ingame_Goods goods; // 현재 재화
    public int nowStage; // 현재 스테이지 (1-1~5 = 1~5, 2-1~5 = 6~10, 3-1~5 = 10~15)
    public int enemyHP, enemyATK, enemyRapid; // 상대 체력, 공격력, 공격속도
    public uint rewardGold, rewardManaStone; // 보상 골드, 보상 마력석
    public TextMeshProUGUI nowStageText;
    float now_DMG; // 현재 공격력
    int nowHP; // 현재 체력
    bool flag = true; // 임시 플래그

    void Start()
    {
        stat = GameObject.Find("Main_Char").GetComponent<CharacterBase>();
        goods = GameObject.Find("Goods").GetComponent<Ingame_Goods>();
        NowHP_Set();
        Gun_DMG_Set();
        SetEnemy(nowStage);    
        StartCoroutine(CharAttackRoutine());
        StartCoroutine(EnemyAttackRoutine());
    }

    IEnumerator CharAttackRoutine() // 공격속도 마다 캐릭터 공격 설정
    {
        AttackEnemy();
        yield return new WaitForSecondsRealtime(stat.Gun_Rapid);
        StartCoroutine(CharAttackRoutine());
    }

    IEnumerator EnemyAttackRoutine() // 공격속도 마다 캐릭터 공격 설정
    {
        AttackChar();
        yield return new WaitForSecondsRealtime(enemyRapid);
        StartCoroutine(EnemyAttackRoutine());
    }

    void AttackEnemy() // 캐릭터 공격 함수
    {
        enemyHP = enemyHP - (int)now_DMG;
        if(enemyHP <= 0) 
        {
            goods.Gold = goods.Gold + rewardGold;
            goods.Mana_Stone = goods.Mana_Stone + rewardManaStone;
            SetEnemy(nowStage);
            NowHP_Set();
        }
    }

    void AttackChar() // enemy 공격 함수
    {
        Debug.Log(nowHP);
        nowHP = nowHP - enemyATK;
        if(nowHP <= 0)
        {
            nowStage = nowStage -1;
            NowHP_Set();
            SetEnemy(nowStage);
        }
    }

    public void Gun_DMG_Set() // 현재 공격력 세팅하는 함수, Stat창에서 ATK Upgrade시 업그레이드 적용.
    {
        now_DMG = ((float)stat.ATK/100*stat.Gun_ATK) + stat.Gun_ATK;
    }

    public void NowHP_Set() // 현재 체력 세팅하는 함수, stat창에서 HP Upgrade시 업그레이드 적용.
    {
        nowHP = stat.Health;
    }

    void SetEnemy(int stage) //스테이지 별 상대 스펙 설정
    {
        switch(stage)
        {
            case 1:
                enemyHP = 1000;
                enemyATK = 10;
                enemyRapid = 1;
                rewardGold = 10;
                rewardManaStone = 1;
                nowStageText.text = "1 - 1";
                break;
            case 2:
                enemyHP = 1000;
                enemyATK = 50;
                enemyRapid = 1;
                rewardGold = 10;
                rewardManaStone = 1;
                nowStageText.text = "1 - 2";
                break;
            case 3:
                nowStageText.text = "1 - 3";
                break;
            case 4:
                nowStageText.text = "1 - 4";
                break;
            case 5:
                nowStageText.text = "1 - 5";
                break;
            case 6:
                nowStageText.text = "2 - 1";
                break;
            case 7:
                nowStageText.text = "2 - 2";
                break;
            case 8:
                nowStageText.text = "2 - 3";
                break;
            case 9:
                nowStageText.text = "2 - 4";
                break;
            case 10:
                nowStageText.text = "2 - 5";
                break;
            case 11:
                nowStageText.text = "3 - 1";
                break;
            case 12:
                nowStageText.text = "3 - 2";
                break;
            case 13:
                nowStageText.text = "3 - 3";
                break;
            case 14:
                nowStageText.text = "3 - 4";
                break;
            case 15:
                nowStageText.text = "3 - 5";
                break;
        }

    }
}
