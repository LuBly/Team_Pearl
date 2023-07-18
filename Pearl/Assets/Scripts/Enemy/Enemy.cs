using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    [Header("몬스터가 따라갈 Player")]
    public Rigidbody2D target;
    [Header("체력바 BackGround")]
    public GameObject hpBackground;
    [Header("체력바")]
    public Transform hpPercent;
    private bool isLive;

    Rigidbody2D rigid;
    Transform trans;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
    }

    private void FixedUpdate()
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
            hpBackground.transform.localScale = new Vector3(1, 1, 0);
        }
        //캐릭터가 몬스터 기준 오른쪽에 있는 경우 scale * -1
        else
        {
            trans.transform.localScale = new Vector3(-1, 1, 0);
            hpBackground.transform.localScale = new Vector3(-1, 1, 0);
        }
    }

    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        health = maxHealth;
        hpPercent.localScale = new Vector3(1, 1, 1);
    }
    //초기속성을 적용하는 함수
    public void Init(SpawnData data)
    {
        maxHealth = data.health;
        health = data.health;
        speed = data.speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet"))
            return;

        health -= collision.GetComponent<Bullet>().damage;

        if (health > 0)
        {
            // Live, HitAction
            // 몬스터의 체력바 조정
            hpPercent.localScale = new Vector3(health / maxHealth, 1, 1);
            
        }
        else
        {
            // Die
            Dead();
        }
    }

    private void Dead()
    {
        // 비활성화, 화면에 젠 되어 있는 EnemyCount --
        isLive = false;
        gameObject.SetActive(false);
        GameManager.instance.spawner.enemyCount--;
    }
}
