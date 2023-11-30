using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;
/*
방치화면 적 관련 스크립트입니다.
*/


public class IdleEnemy : MonoBehaviour
{
    public SpriteRenderer nowEnemySprite; // 현재 적 이미지

    List<Dictionary<string, object>> enemyDataT;

    public CharacterBase stat; // 캐릭터 스탯
    public RewardBoxManager goods; // 현재 재화
    public BackGroundManager backGround; //현재 배경
    List<List<string>> chapter1 = new List<List<string>>(), chapter2 = new List<List<string>>(), chapter3 = new List<List<string>>();

    [Header ("시작 스테이지")]
    public int nowStage; // 현재 스테이지
    [Header ("시작 챕터")]
    public int nowChapter; //현재 챕터 (1, 2, 3)

    int enemyHp, enemyAtk, enemyRapid; // 상대 체력, 공격력, 공격속도
    int rewardGold, rewardManaStone; // 보상 골드, 보상 마력석*/
    int stageText; // 스테이지 텍스트 작성용 변수
    public TextMeshProUGUI nowStageText;
    float nowDmg; // 현재 공격력
    int nowHp; // 현재 체력

    void Awake()
    {
        nowEnemySprite = GetComponent<SpriteRenderer>();
        enemyDataT = CSVReader.Read("EnemyStatTable");
    }

    public void Start()
    {
        NowHpSet();
        GunDmgSet();
        EnemyDataRead();
        SetEnemyT(nowStage);
        backGround.BackImageChange(nowChapter); //배경 이미지 변경
        StartCoroutine("CharAttackRoutine");
        StartCoroutine("EnemyAttackRoutine");
    }

    IEnumerator CharAttackRoutine() // 공격속도 마다 캐릭터 공격 설정
    {
        AttackEnemy();
        yield return new WaitForSecondsRealtime(stat.gunRapid / 1000);
        StartCoroutine(CharAttackRoutine());
    }

    IEnumerator EnemyAttackRoutine() // 공격속도 마다 캐릭터 공격 설정
    {
        AttackChar();
        yield return new WaitForSecondsRealtime(enemyRapid / 1000);
        StartCoroutine(EnemyAttackRoutine());
    }

    void AttackEnemy() // 캐릭터 공격 함수
    {
        enemyHp = enemyHp - (int)nowDmg;
        if(enemyHp <= 0) 
        {
            goods.boxGold = goods.boxGold + rewardGold;
            goods.boxManaStone = goods.boxManaStone + rewardManaStone;
            SetEnemyT(nowStage);
            NowHpSet();
        }
    }

    void AttackChar() // enemy 공격 함수
    {
        nowHp = nowHp - enemyAtk;
        if(nowHp <= 0)
        {
            if(nowStage == 1) nowStage = 1; // 1-1이하로 감소 방지
            else 
            {
                nowStage = nowStage - 1;
            }
            NowHpSet();
            SetEnemyT(nowStage);
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

    void SetEnemyT(int stage)
    {
        switch(nowChapter)
        {
            case 1:
                enemyHp = Int32.Parse(chapter1[stage-1][2]);
                enemyAtk = Int32.Parse(chapter1[stage-1][3]);
                enemyRapid = Int32.Parse(chapter1[stage-1][4]);
                rewardGold = Int32.Parse(chapter1[stage-1][5]);
                rewardManaStone = Int32.Parse(chapter1[stage-1][6]);
                nowEnemySprite.sprite = Resources.Load<Sprite>("EnemyPrefabs/" + chapter1[stage-1][7]);
                break;
            case 2:
                enemyHp = Int32.Parse(chapter2[stage-1][2]);
                enemyAtk = Int32.Parse(chapter2[stage-1][3]);
                enemyRapid = Int32.Parse(chapter2[stage-1][4]);
                rewardGold = Int32.Parse(chapter2[stage-1][5]);
                rewardManaStone = Int32.Parse(chapter2[stage-1][6]);
                nowEnemySprite.sprite = Resources.Load<Sprite>("EnemyPrefabs/" + chapter2[stage-1][7]);
                break;
            case 3:
                enemyHp = Int32.Parse(chapter3[stage-1][2]);
                enemyAtk = Int32.Parse(chapter3[stage-1][3]);
                enemyRapid = Int32.Parse(chapter3[stage-1][4]);
                rewardGold = Int32.Parse(chapter3[stage-1][5]);
                rewardManaStone = Int32.Parse(chapter3[stage-1][6]);
                nowEnemySprite.sprite = Resources.Load<Sprite>("EnemyPrefabs/" + chapter3[stage-1][7]);
                break;
        }
        nowStageText.text = nowChapter.ToString() + " - " + nowStage.ToString() + " 소탕중"; // 챕터 - 스테이지
    }

    public void Stoproutine() // 코루틴 전체 정지
    {
        StopAllCoroutines();
    }

    void EnemyDataRead()
    {
        int count1 = 0, count2 = 0, count3 = 0;
        //Chapter 1 Write
        for(int i = 0; i < enemyDataT.Count; i++)
        {
            if(enemyDataT[i].First().Value.ToString() == "1") count1++;
        }
        for(int i = 0; i < count1; i++)
        {
            chapter1.Add(new List<string>());
            foreach(KeyValuePair<string, object> e in enemyDataT[i])
            {
                chapter1[i].Add(e.Value.ToString());
            }
        }
        //Chapter 2 Write
        count2 = count1;
        for(int i = count1; i < enemyDataT.Count; i++)
        {
            if(enemyDataT[i].First().Value.ToString() == "2") count2++;
        }
        for(int i = count1; i < count2; i++)
        {
            chapter2.Add(new List<string>());
            foreach(KeyValuePair<string, object> e in enemyDataT[i])
            {
                chapter2[i - count1].Add(e.Value.ToString());
            }
        }
        //Chapter 3 Write
        count3 = count2;
        for(int i = count2; i < enemyDataT.Count; i++)
        {
            if(enemyDataT[i].First().Value.ToString() == "3") count3++;
        }
        for(int i = count2; i < count3; i++)
        {
            chapter3.Add(new List<string>());
            foreach(KeyValuePair<string, object> e in enemyDataT[i])
            {
                chapter3[i - count2].Add(e.Value.ToString());
            }
        }
    }
}
