using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
몬스터 드롭 보상 제거용 스크립트입니다.
RewardMove 애니메이션에 이벤트 트리거로 동작합니다.
Reward 프리펩 내부에 삽입
*/

public class RemoveReward : MonoBehaviour
{
    public GameObject reward;
    public void Remove()
    {
        Destroy(reward);
    }
}
