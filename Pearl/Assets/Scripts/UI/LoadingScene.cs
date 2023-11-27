using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public static string nextScene;
    public Slider loadBar;

    private void Start()
    {
        StartCoroutine(OnLoad());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator OnLoad()
    {
        float timer = 0.0f;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false; // 로딩 종료 후, 즉시 시작 방지
        
        while(!op.isDone)
        {
            yield return null;
            
            if(op.progress < 0.9f || loadBar.value < 0.9f) // 로딩 90%이하 로딩바 벨류를 같이 묶지 않으면, 로딩이 너무 빠를 경우, 로딩이 부자연스러워짐(로딩 씬 전환과 동시에 로딩바 100%)
            {
                timer += Time.unscaledDeltaTime; // 고정 시간값
                loadBar.value = Mathf.Lerp(loadBar.value, op.progress, timer);
                if(loadBar.value >= 0.9f) 
                {
                    timer = 0; // 타이머 초기화(초기화가 없을 경우, 90% 이후 너무 빠르게 로딩 게이지가 진행됨.
                    yield return new WaitForSecondsRealtime(2.0f); // 로딩 90% 완료 후 2초 대기(로딩이 너무 순식간에 끝나는 경우 방지)
                }
            }
            else // 로딩 90% 이상 완료
            {
                timer += Time.unscaledDeltaTime; // 고정 시간값
                loadBar.value = Mathf.Lerp(loadBar.value, 1f, timer);
                if(loadBar.value >= 1.0f)
                {
                    op.allowSceneActivation = true; // 씬 시작 허용
                    yield break;
                }
            }
        }
    }
}
