using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TextCore.Text;

/*
게임 내 세이브 데이터를 관리하는 스크립트입니다.
*/


public class SaveManager : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent SaveDone;
    public SaveData saveData;
    public CharacterBase cBase;
    public RewardBoxManager box;
    public IngameGoods goods;
    public StatUpManager stat;
    public InventoryManager iManager;

    public void Save()
    {
        saveData = new SaveData();
        SetSaveData();
        //데이터 기록

        string path = Application.persistentDataPath + "/saves/"; // 폴더 경로
        string filePath = path + "AGSave" + ".json"; // 파일 경로
        string jsonData;

        if(!Directory.Exists(path)) // 경로에 폴더가 존재하지 않을 경우
        {
            Directory.CreateDirectory(path); // 폴더 생성
        }
        if(!File.Exists(filePath)) // 파일이 존재하지 않을 경우
        {
            FileStream temp = File.Create(filePath); // 임시 파일 생성
            temp.Close();
        }

        jsonData = JsonUtility.ToJson(saveData); // saveData json 변환
        
        File.WriteAllText(filePath, jsonData); // 생성했던 임시파일 덮어쓰기

        SaveDone.Invoke();
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/saves/";
        string filePath = path + "AGSave" + ".json";

        if(!File.Exists(filePath)) // 세이브 파일이 없을 경우
        {
            Debug.Log("세이브 파일이 없습니다.");
            return;
        }

        string jsonData = File.ReadAllText(filePath); // jsonData 읽어오기
        saveData = JsonUtility.FromJson<SaveData>(jsonData); // saveData에 json 값 삽입

        LoadSaveData();
    }

    void SetSaveData()
    {
        saveData.id = cBase.id;
        saveData.gold = goods.gold;
        saveData.manastone = goods.manaStone;
        saveData.crystal = goods.crystal;
        saveData.bGold = box.boxGold;
        saveData.bManastone = box.boxManaStone;
        saveData.speedP = stat.speedUpPoint;
        saveData.hpP = stat.hpUpPoint;
        saveData.dmgP = stat.dmgUpPoint;
        SetStageClearData();
        SetGunCountData();
    }

    void LoadSaveData()
    {
        cBase.id = saveData.id;
        goods.gold = saveData.gold;
        goods.manaStone = saveData.manastone;
        goods.crystal = saveData.crystal;
        box.boxGold = saveData.bGold;
        box.boxManaStone = saveData.bManastone;
        stat.speedUpPoint = saveData.speedP;
        stat.hpUpPoint = saveData.hpP;
        stat.dmgUpPoint = saveData.dmgP;
        goods.GoodsUpdate();
        if(!ClearManager.nowClear) GetStageClearData();
        GetGunCountData();
    }

    void SetStageClearData()
    {
        foreach(string i in ClearManager.isClear.Keys)
        {
            saveData.stage.Add(i);
        }
        foreach(bool i in ClearManager.isClear.Values)
        {
            saveData.stageClear.Add(i);
        }
    }

    void GetStageClearData()
    {
        for(int i = 0; i < saveData.stage.Count; i++)
        {
            ClearManager.isClear[saveData.stage[i]] = saveData.stageClear[i];
        }
    }

    void SetGunCountData()
    {
        foreach(int i in iManager.gunList.Values)
        {
            saveData.gunCount.Add(i);
        }
        foreach(string i in iManager.gunList.Keys)
        {
            saveData.gunID.Add(i);
        }
    }

    void GetGunCountData()
    {
        for(int i = 0; i < saveData.gunCount.Count; i++)
        {
            iManager.gunList[saveData.gunID[i]] = saveData.gunCount[i];
        }
    }
}

[System.Serializable]
public class SaveData
{
    public int id; // 총기 id
    public int gold, manastone, crystal; // 재화
    public int bGold, bManastone; // 보상 상자 재화
    public int speedP, hpP, dmgP; // 스탯 업그레이드 포인트
    
    // Dictionary 변수는 UtilityJson을 통한 Json 변환이 불가능하여, Key와 Value 각각을 리스트로 저장하여 저장
    public List<string> stage = new List<string>(); 
    public List<bool> stageClear = new List<bool>();
    public List<string> gunID = new List<string>();
    public List<int> gunCount = new List<int>();
}
