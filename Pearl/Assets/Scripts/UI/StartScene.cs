using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using JetBrains.Annotations;

public class StartScene : MonoBehaviour
{
    public TextMeshProUGUI sText;
    void Start()
    {
        StartCoroutine(FadeInText());
    }

    IEnumerator FadeInText()
    {
        sText.alpha = 0;
        float fadeCount = 0.0f;
        while(fadeCount <= 1.0f)
        {
            fadeCount += 0.1f;
            yield return new WaitForSeconds(0.02f);
            sText.alpha = fadeCount;
        }
        StartCoroutine(FadeOutText());
    }

    IEnumerator FadeOutText()
    {
        sText.alpha = 1;
        float fadeCount = 1.0f;
        while(fadeCount >= 0f)
        {
            fadeCount -= 0.1f;
            yield return new WaitForSeconds(0.02f);
            sText.alpha = fadeCount;
        }
        StartCoroutine(FadeInText());
    }

    void Update()
    {
        if(Input.anyKeyDown)
        {
            if(SaveManager.isExsitSaveFile())
            {
                LoadingScene.LoadScene("Idle");
            }
            else
            {
                ClearManager.ResetClear();
                DataManager.Instance.stageInfo = "c1s1";
                DataManager.Instance.id = 101;
                DataManager.Instance.atk = 0;
                DataManager.Instance.gunAtk = 10;
                DataManager.Instance.gunProficiency = 0;
                DataManager.Instance.gunRapid = 660;
                DataManager.Instance.health = 200;
                DataManager.Instance.critical = 20;
                DataManager.Instance.criticalDmg = 50;
                DataManager.Instance.moveSpeed = 5;
                LoadingScene.LoadScene("IdleStage");
            }
        }
    }
}
