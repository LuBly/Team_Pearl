using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CounterUI : MonoBehaviour
{
    public TextMeshProUGUI counterText;
    public Image counterGauge;
    float killCount;
    float targetKillCount;

    private void Start()
    {
        targetKillCount = GameManager.instance.targetKillCount;
    }
    void Update()
    {
        killCount = GameManager.instance.killCount;
        counterGauge.fillAmount = killCount / targetKillCount;
        counterText.text = killCount.ToString() + " / " + targetKillCount.ToString();
    }
}
