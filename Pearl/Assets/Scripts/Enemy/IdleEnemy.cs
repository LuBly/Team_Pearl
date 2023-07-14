using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
방치화면 적 관련 스크립트입니다.
*/


public class IdleEnemy : MonoBehaviour
{
    public EnemyData[] enemyData;

    CharacterBase stat; // 캐릭터 스탯
    IngameGoods goods; // 현재 재화

    [Header ("시작 스테이지 (1-1~4 = 1~4, 2-1~4 = 5~8, 3-1~4 = 9~12)")]
    public int nowStage; // 현재 스테이지 (1-1~4 = 1~4, 2-1~4 = 5~8, 3-1~4 = 9~12)
    [Header ("시작 챕터(영향 X)")]
    public int nowChapter; //현재 챕터 (1, 2, 3)

    int enemyHp, enemyAtk, enemyRapid; // 상대 체력, 공격력, 공격속도
    uint rewardGold, rewardManaStone; // 보상 골드, 보상 마력석*/
    public TextMeshProUGUI nowStageText;
    float nowDmg; // 현재 공격력
    int nowHp; // 현재 체력

    public void Awake()
    {
        stat = GameObject.Find("MainChar").GetComponent<CharacterBase>();
        goods = GameObject.Find("Goods").GetComponent<IngameGoods>();
        NowHpSet();
        GunDmgSet();
        SetEnemy(nowStage);    
        StartCoroutine("CharAttackRoutine");
        StartCoroutine("EnemyAttackRoutine");
    }

    IEnumerator CharAttackRoutine() // 공격속도 마다 캐릭터 공격 설정
    {
        AttackEnemy();
        yield return new WaitForSecondsRealtime(stat.gunRapid);
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
        enemyHp = enemyHp - (int)nowDmg;
        if(enemyHp <= 0) 
        {
            goods.gold = goods.gold + rewardGold;
            goods.manaStone = goods.manaStone + rewardManaStone;
            SetEnemy(nowStage);
            NowHpSet();
        }
    }

    void AttackChar() // enemy 공격 함수
    {
        nowHp = nowHp - enemyAtk;
        if(nowHp <= 0)
        {
            nowStage = nowStage -1;
            NowHpSet();
            SetEnemy(nowStage);
        }
    }

    public void GunDmgSet() // 현재 공격력 세팅하는 함수, Stat창에서 ATK Upgrade시 업그레이드 적용.
    {
        nowDmg = ((float)stat.atk/100*stat.gunAtk) + stat.gunAtk;
    }

    public void NowHpSet() // 현재 체력 세팅하는 함수, stat창에서 HP Upgrade시 업그레이드 적용.
    {
        nowHp = stat.health;
    }

    void SetEnemy(int stage) //스테이지 별 상대 스펙 설정
    {
        switch(stage)
        {
            case 1:
                /*enemyHp = 1000;
                enemyAtk = 10;
                enemyRapid = 1;
                rewardGold = 10;
                rewardManaStone = 1;*/
                enemyHp = enemyData[0].enemyHp;
                enemyAtk = enemyData[0].enemyAtk;
                enemyRapid = enemyData[0].enemyRapid;
                rewardGold = enemyData[0].rewardGold;
                rewardManaStone = enemyData[0].rewardManaStone;
                nowStageText.text = "1 - 1";
                nowChapter = 1;
                break;
            case 2:
                /*enemyHp = 3000;
                enemyAtk = 50;
                enemyRapid = 1;
                rewardGold = 100;
                rewardManaStone = 10;*/
                enemyHp = enemyData[1].enemyHp;
                enemyAtk = enemyData[1].enemyAtk;
                enemyRapid = enemyData[1].enemyRapid;
                rewardGold = enemyData[1].rewardGold;
                rewardManaStone = enemyData[1].rewardManaStone;
                nowStageText.text = "1 - 2";
                nowChapter = 1;
                break;
            case 3:
                enemyHp = enemyData[2].enemyHp;
                enemyAtk = enemyData[2].enemyAtk;
                enemyRapid = enemyData[2].enemyRapid;
                rewardGold = enemyData[2].rewardGold;
                rewardManaStone = enemyData[2].rewardManaStone;
                nowStageText.text = "1 - 3";
                nowChapter = 1;
                break;
            case 4:
                enemyHp = enemyData[3].enemyHp;
                enemyAtk = enemyData[3].enemyAtk;
                enemyRapid = enemyData[3].enemyRapid;
                rewardGold = enemyData[3].rewardGold;
                rewardManaStone = enemyData[3].rewardManaStone;
                nowStageText.text = "1 - 4";
                nowChapter = 1;
                break;
            case 5:
                enemyHp = enemyData[4].enemyHp;
                enemyAtk = enemyData[4].enemyAtk;
                enemyRapid = enemyData[4].enemyRapid;
                rewardGold = enemyData[4].rewardGold;
                rewardManaStone = enemyData[4].rewardManaStone;
                nowStageText.text = "2 - 1";
                nowChapter = 2;
                break;
            case 6:
                nowStageText.text = "2 - 2";
                nowChapter = 2;
                break;
            case 7:
                nowStageText.text = "2 - 3";
                nowChapter = 2;
                break;
            case 8:
                nowStageText.text = "2 - 4";
                nowChapter = 2;
                break;
            case 9:
                nowStageText.text = "3 - 1";
                nowChapter = 3;
                break;
            case 10:
                nowStageText.text = "3 - 2";
                nowChapter = 3;
                break;
            case 11:
                nowStageText.text = "3 - 3";
                nowChapter = 3;
                break;
            case 12:
                nowStageText.text = "3 - 4";
                nowChapter = 3;
                break;
        }

    }

    public void Stoproutine() // 코루틴 전체 정지
    {
        StopAllCoroutines();
    }
}

[System.Serializable]
public class EnemyData
{
    [Header ("스테이지")]
    public int nowStage;
    [Header ("챕터")]
    public int nowChapter;
    [Header ("적 체력")]
    public int enemyHp;
    [Header ("적 공격력")]
    public int enemyAtk;
    [Header ("적 공격속도 (공격 주기입니다. ex)0.1 = 초당 10회")]
    public int enemyRapid;
    [Header ("보상 골드")]
    public uint rewardGold;
    [Header ("보상 마력석")]
    public uint rewardManaStone;
}
