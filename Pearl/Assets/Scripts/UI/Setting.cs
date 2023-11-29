using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

/*
설정 관련 스크립트입니다. 게임 종료와 설정 기능 구현이 들어갑니다.
*/

public class Setting : MonoBehaviour
{
    public SaveManager sManager;
    bool canExit;
    public void ExitBtnClick() // 게임 종료 버튼 클릭
    {
        canExit = true;
        sManager.Save();
    }

    public void isExit() // SaveDone 이벤트 호출 후 실행(save가 완료되기 전에 꺼지는 경우 방지)
    {
        if(canExit) Application.Quit();
        else return;
    }
}
