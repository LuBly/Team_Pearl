using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
Idle 전투 관련 스크립트입니다.(IdleUI 오브젝트에 삽입)
*/

public class IdleEngage : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent EngageDone; // 전투 종료 이벤트
    public CharacterBase cBase;
    public GoodsBase gBase;
    public SkillManager sKBase;
    float nowDmg;
    float nowRapid;
    GameObject reward;
    int tGold = 0;
    int tMs = 0;
    public GameObject rewardPos;

    List<List<GameObject>> enemyL = new List<List<GameObject>>(); // Enemy 2차원 리스트 샷건 같은 경우에 항상 각 행의 첫번째 열 타겟이 데미지를 입어야 하므로, 리스트의 타겟이 사라졌을때, 자동으로 다시 정렬되므로 리스트를 사용.
    List<List<GameObject>> enemyLA = new List<List<GameObject>>(); // AR용 리스트(AR은 가까운 열부터 공격하기에 열과 행이 반대로 구성되어야 함)

    void Start()
    {
        reward = Resources.Load<GameObject>("Reward/Reward");
        ResetList();
    }

    void ResetList()
    {
        for(int i = 0; i < 3; i++)
        {
            enemyL.Add(new List<GameObject>()); // 행 3개를 가지는 리스트로 초기화
            enemyLA.Add(new List<GameObject>());
        } 
    }

    void SetTarget() // 타겟 설정(스테이지에 현재 활성화 된 몬스터를 각 위치에 해당하는 리스트에 추가)
    {
        GameObject [] enemyA = GameObject.FindGameObjectsWithTag("Enemy"); // Enemy 배열(FindGameObjectsWithTag가 배열로 형성되기에 읽어들이기 쉽게 배열로 선언함.)
        for(int i = 0; i < enemyA.Length; i++)
        {
            int now = Int32.Parse(enemyA[i].name);

            //enemyL 세팅
            if(now <= 3) enemyL[0].Add(enemyA[i]);
            else if(now <= 6) enemyL[1].Add(enemyA[i]);
            else enemyL[2].Add(enemyA[i]);
            
            //enemyLA 세팅
            if(now % 3 == 1) enemyLA[0].Add(enemyA[i]);
            else if(now % 3 == 2) enemyLA[1].Add(enemyA[i]);
            else enemyLA[2].Add(enemyA[i]);
        }
    }

    void GunDmgSet()
    {
        int gun = cBase.id / 100; // 총기 종류 탐색
        int nowSkill = 0; // 현재 총기 숙련도
        switch(gun)
        {
            case 1:
                nowSkill = sKBase.ARLv; // AR 숙련도
                break;
            case 2:
                nowSkill = sKBase.SGLv * 3; // SG 숙련도(레벨 * 3이 상승한 공격력)
                break;
            case 3:
                nowSkill = sKBase.SRLv * 6; // SR 숙련도(레벨 * 6이 상승한 공격력)
                break;
        }
        nowDmg = nowSkill + ((float)nowSkill * (((float)cBase.gunAtk2 + (float)cBase.atk) / 100)) + cBase.gunAtk;
        if(cBase.id / 100 == 2) // 샷건일 경우
        {
            nowDmg = nowDmg / 3; // 공격력 1/3 처리
        }
        nowRapid = cBase.gunRapid;
    }

    public void ReadyToEngage()
    {
        GunDmgSet();
        StartCoroutine(Encounter());
    }
    
    IEnumerator Encounter()
    {
        SetTarget();
        int gun = cBase.id / 100; // 총기 종류 탐색
        Debug.Log(nowDmg);

        while(enemyL.Any()) // 적이 남아 있을 경우
        {
            yield return new WaitForSecondsRealtime(nowRapid / 1000); // 공격속도만큼 반복(현실시간 기준)
            switch(gun)
            {
                case 1 : // AR(가까운 열의 낮은 행부터 공격)
                    if(enemyLA[0].Any())
                    {
                        enemyLA[0][0].GetComponent<EnemyBase>().hp -= nowDmg;
                        if(enemyLA[0][0].GetComponent<EnemyBase>().hp <= 0) 
                        {
                            GameObject dropReward = Instantiate(reward);
                            dropReward.transform.SetParent(rewardPos.transform);
                            dropReward.transform.position = enemyLA[0][0].transform.position;
                            tGold += enemyLA[0][0].GetComponent<EnemyBase>().rewardGold;
                            tMs += enemyLA[0][0].GetComponent<EnemyBase>().rewardManaStone;
                            
                            enemyLA[0][0].SetActive(false);
                            enemyLA[0].RemoveAt(0);
                            if(!enemyLA[0].Any())
                            {
                                enemyLA.RemoveAt(0);
                                if(!enemyLA.Any()) goto EXIT;// enemyLA가 비었을 경우 While문 탈출
                            }
                        }
                    }
                    else goto EXIT;
                    break;
                case 2 : // SG(가까운 열부터 1/3데미지 공격)
                    if(enemyL[0].Any())
                    {
                        enemyL[0][0].GetComponent<EnemyBase>().hp -= nowDmg;
                        if(enemyL[0][0].GetComponent<EnemyBase>().hp <= 0) 
                        {
                            GameObject dropReward = Instantiate(reward);
                            dropReward.transform.SetParent(rewardPos.transform);
                            dropReward.transform.position = enemyL[0][0].transform.position;
                            tGold += enemyL[0][0].GetComponent<EnemyBase>().rewardGold;
                            tMs += enemyL[0][0].GetComponent<EnemyBase>().rewardManaStone;

                            enemyL[0][0].SetActive(false);
                            enemyL[0].RemoveAt(0);
                        }
                    }
                    if(enemyL[1].Any())
                    {
                        enemyL[1][0].GetComponent<EnemyBase>().hp -= nowDmg;
                        if(enemyL[1][0].GetComponent<EnemyBase>().hp <= 0) 
                        {
                            GameObject dropReward = Instantiate(reward);
                            dropReward.transform.SetParent(rewardPos.transform);
                            dropReward.transform.position = enemyL[1][0].transform.position;
                            tGold += enemyL[1][0].GetComponent<EnemyBase>().rewardGold;
                            tMs += enemyL[1][0].GetComponent<EnemyBase>().rewardManaStone;

                            enemyL[1][0].SetActive(false);
                            enemyL[1].RemoveAt(0);
                        }
                    }
                    if(enemyL[2].Any())
                    {
                        enemyL[2][0].GetComponent<EnemyBase>().hp -= nowDmg;
                        if(enemyL[2][0].GetComponent<EnemyBase>().hp <= 0) 
                        {
                            GameObject dropReward = Instantiate(reward);
                            dropReward.transform.SetParent(rewardPos.transform);
                            dropReward.transform.position = enemyL[2][0].transform.position;
                            tGold += enemyL[2][0].GetComponent<EnemyBase>().rewardGold;
                            tMs += enemyL[2][0].GetComponent<EnemyBase>().rewardManaStone;

                            enemyL[2][0].SetActive(false);
                            enemyL[2].RemoveAt(0);
                        }
                    }
                    if(!enemyL[0].Any() && !enemyL[1].Any() && !enemyL[2].Any()) enemyL.Clear();
                    break;
                case 3 : // SR(낮은 행부터 관통 공격)
                    if(enemyL[0].Any())
                    {
                        for(int i = 0; i < enemyL[0].Count; i++)
                        {
                            enemyL[0][i].GetComponent<EnemyBase>().hp -= nowDmg;
                            if(enemyL[0][i].GetComponent<EnemyBase>().hp <= 0)
                            {
                                GameObject dropReward = Instantiate(reward);
                                dropReward.transform.SetParent(rewardPos.transform);
                                dropReward.transform.position = enemyL[0][i].transform.position;
                                tGold += enemyL[0][i].GetComponent<EnemyBase>().rewardGold;
                                tMs += enemyL[0][i].GetComponent<EnemyBase>().rewardManaStone;

                                enemyL[0][i].SetActive(false);
                            }
                        }
                        while(enemyL[0][0].GetComponent<EnemyBase>().hp <= 0)
                        {
                            enemyL[0].RemoveAt(0);
                            if(!enemyL[0].Any())
                            {
                                enemyL.RemoveAt(0);
                                break;
                            }
                        }
                    }
                    break;
            }
        }
        EXIT:
        MoveReward();
        GetGoods();
        enemyL.Clear();
        enemyLA.Clear();
        ResetList();
        EngageDone.Invoke();
        yield break; // 코루틴 종료
    }

    void MoveReward() // 보상 습득 애니메이션 함수
    {
        for(int i = 0; i < rewardPos.transform.childCount; i++)
        {
            rewardPos.transform.GetChild(i).GetComponent<Animator>().SetTrigger("isAllDead");
        }
    }

    void GetGoods() // 보상 습득 적용
    {
        gBase.gold += tGold;
        gBase.manaStone += tMs;
        gBase.GoodsUpdate();
        tGold = 0;
        tMs = 0;
    }
}
