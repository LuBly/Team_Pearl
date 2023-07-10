using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed;
    public Rigidbody2D target;
    
    bool isLive;

    Rigidbody2D rigid;
    Transform trans;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        //캐릭터의 위치 - 몬스터의 위치 = 방향
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;//프레임 상관없이 일정 거리 이동
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;//물리 속도가 이동에 영향을 주지 않도록 속도 제거
    }

    private void LateUpdate()
    {
        //캐릭터가 몬스터 기준 왼쪽에 있는 경우 scale 그대로
        if(target.position.x < rigid.position.x)
        {
            trans.transform.localScale = new Vector3(1, 1, 0);
        }
        //캐릭터가 몬스터 기준 오른쪽에 있는 경우 scale * -1
        else
        {
            trans.transform.localScale = new Vector3(-1,1,0);
        }
    }
}
