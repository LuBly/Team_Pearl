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
    public static Dictionary<string, int> stageInfo = new Dictionary<string, int>();
    void Awake()
    {
        stageInfo.Add("c1s1", 0);
        stageInfo.Add("c1s2", 1);
        stageInfo.Add("c1s3", 2);
        stageInfo.Add("c1s4", 3);
        stageInfo.Add("c2s1", 4);
        stageInfo.Add("c2s2", 5);
        stageInfo.Add("c2s3", 6);
        stageInfo.Add("c2s4", 7);
        stageInfo.Add("c3s1", 8);
        stageInfo.Add("c3s2", 9);
        stageInfo.Add("c3s3", 10);
        stageInfo.Add("c3s4", 11);
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
            ClearManager.isClear[DataManager.Instance.stageInfo] = true;
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
