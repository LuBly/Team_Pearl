using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
방치화면 적 관련 스크립트입니다.
*/


public class IdleEnemy : MonoBehaviour
{
    CharacterBase stat; // 캐릭터 스탯
    Ingame_Goods goods; // 현재 재화
    public int nowStage; // 현재 스테이지
    public int enemyHP; // 상대 체력
    public uint rewardGold, rewardManaStone; // 보상 골드, 보상 마력석

    void Start()
    {
        stat = GameObject.Find("Main_Char").GetComponent<CharacterBase>();
        goods = GameObject.Find("Goods").GetComponent<Ingame_Goods>();
        SetEnemy(nowStage);
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine() // 1초마다 몬스터 공격 설정
    {
        AttackEnemy();
        StartCoroutine(AttackRoutine());
        yield return new WaitForSecondsRealtime(1f);
    }

    void AttackEnemy() // 상대 공격 함수
    {
        enemyHP = enemyHP - (stat.ATK + stat.Gun_ATK);
        if(enemyHP <= 0) 
        {
            goods.Gold = goods.Gold + rewardGold;
            goods.Mana_Stone = goods.Mana_Stone + rewardManaStone;
            SetEnemy(nowStage);
        }
    }

    void SetEnemy(int stage) //스테이지 별 상대 스펙 설정
    {
        switch(stage)
        {
            case 101:
                enemyHP = 100;
                rewardGold = 10;
                rewardManaStone = 1;
                break;
            case 102:
                break;
            case 103:
                break;
            case 104:
                break;
            case 105:
                break;
            case 201:
                break;
            case 202:
                break;
            case 203:
                break;
            case 204:
                break;
            case 205:
                break;
            case 301:
                break;
            case 302:
                break;
            case 303:
                break;
            case 304:
                break;
            case 305:
                break;
        }

    }
}
