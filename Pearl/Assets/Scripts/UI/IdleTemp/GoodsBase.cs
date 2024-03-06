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
    public int gold;
    public int manaStone;
    public int crystal;

    public TextMeshProUGUI idleGold, idleManaStone, idleCrystal;

    public void GoodsUpdate()
    {
        idleGold.text = gold.ToString();
        idleManaStone.text = manaStone.ToString();
        idleCrystal.text = crystal.ToString();
    }
}
