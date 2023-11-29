using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
클리어 변수 스크립트입니다.
이후, 전투씬에서 해당 씬을 클리어 할 경우, c + 챕터 + s + 스테이지(ex 챕터 1 스테이지 1 = c1s1)
ClearManager.isClear["c1s1"] = true;를 추가해 주시면 클리어 처리 됩니다. 
*/


public class ClearManager
{
    public static Dictionary<string, bool> isClear = new Dictionary<string, bool>();
    public static bool isFirst = true; // 임시 변수 추후 세이브 관련 구현 시 교체될 예정.
    public static bool nowClear; // 전투 스테이지 클리어 보상 관리 변수

    public static void ResetClear()
    {
        isClear.Add("c1s1", true);
        isClear.Add("c1s2", false);
        isClear.Add("c1s3", false);
        isClear.Add("c1s4", false);
        isClear.Add("c2s1", false);
        isClear.Add("c2s2", false);
        isClear.Add("c2s3", false);
        isClear.Add("c2s4", false);
        isClear.Add("c3s1", false);
        isClear.Add("c3s2", false);
        isClear.Add("c3s3", false);
        isClear.Add("c3s4", false);
    }

    public static void SetAllClear()
    {
        List<string> key = new List<string>(isClear.Keys);
        for(int i = 0; i < key.Count; i++)
        {
            isClear[key[i]] = true;
        }
    }
}
