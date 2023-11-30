using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    float LimitTime;
    float time;
    float min;
    float sec;
    public TextMeshProUGUI timerText;
    public GameManager gameManager;
    private void Start()
    {
        LimitTime = gameManager.LimitTime;
    }
    void Update()
    {
        LimitTime -= Time.deltaTime;
        if (LimitTime <= 0)
        {
            gameManager.isFail = true;
            LimitTime = 0;
        }
        time = Mathf.Round(LimitTime);
        
        min = Mathf.Floor(time / 60);
        sec = time - min * 60;
        
        if (sec >= 10)
        {
            timerText.text = "0"+ min +" : " + sec;
        }   
        else timerText.text = "0" + min + " : 0" + sec;
    }
}
