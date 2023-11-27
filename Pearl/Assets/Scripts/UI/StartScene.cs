using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

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
            LoadingScene.LoadScene("Idle");
        }
    }
}
