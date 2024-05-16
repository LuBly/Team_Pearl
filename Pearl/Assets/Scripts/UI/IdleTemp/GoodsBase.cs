using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
인게임 재화(골드, 마력석, 크리스탈) 관리 스크립트입니다.
MainDisplay에 삽입

해당 스크립트와 CharacterBase 스크립트는 싱글톤으로 작성 고민 중...
*/

public class GoodsBase : MonoBehaviour
{
    public int gold; // 골드
    public int manaStone; // 마력석
    public int crystal; // 크리스탈

    public TextMeshProUGUI idleGold, idleManaStone, idleCrystal, summonCrystal, summonCrystal2, shopCrystal, statGold; // 재화 텍스트

    public void GoodsUpdate() // 재화 업데이트 함수
    {
        idleGold.text = gold.ToString();
        statGold.text = gold.ToString();
        idleManaStone.text = manaStone.ToString();
        idleCrystal.text = crystal.ToString();
        summonCrystal.text = crystal.ToString();
        summonCrystal2.text = crystal.ToString();
        shopCrystal.text = crystal.ToString();
    }
}
