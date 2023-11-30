using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CounterUI : MonoBehaviour
{
    public TextMeshProUGUI counterText;
    public Image counterGauge;
    public GameManager gameManager;
    float killCount;
    float targetKillCount;

    private void Start()
    {
        targetKillCount = gameManager.targetKillCount;
    }
    void Update()
    {
        killCount = gameManager.killCount;
        if(killCount >= targetKillCount)
        {
            killCount = targetKillCount;
            gameManager.isClear = true;
        }
        counterGauge.fillAmount = killCount / targetKillCount;
        counterText.text = killCount.ToString() + " / " + targetKillCount.ToString();
    }
}
