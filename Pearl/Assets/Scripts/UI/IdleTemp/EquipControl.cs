using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.U2D.IK;
using Unity.Burst.Intrinsics;
using System;
using System.Linq;

/*
장비 장착 및 관리 스크립트입니다.(EquipUI에 삽입)
*/

public class EquipControl : MonoBehaviour
{
    public InventoryManager iManager; // 총기 수량 및 총기 데이터
    public CharacterBase cBase; // 캐릭터 데이터
    public GameObject guns, accs; // 모든 총기, 부착물 부모 오브젝트
    public GameObject infoGun; // 선택 장비 아이콘
    public TextMeshProUGUI infoText; // 선택 장비 능력치
    public GameObject nowGun, nowMuzzle, nowGrip, nowScope; // 장착 장비 아이콘
    public TextMeshProUGUI nowInfo, nowAccInfo; // 장착 총기, 부착물 능력치
    public TextMeshProUGUI failText; // 장착 실패 텍스트
    public int muzzle = 0, grip = 0, scope = 0; // 부착물 장착 현황
    GameObject clickObject; // 선택 장비
    int now = -1; // 데이터 위치 변수
    bool isGun; // 총기 부착물 구분 변수

    public void ChangeGun()
    {
        isGun = true;
    }

    public void ChangeAcc()
    {
        isGun = false;
    }

    public void GunClick() // 총기 이미지 클릭 시 동작 -> 현재 선택 장비 정보 업데이트 용
    {
        clickObject = EventSystem.current.currentSelectedGameObject; // 현재 클릭한 오브젝트 정보 읽기

        if(isGun) // 총기일 경우
        {
            for(int i = 0; i < iManager.gunData.Count; i++)
            {
                if(iManager.gunData[i]["GunID"].ToString() == clickObject.name) // 클릭한 오브젝트의 이름에 해당하는 총기 데이터 위치 파악
                {
                    now = i;
                    break;
                }
            }

            if(now == -1) return; // 클릭 오브젝트 이름에 해당하는 총기 데이터가 없음(에러)

            if(iManager.gunData[now]["GunAtk1"].ToString() == "0") // 총기 공격력 2를 사용하는 총기의 경우
            {
                infoText.text = "이름 : " + iManager.gunData[now]["GunName"].ToString()  + " 등급 : " + iManager.gunData[now]["Grade"] + "\n" + "공격력 : " + iManager.gunData[now]["GunAtk2"] + "%" + " 공격속도 : " + iManager.gunData[now]["Rapid"];
            }
            else infoText.text = "이름 : " + iManager.gunData[now]["GunName"].ToString()  + " 등급 : " + iManager.gunData[now]["Grade"] + "\n" + "공격력 : " + iManager.gunData[now]["GunAtk1"] + " 공격속도 : " + iManager.gunData[now]["Rapid"];
            infoGun.GetComponent<Image>().color = Resources.Load<GameObject>("GunPrefab/" + iManager.gunData[now]["GunPrefab"].ToString()).GetComponent<Image>().color; // 선택 장비 아이콘 색상 설정
            infoGun.GetComponent<Image>().sprite = Resources.Load<GameObject>("GunPrefab/" + iManager.gunData[now]["GunPrefab"].ToString()).GetComponent<Image>().sprite; // 선택 장비 아이콘 이미지 설정
        }
        else // 부착물일 경우
        {
            for(int i = 0; i < iManager.accData.Count; i++)
            {
                if(iManager.accData[i]["AccID"].ToString() == clickObject.name)
                {
                    now = i;
                    break;
                }
            }

            if(now == -1) return;

            int type = Int32.Parse(clickObject.name) / 100;

            switch(type)
            {
                case 4:
                    infoText.text = "이름 : " + iManager.accData[now]["AccName"].ToString() + " 등급 : " + iManager.accData[now]["Grade"] + "\n" + "기본 공격 피해 증가 +" + iManager.accData[now]["BasicAtkDmg%"] + "%";
                    break;
                case 5:
                    infoText.text = "이름 : " + iManager.accData[now]["AccName"].ToString() + " 등급 : " + iManager.accData[now]["Grade"] + "\n" + "스킬 공격 피해 증가 +" + iManager.accData[now]["SkillDmg%"] + "%";
                    break;
                case 6:
                    infoText.text = "이름 : " + iManager.accData[now]["AccName"].ToString() + " 등급 : " + iManager.accData[now]["Grade"] + "\n" + "기본 공격 피해 증가 +" + iManager.accData[now]["BasicAtkDmg%"] + "%";
                    break;
                case 7:
                    infoText.text = "이름 : " + iManager.accData[now]["AccName"].ToString() + " 등급 : " + iManager.accData[now]["Grade"] + "\n" + "스킬 공격 피해 증가 +" + iManager.accData[now]["SkillDmg%"] + "%";
                    break;
                case 8:
                    infoText.text = "이름 : " + iManager.accData[now]["AccName"].ToString() + " 등급 : " + iManager.accData[now]["Grade"] + "\n" + "치명타 피해 증가 +" + iManager.accData[now]["CritDmg%"] + "%";
                    break;
                case 9:
                    infoText.text = "이름 : " + iManager.accData[now]["AccName"].ToString() + " 등급 : " + iManager.accData[now]["Grade"] + "\n" + "치명타 확률 증가 +" + iManager.accData[now]["CritChance%"] + "%";
                    break;
            }
            infoGun.GetComponent<Image>().color = Resources.Load<GameObject>("AccPrefab/" + iManager.accData[now]["AccPrefab"].ToString()).GetComponent<Image>().color; // 선택 장비 아이콘 색상 설정
            infoGun.GetComponent<Image>().sprite = Resources.Load<GameObject>("AccPrefab/" + iManager.accData[now]["AccPrefab"].ToString()).GetComponent<Image>().sprite; // 선택 장비 아이콘 이미지 설정
        }
    }

    public void EquipClick() // 장착 버튼 클릭 시 동작
    {
        string nowId; // 현재 선택된 장비 ID

        StopCoroutine("OnFailText"); // 코루틴 중복 실행 방지
        
        if(now != -1) // 선택한 장비가 있을 경우 now가 -1이 아닌 다른 수가 됨.
        {
            if(isGun) // 총기일 경우
            {
                nowId = iManager.gunData[now]["GunID"].ToString();
                if(iManager.gunList[nowId] == 0) StartCoroutine("OnFailText"); // 선택된 장비 보유 수량이 0일 경우 장착 실패 텍스트 출력
                else // 선택된 장비를 보유하고 있을 경우
                {
                    cBase.id = (int)iManager.gunData[now]["GunID"]; // 캐릭터 총기 ID 변경
                    cBase.gunAtk = (int)iManager.gunData[now]["GunAtk1"]; // 캐릭터 총기 공력력 변경
                    cBase.gunAtk2 = (int)iManager.gunData[now]["GunAtk2"]; // 캐릭터 총기 공격력2 변경
                    cBase.gunRapid = (int)iManager.gunData[now]["Rapid"]; // 캐릭터 총기 공격속도 변경
                    EquipUpdate(); // 현재 장착 중인 장비 업데이트
                }
            }
            else // 부착물일 경우
            {
                nowId = iManager.accData[now]["AccID"].ToString();
                if(iManager.accList[nowId] == 0) StartCoroutine("OnFailText");
                else
                {
                    int type = Int32.Parse(nowId) / 100;
                    switch(type)
                    {
                        case 4:
                        case 5:
                            scope = Int32.Parse(nowId);
                            break;
                        case 6:
                        case 7:
                            grip = Int32.Parse(nowId);
                            break;
                        case 8:
                        case 9:
                            muzzle = Int32.Parse(nowId);
                            break;
                    }
                }
                AccUpdate();
            }
        }
        else // 선택한 장비가 없을 경우
        {
            StartCoroutine("OnFailText"); // 장착 실패 텍스트 출력
        }
    }

    public void EquipUpdate() // 현재 장착 중인 장비 업데이트 함수 / Main -> 장비 버튼 클릭 및 장비 -> 장착 버튼 클릭 시 동작
    {
        int nowId = cBase.id; // 캐릭터 총기 ID
        int n = -1;
        for(int i = 0; i < iManager.gunData.Count; i++) // 총기 ID 위치 확인
        {
            if(iManager.gunData[i]["GunID"].ToString() == nowId.ToString())
            {
                n = i;
                break;
            }
        }

        if(n == -1) return; // 총기 ID가 데이터에 존재하지 않음(에러)

        if(iManager.gunData[n]["GunAtk1"].ToString() == "0")
        {
            nowInfo.text = "이름 : " + iManager.gunData[n]["GunName"].ToString() + "\n" + "등급 : " + iManager.gunData[n]["Grade"] + "\n" + "공격력 : " + iManager.gunData[n]["GunAtk2"] + "%" + "\n" + "공격속도 : " + iManager.gunData[n]["Rapid"];
        }
        else nowInfo.text = "이름 : " + iManager.gunData[n]["GunName"].ToString() + "\n" + "등급 : " + iManager.gunData[n]["Grade"] + "\n" + "공격력 : " + iManager.gunData[n]["GunAtk1"] + "\n" + "공격속도 : " + iManager.gunData[n]["Rapid"];
        nowGun.GetComponent<Image>().color = Resources.Load<GameObject>("GunPrefab/" + iManager.gunData[n]["GunPrefab"].ToString()).GetComponent<Image>().color;
        nowGun.GetComponent<Image>().sprite = Resources.Load<GameObject>("GunPrefab/" + iManager.gunData[n]["GunPrefab"].ToString()).GetComponent<Image>().sprite;
    }

    public void AccUpdate()
    {
        int nM = -1, nS = -1, nG = -1;
        for(int i = 0; i < iManager.accData.Count; i++) // 추후 탐색 알고리즘 사용하면 동작 시간 단축 가능, 현재는 표본수가 워낙 적어서 상관없을듯함
        {
            if(iManager.accData[i]["AccID"].ToString() == muzzle.ToString()) nM = i;
            if(iManager.accData[i]["AccID"].ToString() == grip.ToString()) nS = i;
            if(iManager.accData[i]["AccID"].ToString() == scope.ToString()) nG = i;
        }

        if(nM == -1)
        {
            nowAccInfo.text = "총구 : 장착 중인 총구가 없습니다.\n";
        }
        else
        {
            if(iManager.accData[nM]["CritDmg%"].ToString() != "0") nowAccInfo.text = "총구 : 치명타 피해 증가 +" + iManager.accData[nM]["CritDmg%"] + "%" + "\n";
            else nowAccInfo.text = "총구 : 치명타 확률 증가 +" + iManager.accData[nM]["CritChance%"] + "%" + "\n";
            nowMuzzle.GetComponent<Image>().color = Resources.Load<GameObject>("AccPrefab/" + iManager.accData[nM]["AccPrefab"].ToString()).GetComponent<Image>().color;
            nowMuzzle.GetComponent<Image>().sprite = Resources.Load<GameObject>("AccPrefab/" + iManager.accData[nM]["AccPrefab"].ToString()).GetComponent<Image>().sprite;
        }

        if(nG == -1)
        {
            nowAccInfo.text += "손잡이 : 장착 중인 손잡이가 없습니다.\n";
        }
        else
        {
            if(iManager.accData[nG]["BasicAtkDmg%"].ToString() != "0") nowAccInfo.text += "손잡이 : 기본 공격 피해 증가 +" + iManager.accData[nG]["BasicAtkDmg%"] + "%" + "\n";
            else nowAccInfo.text += "손잡이 : 스킬 공격 피해 증가 +" + iManager.accData[nG]["SkillDmg%"] + "%" + "\n";
            nowGrip.GetComponent<Image>().color = Resources.Load<GameObject>("AccPrefab/" + iManager.accData[nG]["AccPrefab"].ToString()).GetComponent<Image>().color;
            nowGrip.GetComponent<Image>().sprite = Resources.Load<GameObject>("AccPrefab/" + iManager.accData[nG]["AccPrefab"].ToString()).GetComponent<Image>().sprite;
        }

        if(nS == -1)
        {
            nowAccInfo.text += "조준경 : 장착 중인 조준경이 없습니다.";
        }
        else
        {
            if(iManager.accData[nS]["BasicAtkDmg%"].ToString() != "0") nowAccInfo.text += "조준경 : 기본 공격 피해 증가 +" + iManager.accData[nS]["BasicAtkDmg%"] + "%" + "\n";
            else nowAccInfo.text += "조준경 : 스킬 공격 피해 증가 +" + iManager.accData[nS]["SkillDmg%"] + "%" + "\n";
            nowScope.GetComponent<Image>().color = Resources.Load<GameObject>("AccPrefab/" + iManager.accData[nS]["AccPrefab"].ToString()).GetComponent<Image>().color;
            nowScope.GetComponent<Image>().sprite = Resources.Load<GameObject>("AccPrefab/" + iManager.accData[nS]["AccPrefab"].ToString()).GetComponent<Image>().sprite;
        }
    }

    public void BtnARClick() // 돌격소총 목록 보이게
    {
        guns.transform.GetChild(0).gameObject.SetActive(true);
        guns.transform.GetChild(1).gameObject.SetActive(false);
        guns.transform.GetChild(2).gameObject.SetActive(false);
    }

    public void BtnSGClick() // 산탄총 목록 보이게
    {
        guns.transform.GetChild(0).gameObject.SetActive(false);
        guns.transform.GetChild(1).gameObject.SetActive(true);
        guns.transform.GetChild(2).gameObject.SetActive(false);
    }

    public void BtnSRClick() // 저격소총 목록 보이게
    {
        guns.transform.GetChild(0).gameObject.SetActive(false);
        guns.transform.GetChild(1).gameObject.SetActive(false);
        guns.transform.GetChild(2).gameObject.SetActive(true);
    }

    public void CountControl() // 총기 아이콘 하단의 총기 개수 적용 / Main -> 장비 클릭 시 동작
    {
        for(int j = 0; j < guns.transform.childCount; j++) // 모든 총기 부모 오브젝트의 자식 오브젝트 수 = AR / SR / SG -> 3
        {
            GameObject obj = guns.transform.GetChild(j).gameObject; // 0번 자식 오브젝트부터 탐색
            for(int i = 0; i < obj.transform.childCount; i++) // 자식 오브젝트의 수 = 총기 ID 수
            {
                obj.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = iManager.gunList[obj.transform.GetChild(i).name].ToString();
                // 각 총기 ID 오브젝트의 자식 오브젝트(총기 개수 텍스트)를 현재 보유중인 수량으로 설정
            }
        }

        for(int j = 0; j < accs.transform.childCount; j++) // 모든 부착물 부모 오브젝트의 자식 오브젝트 수 = 총구 / 조준경 / 손잡이 -> 3
        {
            GameObject obj = accs.transform.GetChild(j).gameObject; // 0번 자식 오브젝트부터 탐색
            for(int i = 0; i < obj.transform.childCount; i++) // 자식 오브젝트의 수 = 총기 ID 수
            {
                obj.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = iManager.accList[obj.transform.GetChild(i).name].ToString();
                // 각 총기 ID 오브젝트의 자식 오브젝트(총기 개수 텍스트)를 현재 보유중인 수량으로 설정
            }
        }
        
    }

    IEnumerator OnFailText() // 장착 실패 텍스트 Fading 코루틴
    {
        float nowAlpha = 1f;
        failText.color = new Color(failText.color.r, failText.color.g, failText.color.b, nowAlpha); // 알파값 1로 설정 == 텍스트 등장

        while(failText.color.a >= 0) // 알파값 0이 될때까지 반복
        {
            yield return new WaitForSeconds(0.1f);
            nowAlpha -= 0.1f;
            failText.color = new Color(failText.color.r, failText.color.g, failText.color.b, nowAlpha);
        }
        yield break;
    }

    public void AllSynthesize() // 일괄 합성
    {
        for(int i = 0; i < iManager.gunList.Count; i++) // 총기 리스트 탐색
        {
            int nV = iManager.gunList.Values.ToList()[i];
            string nK = iManager.gunList.Keys.ToList()[i];
            if(Int32.Parse(nK) == cBase.id) nV -= 1; // 현재 장착중인 총기일 경우 장착 중인 1개 제외하고 합성 진행
            if(nV >= 3 && Int32.Parse(nK) % 100 < 11) // 총기 수량이 3개 이상이고, 각 총기의 10번 총기까지만 합성 11번 -> 12번은 합성으로 습득X
            {
                int up = nV / 3; // 생성된 다음 등급 총기 수
                int rest = nV % 3; // 남은 현재 등급 총기 수
                iManager.gunList[nK] = rest; // 현재 등급 총기 수 적용
                if(Int32.Parse(nK) == cBase.id) iManager.gunList[nK] += 1; // 장착 중이던 1개 제외한 총기 수 적용
                iManager.gunList[(Int32.Parse(nK) + 1).ToString()] += up; // 다음 등급 총기 수 적용
            }
        }
        CountControl(); // 총기 개수 업데이트
    }
}
