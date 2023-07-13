using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;//데미지
    public int per; //관통
    public float lifeTime = 5.0f;

    Rigidbody2D rigid;
    void Awake()
    {
        rigid= GetComponent<Rigidbody2D>();
    }
    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;
        if (per > -1)
        {
            rigid.velocity = dir*10f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //충돌한 것이 몬스터일 경우만 check
        if (!collision.CompareTag("Enemy") || per<0)
            return;

        per--;
        if (per < 0)
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
