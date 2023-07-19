using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundManager : MonoBehaviour
{
    public Sprite[] backImage; // 배경 이미지 배열
    public SpriteRenderer nowImage; // 현재 이미지 변수

    void Start()
    {
        nowImage = GetComponent<SpriteRenderer>();
    }


    public void BackImageChange(int chapter) // 배경 이미지 변경 함수
    {
        switch(chapter)
        {
            case 1:
                nowImage.sprite = backImage[0];
                break;
            case 2:
                nowImage.sprite = backImage[1];
                break;
        }
    }
}
