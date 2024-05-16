using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
인벤토리 관련 스크립트입니다. 총기 및 총기 부착물의 수량을 관리합니다.
*/

public class InventoryManager : MonoBehaviour
{
    public List<Dictionary<string, object>> gunData; // 총기 데이터
    public List<Dictionary<string, object>> accData; // 부착물 데이터
    public Dictionary<string, int> gunList = new Dictionary<string, int>(); // 총기 정보(ID, 개수)
    public Dictionary<string, int> accList = new Dictionary<string, int>(); // 부착물 정보(ID, 개수)
    void Awake()
    {
        gunData = CSVReader.Read("GunTable");
        accData = CSVReader.Read("GunAccTable");

        for(int i = 0; i < gunData.Count; i++)
        {
            gunList.Add(gunData[i]["GunID"].ToString(), 0); // 총기 정보 초기화
        }

        for(int i = 0; i < accData.Count; i++)
        {
            accList.Add(accData[i]["AccID"].ToString(), 0); // 부착물 정보 초기화
        }
        //기본 총기 세개 1개씩 지급
        gunList["101"] = 1;
        gunList["201"] = 1;
        gunList["301"] = 1;

        accList["901"] = 1;
        accList["801"] = 1;
        accList["701"] = 1;
        accList["601"] = 1;
        accList["501"] = 1;
        accList["401"] = 1;
    }
}
