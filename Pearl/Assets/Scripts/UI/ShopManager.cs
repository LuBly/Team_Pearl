using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
상점 시스템 관련 스크립트 입니다. 장비 소환 및 재화를 이용한 아이템 구매 등 전반적인 기능을 모두 담당합니다.
*/

public class ShopManager : MonoBehaviour
{
    public IngameGoods goods;
    public InventoryManager iManager;
    public SaveManager sManager;
    public TextMeshProUGUI cText, LvText;
    public Slider gLv;
    public string gachaLevel = "Lv1"; // 뽑기 레벨
    public int gachaCount = 0; // 뽑기 횟수

    List<Dictionary<string, object>> gachaT = new List<Dictionary<string, object>>(); // 뽑기 표
    List<string> grade = new List<string>(); // 등급
    List<int> Lv1 = new List<int>(); // 뽑기레벨 1 확률
    List<int> Lv2 = new List<int>(); // 뽑기레벨 2 확률
    List<int> Lv3 = new List<int>(); // 뽑기레벨 3 확률
    List<int> gLvPoint = new List<int> {100, 500};

    void Start()
    {
        gachaT = CSVReader.Read("GachaTable"); // 뽑기 표 읽어오기
        TextUpdate();

        for(int i = 0; i < gachaT.Count; i++)
        {
            grade.Add(gachaT[i]["Grade"].ToString());
            Lv1.Add((int)gachaT[i]["Lv1"]);
            Lv2.Add((int)gachaT[i]["Lv2"]);
            Lv3.Add((int)gachaT[i]["Lv3"]);
        }
    }

    void TextUpdate()
    {
        LvText.text = gachaLevel;
        if(gachaLevel != "Lv3")
        {
            cText.text = gachaCount.ToString() + " / " + gLvPoint[gachaLevel[2] - '1'].ToString();
            gLv.maxValue = gLvPoint[gachaLevel[2] - '1'];
            gLv.minValue = 0;
            gLv.value = gachaCount;
        }
        else
        {
            cText.text = "LvMAX";
            gLv.maxValue = 1;
            gLv.minValue = 0;
            gLv.value = 1;
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
        string nGrade = "";
        List<string> nList = new List<string>();
        if(goods.crystal >= 100)
        {
            goods.crystal -= 100;
            goods.GoodsUpdate();
            switch (gachaLevel)
            {
                case "Lv1":
                    for(int i = 0; i < Lv1.Count; i++)
                    {
                        if(Lv1[i] >= ran)
                        {
                            nGrade = grade[i];
                            break;   
                        }
                    }
                    break;
                case "Lv2":
                    for(int i = 0; i < Lv2.Count; i++)
                    {
                        if(Lv2[i] >= ran)
                        {
                            nGrade = grade[i];
                            break;   
                        }
                    }
                    break;
                case "Lv3":
                    for(int i = 0; i < Lv3.Count; i++)
                    {
                        if(Lv3[i] >= ran)
                        {
                            nGrade = grade[i];
                            break;   
                        }
                    }
                    break;
            }
            if(nGrade == "")
            {
                Debug.Log("버그 발생!"); // 예외처리 필요
                return;
            }
            for(int i = 0; i < iManager.gunData.Count; i++)
            {
                if(iManager.gunData[i]["Grade"].ToString() == nGrade) nList.Add(iManager.gunData[i]["GunID"].ToString());
            }
            ran = Random.Range(0, nList.Count);
            iManager.gunList[nList[ran]] += 1;
            gachaCount++;
            if(gachaLevel[2] != '3' && gachaCount >= gLvPoint[gachaLevel[2] - '1'])
            {
                gachaLevel = "Lv" + (gachaLevel[2] - '0' + 1).ToString();
                gachaCount = 0;
            }
            TextUpdate();
            sManager.Save();
        }
        else
        {
            // 크리스탈 부족 문구 출력.
        }
    }

    public void Btn10Click()
    {
        if(goods.crystal >= 1000)
        {
            for(int j = 0; j < 10; j++)
            {
                int ran = Random.Range(1, 10001);
                string nGrade = "";
                List<string> nList = new List<string>();
                goods.crystal -= 100;
                goods.GoodsUpdate();
                switch (gachaLevel)
                {
                    case "Lv1":
                        for(int i = 0; i < Lv1.Count; i++)
                        {
                            if(Lv1[i] >= ran)
                            {
                                nGrade = grade[i];
                                break;   
                            }
                        }
                        break;
                    case "Lv2":
                        for(int i = 0; i < Lv2.Count; i++)
                        {
                            if(Lv2[i] >= ran)
                            {
                                nGrade = grade[i];
                                break;   
                            }
                        }
                        break;
                    case "Lv3":
                        for(int i = 0; i < Lv3.Count; i++)
                        {
                            if(Lv3[i] >= ran)
                            {
                                nGrade = grade[i];
                                break;   
                            }
                        }
                        break;
                }
                if(nGrade == "")
                {
                    Debug.Log("버그 발생!"); // 예외처리 필요
                    return;
                }
                for(int i = 0; i < iManager.gunData.Count; i++)
                {
                    if(iManager.gunData[i]["Grade"].ToString() == nGrade) nList.Add(iManager.gunData[i]["GunID"].ToString());
                }
                ran = Random.Range(0, nList.Count);
                iManager.gunList[nList[ran]] += 1;
                gachaCount++;
                if(gachaLevel[2] != '3' && gachaCount >= gLvPoint[gachaLevel[2] - '1'])
                {
                    gachaLevel = "Lv" + (gachaLevel[2] - '0' + 1).ToString();
                    gachaCount = 0;
                }
            }
            TextUpdate();
            sManager.Save();
        } 
        else
        {
            //크리스탈 부족
        }

    }

    public void Btn30Click()
    {
        if(goods.crystal >= 3000)
        {
            for(int j = 0; j < 30; j++)
            {
                int ran = Random.Range(1, 10001);
                string nGrade = "";
                List<string> nList = new List<string>();
                goods.crystal -= 100;
                goods.GoodsUpdate();
                switch (gachaLevel)
                {
                    case "Lv1":
                        for(int i = 0; i < Lv1.Count; i++)
                        {
                            if(Lv1[i] >= ran)
                            {
                                nGrade = grade[i];
                                break;   
                            }
                        }
                        break;
                    case "Lv2":
                        for(int i = 0; i < Lv2.Count; i++)
                        {
                            if(Lv2[i] >= ran)
                            {
                                nGrade = grade[i];
                                break;   
                            }
                        }
                        break;
                    case "Lv3":
                        for(int i = 0; i < Lv3.Count; i++)
                        {
                            if(Lv3[i] >= ran)
                            {
                                nGrade = grade[i];
                                break;   
                            }
                        }
                        break;
                }
                if(nGrade == "")
                {
                    Debug.Log("버그 발생!"); // 예외처리 필요
                    return;
                }
                for(int i = 0; i < iManager.gunData.Count; i++)
                {
                    if(iManager.gunData[i]["Grade"].ToString() == nGrade) nList.Add(iManager.gunData[i]["GunID"].ToString());
                }
                ran = Random.Range(0, nList.Count);
                iManager.gunList[nList[ran]] += 1;
                gachaCount++;
                if(gachaLevel[2] != '3' && gachaCount >= gLvPoint[gachaLevel[2] - '1'])
                {
                    Debug.Log((gachaLevel[2] - '0' + 1).ToString());
                    gachaLevel = "Lv" + (gachaLevel[2] - '0' + 1).ToString();
                    gachaCount = 0;
                }
            }
            TextUpdate();
            sManager.Save();
        } 
        else
        {
            //크리스탈 부족
        }

    }
}
