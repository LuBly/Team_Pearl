using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipManager : MonoBehaviour
{
    CharacterBase stat;
    IdleEnemy dmg;
    public int equip;
    int id;

    void Awake()
    {
        stat = GameObject.Find("MainChar").GetComponent<CharacterBase>();
        dmg = GameObject.Find("MainEnemy").GetComponent<IdleEnemy>();
    }

    public void EquipClick()
    {
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        equip = int.Parse(clickObject.name);
        GunDataRead();
    }

    void GunDataRead()
    {
        List<Dictionary<string, object>> gunData = CSVReader.Read("GunId"); // CSV 파일 "GunId" 에서 gunData 변수로 읽어오는 기능, CSVREADER.cs는 외부에서 가져옴. https://github.com/tikonen/blog/blob/master/csvreader/CSVReader.cs
        id = equip%100 - 1;
        stat.gunAtk = (int)gunData[id]["Attack"]; // CSV 파일 Attack 라인 id번 숫자 가져오기
        dmg.GunDmgSet();
        stat.gunRapid = (float)System.Convert.ToDouble(gunData[id]["Rapid"]); // CSV 파일 Rapid 라인 id번 숫자 가져오기
        //System.Conver 형식에 Float이 없어서 Double형으로 변경 후, float형 변경. (float)만 사용해서 변경할 경우, InvaildCastException 즉, 올바르지 않은 형변환 에러가 나타남.
    }


}
