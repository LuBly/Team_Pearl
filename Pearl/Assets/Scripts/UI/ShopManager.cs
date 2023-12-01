using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
상점 시스템 관련 스크립트 입니다. 장비 소환 및 재화를 이용한 아이템 구매 등 전반적인 기능을 모두 담당합니다.
*/

public class ShopManager : MonoBehaviour
{
    public IngameGoods goods;
    public string gachaLevel = "Lv1"; // 뽑기 레벨
    public int gachaCount = 0; // 뽑기 횟수

    List<Dictionary<string, object>> gachaT = new List<Dictionary<string, object>>(); // 뽑기 표
    List<string> grade = new List<string>(); // 등급
    List<int> Lv1 = new List<int>(); // 뽑기레벨 1 확률
    List<int> Lv2 = new List<int>(); // 뽑기레벨 2 확률
    List<int> Lv3 = new List<int>(); // 뽑기레벨 3 확률

    void Start()
    {
        gachaT = CSVReader.Read("GachaTable"); // 뽑기 표 읽어오기

        for(int i = 0; i < gachaT.Count; i++)
        {
            grade.Add(gachaT[i]["Grade"].ToString());
            Lv1.Add((int)gachaT[i]["Lv1"]);
            Lv2.Add((int)gachaT[i]["Lv2"]);
            Lv3.Add((int)gachaT[i]["Lv3"]);
        }
    }

    public void ChargeCrystal() // 임시 함수
    {
        goods.crystal += 30000;
        goods.GoodsUpdate();
    }


    public void Btn1Click() // 1회 소환
    {
        int ran = Random.Range(1, 10001);
        if(goods.crystal >= 100)
        {
            goods.crystal -= 100;

        }
        else
        {
            // 크리스탈 부족 문구 출력.
        }
    }
}
