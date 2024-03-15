using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
Idle Stage 관리 스크립트입니다. StageUI에 삽입
*/

public class StageManager : MonoBehaviour
{
    public TextMeshProUGUI stageText, chapterText, exGoldText, exMsText, failText, notiText; // 변경 될 텍스트
    public Image monster; // 미리보기 이미지
    public GameObject stageBox; // 버튼과 챕터명 부모 오브젝트
    public EnemyManager eManager; // enemyData 담고 있는 스크립트
    public string nowStage; // Stage 변수(해당 변수가 몬스터 데이터 및 몬스터 이미지 등 실제 게임 진행에 영향을 미침)
    int nowChapter, chapter; // nowChapter == 실제 변경에 사용하는 변수, chapter == UI 안에서 변하는 챕터 값(버튼 클릭 등)
    string stage; // stage 선택 값 저장 변수
    string [] chapters = new string [] {"Europa", "Kepler", "Ganymede", "Callisto", "Enceladus", "Gliese", "Trappist", "Infested ship"}; // 각 챕터 이름 배열

    void Start()
    {
        StageUpdate(); // 첫 로드 후, 스테이지 이름 로드
        monster.transform.gameObject.SetActive(false); // 몬스터 미리보기 비활성화
    }

    public void ResetPanelStage() // Stage 내 초기화 해줘야하는 일회용 변수 초기화 함수(Stage 버튼 클릭 시 실행)
    {
        nowChapter = Int32.Parse(nowStage.Substring(0, 1)) - 1;
        chapter = nowChapter;
        monster.transform.gameObject.SetActive(false);
        exGoldText.text = "0";
        exMsText.text = "0";
        stage = "";
    }

    public void StageUpdate() // Stage 이름 업데이트 함수 Start와  IdleUI의 EngageDone 이벤트에 삽입
    {
        stageText.text = chapters[nowChapter] + " " + nowStage.Substring(2,1);
    }

    public void ChapterUpdate() // StagePanel 내부의 챕터 업데이트 함수
    {
        chapterText.text = chapters[chapter];
        if(chapter == 0) stageBox.transform.GetChild(1).gameObject.SetActive(false); // 챕터가 첫 번째 챕터일 경우 왼쪽 버튼 비활성화
        else if(chapter == chapters.Length - 1) stageBox.transform.GetChild(0).gameObject.SetActive(false); // 챕터가 마지막 챕터일 경우 오른쪽 버튼 비활성화
        else // 양쪽 버튼 모두 활성화
        {
            stageBox.transform.GetChild(0).gameObject.SetActive(true);
            stageBox.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void StageClick()
    {
        stage = EventSystem.current.currentSelectedGameObject.name; // 스테이지 버튼 클릭(각 버튼이 스테이지 이름(1, 2, 3, 4))

        monster.sprite = Resources.Load<Sprite>("EnemyPrefabs/" + (chapter + 1) + "_" + stage); // 미리보기 이미지 로드
        
        string target = (chapter + 1).ToString() + "_" + stage; // 불러올 스테이지 이름

        for(int i = 0; i < eManager.enemyData.Count; i++)
        {
            if(eManager.enemyData[i].First().Value.ToString() == target)
            {
                exGoldText.text = ((int)eManager.enemyData[i]["RewardGold"] * (int)eManager.enemyData[i]["Count"]).ToString(); // 미리보기 골드 세팅
                exMsText.text = ((int)eManager.enemyData[i]["RewardManaStone"] * (int)eManager.enemyData[i]["Count"]).ToString(); // 미리보기 마력석 세팅
                break;
            }
        }
        monster.transform.gameObject.SetActive(true);
    }

    public void BtnRightClick() // 챕터 우측 버튼
    {
        chapter += 1;
        ChapterUpdate();
    }

    public void BtnLeftClick() // 챕터 좌측 버튼
    {
        chapter -= 1;
        ChapterUpdate();
    }

    public void BtnMoveClick() // 이동 버튼 클릭
    {
        StopCoroutine("OnFailText"); // 코루틴 중복 실행 방지
        StopCoroutine("OnNotiText");
        nowChapter = chapter; // 적용 챕터 세팅
        if(stage == "") StartCoroutine("OnFailText"); // 설정된 스테이지가 없을 경우
        else 
        {
            nowStage = (nowChapter + 1).ToString() + "_" + stage; // 스테이지 변경
            StartCoroutine("OnNotiText"); // 안내 문구 출력
        }
    }

    IEnumerator OnFailText() // 이동 실패 텍스트 Fading 코루틴
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

    IEnumerator OnNotiText() // 이동 안내 텍스트 Fading 코루틴
    {
        float nowAlpha = 1f;
        notiText.color = new Color(notiText.color.r, notiText.color.g, notiText.color.b, nowAlpha); // 알파값 1로 설정 == 텍스트 등장

        while(notiText.color.a >= 0) // 알파값 0이 될때까지 반복
        {
            yield return new WaitForSeconds(0.1f);
            nowAlpha -= 0.1f;
            notiText.color = new Color(notiText.color.r, notiText.color.g, notiText.color.b, nowAlpha);
        }
        yield break;
    }

    
}
