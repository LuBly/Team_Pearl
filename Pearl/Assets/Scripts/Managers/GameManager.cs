using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //바로 메모리에 올려버린다.
    public static GameManager instance;
    public PoolManager pool;
    public Spawner spawner;
    public Player player;
    public Weapon weapon; // player의 무기정보를 가져올 변수
    public GameObject hud;
    public GameObject PauseMenu;
    public GameObject FailMenu;
    public GameObject ClearMenu;
    [Header("제한 시간")]
    public float LimitTime;

    [Header("처치 수")]
    public int killCount;

    [Header("목표 처치 수")]
    public int targetKillCount;

    public bool isFail = false;
    public bool isClear = false;
    // Fail의 조건
    // Time out_, 캐릭터 사망_
    // 
    void Awake()
    {
        instance = this;
        isFail = false;
        isClear = false;
    }

    private void Update()
    {
        if (isFail)
        {
            failStage();
        }

        if (isClear)
        {
            clearStage();
        }
    }

    public void failStage()
    {
        if(!isClear)
        {
            hud.SetActive(false);
            FailMenu.SetActive(true);
        }
    }

    public void clearStage()
    {
        if(!isFail)
        {
            hud.SetActive(false);
            ClearMenu.SetActive(true);
        }
    }

    public void activePauseMenu()
    {
        PauseMenu.SetActive(true);
    }
    public void continueStage()
    {
        PauseMenu.SetActive(false);
    }
    public void resetStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void exitStage()
    {
        if (isClear)
        {
            Debug.Log("stageManager에 클리어 여부 저장");
        }
        SceneManager.LoadScene("Idle");
        DataManager.Instance.Init();
    }

    
}
