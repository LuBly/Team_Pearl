using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class EquipManager : MonoBehaviour
{
    public TextMeshProUGUI equipText;
    public SaveManager sManager;
    public InventoryManager iManager;
    CharacterBase stat;
    IdleEnemy dmg;
    GameObject ar, sg, sr;
    int equip = 0;
    int id;
    string reset; // OutlineControl 초기화용 변수
    GameObject clickObject;

    void Awake()
    {
        stat = GameObject.Find("MainChar").GetComponent<CharacterBase>();
        dmg = GameObject.Find("MainEnemy").GetComponent<IdleEnemy>();
        ar = GameObject.Find("AR");
        sg = GameObject.Find("SG");
        sr = GameObject.Find("SR");
        ar.SetActive(true);
        sg.SetActive(false);
        sr.SetActive(false);
        reset = "reset";
    }

    public void GunClick() // 총기 아이콘 클릭 시 동작
    {
        clickObject = EventSystem.current.currentSelectedGameObject; // 현재 클릭한 오브젝트 받아오기
        if(clickObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text != "0")
        {
            equip = int.Parse(clickObject.name); // 클릭한 오브젝트 이름을 int형으로 치환
            OutlineControl(clickObject.name); // Outline 동작
        }
    }

    public void EquipClick() // 장착 버튼 클릭 시 동작
    {
        GunDataRead();
        sManager.Save();
    }

    void GunDataRead()
    {
        List<Dictionary<string, object>> gunData = CSVReader.Read("GunTable");
        int now = -1;
        if(equip == 0)
        {
            for(int i = 0; i < gunData.Count; i++)
            {
                if((int)gunData[i]["GunID"] == stat.id)
                {
                    now = i;
                    break;
                }
            }
        }
        else
        {
            for(int i = 0; i < gunData.Count; i++)
            {
                if((int)gunData[i]["GunID"] == equip)
                {
                    now = i;
                    break;
                }
            }
        }
        if(now == -1) Debug.Log("잘못된 ID 입력!"); // 예외처리 필요
        else
        {
            stat.id = (int)gunData[now]["GunID"];
            equipText.text = gunData[now]["GunName"].ToString();
            if((int)gunData[now]["GunAtk1"] == 0) stat.gunAtk = (int)gunData[now]["GunAtk2"];
            else stat.gunAtk = (int)gunData[now]["GunAtk1"];
            dmg.GunDmgSet();
            stat.gunRapid = (float)System.Convert.ToDouble(gunData[now]["Rapid"]);
        }
    }

    public void BtnARClick()
    {
        OutlineControl(reset); // 아웃라인 제거
        ar.SetActive(true);
        sg.SetActive(false);
        sr.SetActive(false);
        CountControl();
    }

    public void BtnSGClick()
    {
        OutlineControl(reset);
        ar.SetActive(false);
        sg.SetActive(true);
        sr.SetActive(false);
        CountControl();
    }

    public void BtnSRClick()
    {
        OutlineControl(reset);
        ar.SetActive(false);
        sg.SetActive(false);
        sr.SetActive(true);
        CountControl();
    }

    void OutlineControl(string gunId) // 아웃라인 활성화 함수
    {
        GameObject [] obj = GameObject.FindGameObjectsWithTag("Gun"); // 태그 중 Gun을 가지고 있는 오브젝트 모두 검색
        for(int i = 0; i < obj.Length; i++)
        {
            if(gunId == obj[i].name) // 현재 클릭된 오브젝트 이름과 같은 이름의 오브젝트일 경우
            {
                obj[i].GetComponent<Outline>().enabled = true; // 아웃라인 활성화
            }
            else obj[i].GetComponent<Outline>().enabled = false; // 아웃라인 비활성화
        }
    }

    public void CountControl()
    {
        GameObject [] obj = GameObject.FindGameObjectsWithTag("Gun");
        for(int i = 0; i < obj.Length; i++)
        {
            obj[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = iManager.gunList[obj[i].name].ToString();
        }
    }

}
