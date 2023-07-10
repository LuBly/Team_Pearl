using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
방치화면 적 관련 스크립트입니다.
*/


public class IdleEnemy : MonoBehaviour
{
    CharacterBase stat;
    Ingame_Goods goods;
    public int nowStage;
    public int enemyHP;
    public uint rewardGold, rewardManaStone;

    void Start()
    {
        stat = GameObject.Find("Main_Char").GetComponent<CharacterBase>();
        goods = GameObject.Find("Goods").GetComponent<Ingame_Goods>();
        SetEnemy(nowStage);
    }

    void FixedUpdate()
    {
        enemyHP = enemyHP - (stat.ATK + stat.Gun_ATK);
        if(enemyHP <= 0) 
        {
            goods.Gold = goods.Gold + rewardGold;
            goods.Mana_Stone = goods.Mana_Stone + rewardManaStone;
            Start();
        }
    }

    void SetEnemy(int stage)
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
