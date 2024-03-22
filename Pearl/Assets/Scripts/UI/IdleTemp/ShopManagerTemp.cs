using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
상점 기능 관리 스크립트입니다(ShopUI 삽입)
*/

public class ShopManagerTemp : MonoBehaviour
{
    public GameObject pScroll, cScroll;
    public GoodsBase gBase;
    public List<GameObject> Lcategory;
    int category = 0;// 카테고리(0 : 패키지 1 : 보석 충전)

    List<Dictionary<string, object>> cashTable;

    private void Awake()
    {
        cashTable = CSVReader.Read("CashTable");
    }

    private void LoadProduct() // 상품 목록 불러오기
    {
        for(int i = 0; i < cashTable.Count; i++)
        {
            if((int)cashTable[i]["DisplayCategory"] == category)
            {
                GameObject prefab = Resources.Load<GameObject>("Product/Product"); // 제품 이미지 기본 모델
                GameObject product = Instantiate(prefab); // 제품 프리펩 생성
                product.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Product/" + cashTable[i]["ProductIcon"]); // 제품 아이콘 설정
                product.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = cashTable[i]["ProductDesc"].ToString().Replace("%n", "\n"); // 제품 설명 작성
                product.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = cashTable[i]["Price"].ToString() + "원"; // 제품 가격 작성
                product.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(BtnBuyClick); // 구매 버튼 기능 설정
                product.name = cashTable[i]["ProductID"].ToString(); // 제품 이름 변경
                product.transform.SetParent(Lcategory[category].transform); // 생성된 프리펩을 해당 카테고리 밑으로 보내기
            }
        }
    }

    private void BtnBuyClick()
    {
        Debug.Log("결제 창 출력");
        //결제 완료 시
    }

    public void RemoveProduct() // 로딩 된 상점 데이터 비우기
    {
        for(int i = 0; i < Lcategory.Count; i++)
        {
            foreach(Transform child in Lcategory[i].transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void BtnPackageClick() // 패키지 목록 불러오기
    {
        pScroll.SetActive(true);
        cScroll.SetActive(false);
        category = 0;
        if(Lcategory[category].transform.childCount == 0) LoadProduct(); // 리스트 중복 생성 방지
    }

    public void BtnCrystalClick() // 보석 충전 목록 불러오기
    {
        pScroll.SetActive(false);
        cScroll.SetActive(true);
        category = 1;
        if(Lcategory[category].transform.childCount == 0) LoadProduct();
    }
}
