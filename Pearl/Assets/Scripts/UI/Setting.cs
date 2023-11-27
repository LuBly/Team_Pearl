using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
설정 관련 스크립트입니다. 게임 종료와 설정 기능 구현이 들어갑니다.
*/

public class Setting : MonoBehaviour
{
    
    public void ExitBtnClick() // 게임 종료
    {
        Application.Quit();
    }
}
