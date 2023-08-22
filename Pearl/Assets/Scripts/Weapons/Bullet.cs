using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;            // 데미지
    public float knockbackPower;    // 넉백 파워
    public int count;               // 관통
    public float lifeTime;   // 총알이 사라지기까지의 시간 = 사정거리
    //총의 특성상 거리로 두는게 좋을 것 같다.
    //일단 임시 개발단계에선 시간으로 개발

    Rigidbody2D rigid;
    void Awake()
    {
        rigid= GetComponent<Rigidbody2D>();
    }
    public void Init(float damage, int count, float knockbackPower, float lifeTime, Vector3 dir)
    {
        this.damage = damage;
        this.count = count;
        this.knockbackPower = knockbackPower;
        this.lifeTime = lifeTime;
        
        if (count > -1)
        {
            //정해진 dir 방향으로 이동
            rigid.velocity = dir*10f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //충돌한 것이 몬스터일 경우만 check
        if (!collision.CompareTag("Enemy") || count<0)
            return;

        count--;
        if (count < 0)
        {
            //총알 초기화
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(Delete());
    }

    private void OnDisable()
    {
        StopCoroutine(Delete());
    }

    IEnumerator Delete()
    {
        yield return new WaitForSeconds(lifeTime);
        //총알 초기화
        rigid.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }
}
