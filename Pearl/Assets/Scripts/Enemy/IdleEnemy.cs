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
    public Sprite[] enemySprite; // 적 이미지 배열
    public SpriteRenderer nowEnemySprite; // 현재 적 이미지

    CharacterBase stat; // 캐릭터 스탯
    IngameGoods goods; // 현재 재화
    BackGroundManager backGround; //현재 배경

    [Header ("시작 스테이지 (1-1~4 = 1~4, 2-1~4 = 5~8, 3-1~4 = 9~12)")]
    public int nowStage; // 현재 스테이지 (1-1~4 = 1~4, 2-1~4 = 5~8, 3-1~4 = 9~12)
    [Header ("시작 챕터(영향 X)")]
    public int nowChapter; //현재 챕터 (1, 2, 3)

    int enemyHp, enemyAtk, enemyRapid; // 상대 체력, 공격력, 공격속도
    uint rewardGold, rewardManaStone; // 보상 골드, 보상 마력석*/
    int stageText; // 스테이지 텍스트 작성용 변수
    public TextMeshProUGUI nowStageText;
    float nowDmg; // 현재 공격력
    int nowHp; // 현재 체력

    public void Awake()
    {
        stat = GameObject.Find("MainChar").GetComponent<CharacterBase>();
        goods = GameObject.Find("Goods").GetComponent<IngameGoods>();
        backGround = GameObject.Find("BackGround").GetComponent<BackGroundManager>();
        nowEnemySprite = GetComponent<SpriteRenderer>();
    }

    public void Start()
    {
        NowHpSet();
        GunDmgSet();
        SetEnemy(nowStage);
        backGround.BackImageChange(nowChapter); //배경 이미지 변경
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
        enemyHp = enemyData[stage - 1].enemyHp; // 적 체력 설정
        enemyAtk = enemyData[stage - 1].enemyAtk; //적 공격력 설정
        enemyRapid = enemyData[stage - 1].enemyRapid; // 적 공격속도 설정
        rewardGold = enemyData[stage - 1].rewardGold; // 적 보상 골드 설정
        rewardManaStone = enemyData[stage - 1].rewardManaStone; // 적 보상 마력석 설정
        nowEnemySprite.sprite = enemySprite[stage - 1]; // 적 스프라이트 변경
        if(stage >=1 && stage <=4)
        {
            nowChapter = 1;
            stageText = stage;
        } 
        else if(stage >=5 && stage <=8)
        {
            nowChapter = 2;
            stageText = stage - 4;
        }
        else if(stage >=9 && stage <=12)
        {
            nowChapter = 3;
            stageText = stage - 8;
        }
        else nowChapter = 99; // Error!
        nowStageText.text = nowChapter.ToString() + " - " + stageText.ToString(); // 챕터 - 스테이지
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
