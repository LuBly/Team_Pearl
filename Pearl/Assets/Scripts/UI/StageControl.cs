using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class StageControl : MonoBehaviour
{
    IdleEnemy Stage;
    GameObject BtnLeft, BtnRight;
    public TextMeshProUGUI ChapterText;
    int now = 1;

    void Start()
    {
        Stage = GameObject.Find("MainEnemy").GetComponent<IdleEnemy>();
        BtnLeft = GameObject.Find("BtnLeft");
        BtnRight = GameObject.Find("BtnRight");
    }

    public void StageClick()
    {
        ClearCheck();
        now = Stage.nowChapter;
        if(now == 1)
        {
            BtnLeft.SetActive(false);
            if(!ClearManager.isClear["c2s1"]) BtnRight.SetActive(false);
            else BtnRight.SetActive(true);
            ChapterText.text = "Chapter 1";
        }
        else if(now == 2)
        {
            BtnLeft.SetActive(true);
            if(ClearManager.isClear["c3s1"]) BtnRight.SetActive(true);
            ChapterText.text = "Chapter 2";
        }
        else if(now == 3)
        {
            BtnRight.SetActive(false);
            ChapterText.text = "Chapter 3";
        }
    }

    void ClearCheck()
    {
        for(int i = 0; i < 4; i++) // 스테이지 수
        {
            if(ClearManager.isClear["c" + now + "s" + (i + 1)])
            {
                GameObject.Find("Chapter").transform.GetChild(i).gameObject.SetActive(true);
            }
            else GameObject.Find("Chapter").transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void BtnLeftClick()
    {
        now = now - 1;
        if(now == 1)
        {
            BtnLeft.SetActive(false);
            ChapterText.text = "Chapter 1";
        }
        else if(now == 2)
        {
            BtnLeft.SetActive(true);
            BtnRight.SetActive(true);
            ChapterText.text = "Chapter 2";
        }
    }

    public void BtnRightClick()
    {
        now = now + 1;
        if(now == 2)
        {
            BtnLeft.SetActive(true);
            BtnRight.SetActive(true);
            ChapterText.text = "Chapter 2";
        }
        else if(now == 3)
        {
            BtnRight.SetActive(false);
            ChapterText.text = "Chapter 3";
        }
    }

    public void StageBtnClick()
    {
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        if(now == 1)
        {
            Stage.nowStage = int.Parse(clickObject.name);
            Stage.nowChapter = now;
            Stage.Stoproutine();
            Stage.Start();
        }
        else if(now == 2)
        {
            Stage.nowStage = int.Parse(clickObject.name);
            Stage.nowChapter = now;
            Stage.Stoproutine();
            Stage.Start();
        }
        else if(now == 3)
        {
            Stage.nowStage = int.Parse(clickObject.name);
            Stage.nowChapter = now;
            Stage.Stoproutine();
            Stage.Start();
        }
    }
}
