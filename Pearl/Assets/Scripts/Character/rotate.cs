using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    PlayerStatus player;
    public Transform weaponBone;

    private void Awake()
    {
        player = GetComponentInParent<PlayerStatus>();
        
    }
    void idleRotate()
    {
        weaponBone.rotation = Quaternion.Euler(0, 0 ,0);
    }

    // 애니매이션에선 bone 15(총기의 rotation은 건드리지 않는다.)
    // rotation의 경우 모두 script에서 담당
    // animtaion에서 rotation을 담당하고 있는 경우
    // script에서 rotate를 변경하는 것이 적용 X
    // 모든 animation에서 rotation을 모두 삭제, script에서 rotation을 관리
    // 반동 - 랜덤 범위 해서 -0.5, 0.5 
}
