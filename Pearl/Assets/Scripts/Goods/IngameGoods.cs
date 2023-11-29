using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
인 게임 내에서 사용되는 모든 재화 변수 관리하는 스크립트입니다.
*/

public class IngameGoods : MonoBehaviour
{
    public TextMeshProUGUI nowGold, nowManaStone, nowCrystal;
    public int gold; // 금화
    public int manaStone; // 마력석
    public int crystal; // 보석

    void Start()
    {
        GoodsUpdate();
    }

    public void GoodsUpdate()
    {
        nowGold.text = gold.ToString();
        nowCrystal.text = crystal.ToString();
        nowManaStone.text = manaStone.ToString();
    }
}
