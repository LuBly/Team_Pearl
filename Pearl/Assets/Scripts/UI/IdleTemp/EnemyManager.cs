using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/*
Enemy Stat Table 읽어오고 EnemyBase에 전달해주는 Manager 스크립트입니다. (Enemy 오브젝트에 삽입)
*/

public class EnemyManager : MonoBehaviour
{
    public StageManager sManager;
    public GameObject enemyP;
    List<Dictionary<string, object>> enemyData;

    private void Awake()
    {
        enemyData = CSVReader.Read("EnemyStatTableTemp");
    }

    public void EnemySet() // 적 설정 및 초기화 함수
    {
        string nowStage = sManager.nowStage; // 현재 스테이지 정보( ex) 1_1 = 챕터1-1)
        int hp = 0; // 체력 변수
        int rewardGold = 0; // 보상 골드 변수
        int rewardManaStone = 0; // 보상 마력석 변수
        string pos = ""; // 제외 포지션 (좌측 상단부터 1 2 3 / 4 5 6 / 7 8 9 배열)

        for(int i = 0; i < enemyP.transform.childCount; i++)
        {
            enemyP.transform.GetChild(i).gameObject.SetActive(true);
        }

        for(int i = 0; i < enemyData.Count; i++)
        {
            if(enemyData[i].First().Value.ToString() == nowStage) // 현재 스테이지 몬스터 정보 접근
            {
                hp = (int)enemyData[i]["Hp"]; // 체력 설정
                rewardGold = (int)enemyData[i]["RewardGold"]; // 골드 설정
                rewardManaStone = (int)enemyData[i]["RewardManaStone"]; // 마력석 설정
                pos = enemyData[i]["Position"].ToString(); // 제외 포지션 설정
                break;
            }
        }

        for(int i = 0; i < pos.Length; i++)
        {
            enemyP.transform.GetChild(pos[i] - '1').gameObject.SetActive(false); // 제외 포지션 몬스터 비활성화
        }

        int c = enemyP.transform.childCount;
        for(int i = 0; i < c; i++) // 모든 몬스터 체력, 보상 세팅
        {
            enemyP.transform.GetChild(i).GetComponent<EnemyBase>().hp = hp;
            enemyP.transform.GetChild(i).GetComponent<EnemyBase>().rewardGold = rewardGold;
            enemyP.transform.GetChild(i).GetComponent<EnemyBase>().rewardManaStone = rewardManaStone;
            enemyP.transform.GetChild(i).GetComponent<Image>().sprite = Resources.Load<Sprite>("EnemyPrefabs/" + nowStage);
        }
    }
}
