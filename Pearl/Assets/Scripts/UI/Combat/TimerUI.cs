using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [SerializeField]
    [Header("제한 시간")]
    private float LimitTime = 10f;
    float time;
    float min;
    float sec;
    public TextMeshProUGUI textTimer;
    void Update()
    {
        LimitTime -= Time.deltaTime;
        time = Mathf.Round(LimitTime);
        
        min = Mathf.Floor(time / 60);
        sec = time - min * 60;
        
        if (sec >= 10)
        {
            textTimer.text = "0"+ min +" : " + sec;
        }   
        else textTimer.text = "0" + min + " : 0" + sec;
    }
}
