using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    /*
     * 몬스터가 캐릭터 주변에 왔는지 파악하는 class
     * 가장 가까운 적에게 공격
    */

    public float scanRange;         // 범위
    public LayerMask targetLayer;   // 레이어
    public RaycastHit2D[] targets;  // 스캔 결과 배열
    public Transform nearestTarget; // 가장 가까운 타겟

    void FixedUpdate()
    {
        //범위 탐색을 시작할 위치, 원의 반지름, 범위 탐색 방향, 범위 탐색 길이, 대상 레이어
        targets=Physics2D.CircleCastAll(transform.position,scanRange,Vector2.zero,0,targetLayer);
        nearestTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 3000;//비교 기준
        
        foreach(RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float distance = Vector3.Distance(myPos, targetPos);
            
            //설정된 최소거리보다 monster와의 거리가 짧으면 공격X
            //Bullet이 이상하게 나가는 현상 방지용
            if (GameManager.instance.player.minDistance > distance) continue;
            Vector2 dir = targetPos - myPos;
            float angle;
            if (transform.localScale.x < 0)//왼쪽을 바라보고 있을 때
            {
                angle = Vector2.SignedAngle(-transform.right, dir);
            }
            else
            {
                angle = Vector2.SignedAngle(transform.right, dir);
            }

            if (angle >= -60f && angle <= 60f)
            {
                float curDiff = Vector3.Distance(myPos, targetPos);
                if (curDiff < diff)
                {
                    diff = curDiff;
                    result = target.transform;
                }
            }
        }
        return result;
    }
}
