using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

/*
상점 시스템 관련 스크립트 입니다. 장비 소환 및 재화를 이용한 아이템 구매 등 전반적인 기능을 모두 담당합니다.
*/

public class SummonManager : MonoBehaviour
{
    public GoodsBase gBase; // 재화
    public InventoryManager iManager; // 인벤토리
    //public SaveManager sManager; // 세이브
    public TextMeshProUGUI cText, LvText; // 뽑기 횟수, 뽑기 레벨
    public UnityEngine.UI.Slider gLv; // 뽑기 횟수 게이지
    public string gachaLevel = "Lv1"; // 뽑기 레벨
    public int gachaCount = 0; // 뽑기 횟수
    public GameObject summonBox; // 뽑기 화면
    public GameObject layout; // 뽑기 레이아웃
    public GameObject nEnoughBox; // 재화 부족 화면

    List<Dictionary<string, object>> gachaT = new List<Dictionary<string, object>>(); // 뽑기 표
    List<string> grade = new List<string>(); // 등급
    List<int> Lv1 = new List<int>(); // 뽑기레벨 1 확률
    List<int> Lv2 = new List<int>(); // 뽑기레벨 2 확률
    List<int> Lv3 = new List<int>(); // 뽑기레벨 3 확률
    List<int> gLvPoint = new List<int> {100, 500}; // 뽑기 레벨 게이지 (1, 2)

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

    void TextUpdate() // 뽑기 레벨 텍스트 업데이트 함수
    {
        LvText.text = gachaLevel; // 현재 뽑기 레벨
        if(gachaLevel != "Lv3") // 3레벨(MAX레벨)이 아닐경우
        {
            cText.text = gachaCount.ToString() + " / " + gLvPoint[gachaLevel[2] - '1'].ToString(); // 뽑기 횟수 기록
            gLv.maxValue = gLvPoint[gachaLevel[2] - '1']; // 뽑기 횟수 게이지 설정
            gLv.minValue = 0;
            gLv.value = gachaCount;
        }
        else // 3레벨일 경우
        {
            cText.text = "LvMAX";
            gLv.maxValue = 1;
            gLv.minValue = 0;
            gLv.value = 1;
        }
        
    }

    public void ChargeCrystal() // 임시 함수
    {
        gBase.crystal += 30000;
        gBase.GoodsUpdate();
    }


    public void Btn1Click() // 1회 소환
    {
        int ran = Random.Range(1, 10001); // 1~10000까지 숫자를 이용한 확률 구현(소수점 두자릿수까지 표현 가능)
        string nGrade = "";
        List<string> nList = new List<string>(); // 현재 뽑기 후보 리스트(등급을 우선으로 뽑은 후, 해당 등급 장비 중 하나를 랜덤하게 뽑아서 구현)
        if(gBase.crystal >= 100)
        {
            NowSummonDestroy();
            summonBox.GetComponent<UIMove>().SetParentPos();
            layout.GetComponent<GridLayoutGroup>().childAlignment = TextAnchor.MiddleCenter;
            gBase.crystal -= 100;
            gBase.GoodsUpdate();
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
            for(int i = 0; i < iManager.gunData.Count; i++) // 총기 데이터 테이블에서 해당되는 등급 ID 다 갖고오기
            {
                if(iManager.gunData[i]["Grade"].ToString() == nGrade) nList.Add(iManager.gunData[i]["GunID"].ToString());
            }
            ran = Random.Range(0, nList.Count);
            for(int i = 0; i < iManager.gunData.Count; i++)
            {
                if(iManager.gunData[i]["GunID"].ToString() == nList[ran]) // 총기 데이터 테이블에서 결정된 총기 Prefab 가져오기
                {
                    GameObject prefab = Resources.Load<GameObject>("GunPrefab/" + iManager.gunData[i]["GunPrefab"].ToString());
                    GameObject summon = Instantiate(prefab); // 해당 총기 Prefab 생성 
                    summon.transform.SetParent(layout.transform); // 생성된 프리펩을 Layout 밑으로 보내기
                    break;
                }
            }
            iManager.gunList[nList[ran]] += 1; // 인벤토리 총기 수 증가
            gachaCount++; // 뽑기 횟수 증가
            if(gachaLevel[2] != '3' && gachaCount >= gLvPoint[gachaLevel[2] - '1']) // 뽑기 레벨이 최대가 아니고, 뽑기 횟수가 승급 횟수에 도달했을경우
            {
                gachaLevel = "Lv" + (gachaLevel[2] - '0' + 1).ToString();
                gachaCount = 0;
            }
            TextUpdate();
            //sManager.Save();
        }
        else
        {
            nEnoughBox.GetComponent<UIMove>().SetParentPos();// 크리스탈 부족 문구 출력.
        }
    }

    public void Btn10Click() // 10회 반복
    {
        if(gBase.crystal >= 1000)
        {
            NowSummonDestroy();
            summonBox.GetComponent<UIMove>().SetParentPos();
            layout.GetComponent<GridLayoutGroup>().childAlignment = TextAnchor.UpperLeft;
            for(int j = 0; j < 10; j++)
            {
                int ran = Random.Range(1, 10001);
                string nGrade = "";
                List<string> nList = new List<string>();
                gBase.crystal -= 100;
                gBase.GoodsUpdate();
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
                for(int i = 0; i < iManager.gunData.Count; i++)
                {
                    if(iManager.gunData[i]["GunID"].ToString() == nList[ran])
                    {
                        GameObject prefab = Resources.Load<GameObject>("GunPrefab/" + iManager.gunData[i]["GunPrefab"].ToString());
                        GameObject summon = Instantiate(prefab);
                        summon.transform.SetParent(layout.transform);
                        break;
                    }
                }
                iManager.gunList[nList[ran]] += 1;
                gachaCount++;
                if(gachaLevel[2] != '3' && gachaCount >= gLvPoint[gachaLevel[2] - '1'])
                {
                    gachaLevel = "Lv" + (gachaLevel[2] - '0' + 1).ToString();
                    gachaCount = 0;
                }
            }
            TextUpdate();
            //sManager.Save();
        } 
        else
        {
            nEnoughBox.GetComponent<UIMove>().SetParentPos();// 크리스탈 부족 문구 출력.
        }

    }

    public void Btn30Click() // 30회 반복
    {
        layout.GetComponent<GridLayoutGroup>().childAlignment = TextAnchor.UpperLeft;
        if(gBase.crystal >= 3000)
        {
            NowSummonDestroy();
            summonBox.GetComponent<UIMove>().SetParentPos();
            for(int j = 0; j < 30; j++)
            {
                int ran = Random.Range(1, 10001);
                string nGrade = "";
                List<string> nList = new List<string>();
                gBase.crystal -= 100;
                gBase.GoodsUpdate();
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
                for(int i = 0; i < iManager.gunData.Count; i++)
                {
                    if(iManager.gunData[i]["GunID"].ToString() == nList[ran])
                    {
                        GameObject prefab = Resources.Load<GameObject>("GunPrefab/" + iManager.gunData[i]["GunPrefab"].ToString());
                        GameObject summon = Instantiate(prefab);
                        summon.transform.SetParent(layout.transform);
                        break;
                    }
                }
                iManager.gunList[nList[ran]] += 1;
                gachaCount++;
                if(gachaLevel[2] != '3' && gachaCount >= gLvPoint[gachaLevel[2] - '1'])
                {
                    gachaLevel = "Lv" + (gachaLevel[2] - '0' + 1).ToString();
                    gachaCount = 0;
                }
            }
            TextUpdate();
            //sManager.Save();
        } 
        else
        {
            nEnoughBox.GetComponent<UIMove>().SetParentPos();// 크리스탈 부족 문구 출력.
        }

    }

    private void NowSummonDestroy() // 현재 LayOut 안에 소환된 오브젝트 모두 삭제
    {
        foreach(Transform child in layout.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
