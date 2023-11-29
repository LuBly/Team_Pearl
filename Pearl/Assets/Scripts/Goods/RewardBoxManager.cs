using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardBoxManager : MonoBehaviour
{
    public SaveManager sManager;
    public int boxGold, boxManaStone;
    IngameGoods goods;
    void Awake()
    {
        goods = GameObject.Find("Goods").GetComponent<IngameGoods>();
        boxGold = 0;
        boxManaStone = 0;
    }

    public void GetReward()
    {
        goods.gold += boxGold;
        goods.manaStone += boxManaStone;
        boxGold = 0;
        boxManaStone = 0;
        goods.GoodsUpdate();
        sManager.Save();
    }
}
