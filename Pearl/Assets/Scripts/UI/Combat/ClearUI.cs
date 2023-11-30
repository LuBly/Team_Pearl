using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mono.Cecil;

public class ClearUI : MonoBehaviour
{
    public TextMeshProUGUI clearMessage;
    public TextMeshProUGUI countMessage;
    public GameManager gameManager;
    public float countDownTime;
    private void OnEnable()
    {
        clearMessage.text = "스테이지 " + (gameManager.stageInfo[DataManager.Instance.stageInfo] + 1).ToString() + " 클리어";
        StartCoroutine("CountDown");
    }

    IEnumerator CountDown()
    {
        for(float time = countDownTime; time >= 0; time--)
        {
            countMessage.text = "메인화면으로 돌아가시겠습니까? (" + time.ToString() + ")";
            yield return new WaitForSecondsRealtime(1f);
        }

        gameManager.exitStage();
    }
}
