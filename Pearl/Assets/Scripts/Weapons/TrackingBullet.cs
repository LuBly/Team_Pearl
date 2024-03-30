using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class TrackingBullet : MonoBehaviour
{
    private float damage = 0;
    private float bulletSpeed = 0;
    private LayerMask enemyLayer = 999;
    private Vector3 dir;
    private Rigidbody2D rigid;
    private void OnEnable()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (damage == 0 || bulletSpeed == 0 || enemyLayer == 999) return;

        dir = GameObject.FindWithTag("GM").GetComponent<GameManager>().player.transform.position
            - transform.position;

        dir = dir.normalized;
        rigid.velocity = dir * bulletSpeed;
    }


    public void Init(float damage, float bulletSpeed, LayerMask enemyLayer)
    {
        this.damage = damage;
        this.bulletSpeed = bulletSpeed;
        this.enemyLayer = enemyLayer;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //충돌한 것이 몬스터일 경우만 check
        if (collision.CompareTag("Player"))
        {
            // Enemy에게 피해
            collision.gameObject.GetComponent<PlayerStatus>().TakeDamage(damage);
            // 총알 초기화
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}
