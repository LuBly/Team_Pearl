using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//캔버스 위치 관리하는 스크립트 입니다.

public class UIMove : MonoBehaviour
{
    public bool Start_Parent_Pos;
    Vector3 pos;

    void Start()
    {
        pos = transform.position;

        if( Start_Parent_Pos )
        {
            SetParent_Pos();
        }
    }

    public void SetParent_Pos()
    {
        transform.position = transform.parent.position;
    }

    public void Return_Pos()
    {
        transform.position = pos;
    }
}
