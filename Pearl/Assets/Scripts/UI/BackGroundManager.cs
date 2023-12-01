using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundManager : MonoBehaviour
{
    public Image nowImage; // 현재 이미지 변수

    public void BackImageChange(int chapter) // 배경 이미지 변경 함수
    {
        nowImage.sprite = Resources.Load<Sprite>("Background/Chapter" + chapter);
    }
}
