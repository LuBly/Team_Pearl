using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//캔버스 위치 관리하는 스크립트 입니다.

public class UIMove : MonoBehaviour
{
    public bool startParentPos;
    Vector3 pos;

    void Start()
    {
        pos = transform.position;

        if( startParentPos )
        {
            SetParentPos();
        }
    }

    public void SetParentPos()
    {
        transform.position = transform.parent.position;
    }

    public void ReturnPos()
    {
        transform.position = pos;
    }
}
