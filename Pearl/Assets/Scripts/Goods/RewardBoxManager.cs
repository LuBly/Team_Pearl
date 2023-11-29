using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardBoxManager : MonoBehaviour
{
    public int boxGold, boxManaStone;
    bool isSave; // 세이브 데이터 확인 변수(임시)
    IngameGoods goods;
    void Awake()
    {
        goods = GameObject.Find("Goods").GetComponent<IngameGoods>();
        if(!isSave)
        {
            boxGold = 0;
            boxManaStone = 0;
        }
    }

    public void GetReward()
    {
        goods.gold += boxGold;
        goods.manaStone += boxManaStone;
        boxGold = 0;
        boxManaStone = 0;
    }
}
