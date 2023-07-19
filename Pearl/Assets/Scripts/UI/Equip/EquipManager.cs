using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipManager : MonoBehaviour
{
    CharacterBase stat;
    IdleEnemy dmg;
    GameObject ar, sg, sr;
    public int equip;
    int id;

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
    }

    public void EquipClick() // 총기 아이콘 클릭 시 동작
    {
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        equip = int.Parse(clickObject.name);
        GunDataRead();
    }

    void GunDataRead()
    {
        List<Dictionary<string, object>> gunData = CSVReader.Read("GunId"); // CSV 파일 "GunId" 에서 gunData 변수로 읽어오는 기능, CSVREADER.cs는 외부에서 가져옴. https://github.com/tikonen/blog/blob/master/csvreader/CSVReader.cs
        if(equip > 100 && equip <= 200)
        {
            id = equip%100 - 1;
        }
        else if(equip >200 && equip <= 300)
        {
            id = equip%200 + 99;
        }
        else if(equip > 300 && equip <= 400)
        {
            id = equip%300 + 199;
        }
        else Debug.Log("Error!"); // 예외처리 할 부분
        stat.gunAtk = (int)gunData[id]["Attack"]; // CSV 파일 Attack 라인 id번 숫자 가져오기
        dmg.GunDmgSet();
        stat.gunRapid = (float)System.Convert.ToDouble(gunData[id]["Rapid"]); // CSV 파일 Rapid 라인 id번 숫자 가져오기
        //System.Conver 형식에 Float이 없어서 Double형으로 변경 후, float형 변경. (float)만 사용해서 변경할 경우, InvaildCastException 즉, 올바르지 않은 형변환 에러가 나타남.
    }

    public void BtnARClick()
    {
        ar.SetActive(true);
        sg.SetActive(false);
        sr.SetActive(false);
    }

    public void BtnSGClick()
    {
        ar.SetActive(false);
        sg.SetActive(true);
        sr.SetActive(false);
    }

    public void BtnSRClick()
    {
        ar.SetActive(false);
        sg.SetActive(false);
        sr.SetActive(true);
    }


}
