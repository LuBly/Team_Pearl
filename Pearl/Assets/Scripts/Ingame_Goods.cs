using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
인 게임 내에서 사용되는 모든 재화 변수 관리하는 스크립트입니다.
*/

public class Ingame_Goods : MonoBehaviour
{
    public TextMeshProUGUI nowGold, nowManaStone, nowCrystal;
    public uint Gold; // 금화
    public uint Mana_Stone; // 마력석
    public uint Crystal; // 보석

    void Update()
    {
        nowGold.text = Gold.ToString();
        nowCrystal.text = Crystal.ToString();
        nowManaStone.text = Mana_Stone.ToString();
    }
}
