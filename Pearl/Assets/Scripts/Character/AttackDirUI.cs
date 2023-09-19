using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 공격 방향을 설정하는 UI
/// 누르는 방향으로 공격을 발사한다.
/// 캐릭터 회전 = 공격 방향
/// </summary>

public class AttackDirUI : MonoBehaviour
{
    public Vector3 dir = Vector3.right;

    public void leftFire()
    {
        dir = Vector3.left;
    }
    public void rightFire()
    {
        dir = Vector3.right;
    }
}
