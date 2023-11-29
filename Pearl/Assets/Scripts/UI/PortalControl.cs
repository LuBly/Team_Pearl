using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class PortalControl : MonoBehaviour
{
    public GameObject pChapter;
    IdleEnemy Stage;
    public GameObject BtnLeft, BtnRight;
    public TextMeshProUGUI ChapterText;
    public SaveManager sManager;
    int now = 1;

    void Start()
    {
        Stage = GameObject.Find("MainEnemy").GetComponent<IdleEnemy>();
    }

    public void PortalClick()
    {
        ClearCheck();
        now = Stage.nowChapter;
        if(now == 1)
        {
            BtnLeft.SetActive(false);
            if(!ClearManager.isClear["c1s4"]) BtnRight.SetActive(false);
            else BtnRight.SetActive(true);
            ChapterText.text = "Chapter 1";
        }
        else if(now == 2)
        {
            BtnLeft.SetActive(true);
            if(ClearManager.isClear["c2s4"]) BtnRight.SetActive(true);
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
        for(int i = 1; i <= 3; i++) // 스테이지 수
        {
            if(ClearManager.isClear["c" + now + "s" + i])
            {
                pChapter.transform.GetChild(i).gameObject.SetActive(true);
            }
            else pChapter.transform.GetChild(i).gameObject.SetActive(false);
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

    public void PortalBtnClick()
    {
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        String pStage = "c" + now.ToString() + "s" + clickObject.name;
        sManager.Save();
        DataManager.Instance.stageInfo = pStage;
        DataManager.Instance.Load();
        LoadingScene.LoadScene("IdleStage");
    }
}
