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
        //ĳ������ ��ġ - ������ ��ġ = ����
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;//������ ������� ���� �Ÿ� �̵�
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;//���� �ӵ��� �̵��� ������ ���� �ʵ��� �ӵ� ����
    }

    private void LateUpdate()
    {
        //ĳ���Ͱ� ���� ���� ���ʿ� �ִ� ��� scale �״��
        if(target.position.x < rigid.position.x)
        {
            trans.transform.localScale = new Vector3(1, 1, 0);
        }
        //ĳ���Ͱ� ���� ���� �����ʿ� �ִ� ��� scale * -1
        else
        {
            trans.transform.localScale = new Vector3(-1,1,0);
        }
    }
}
