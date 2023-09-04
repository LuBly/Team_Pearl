using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
상점 시스템 관련 스크립트 입니다. 장비 소환 및 재화를 이용한 아이템 구매 등 전반적인 기능을 모두 담당합니다.
*/

public class ShopManager : MonoBehaviour
{
    IngameGoods goods;

    void Start()
    {
        goods = GameObject.Find("Goods").GetComponent<IngameGoods>();
    }

    public void OnBtn1Click() // 1회 소환
    {
        if(goods.crystal >= 100)
        {
            // 1회 소환 동작
        }
        else
        {
            // 크리스탈 부족 문구 출력.
        }
    }
}
