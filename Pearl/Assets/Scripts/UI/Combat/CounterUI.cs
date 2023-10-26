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
    // Update is called once per frame
    void Update()
    {
        killCount = GameManager.instance.killCount;
        counterGauge.fillAmount = killCount / 100f;
        counterText.text = killCount.ToString() + " / 100";
    }
}
