using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [Header("몬스터가 따라갈 Player")]
    public Rigidbody2D target;

    [Header("이동속도")]
    public float speed;
    
    [Header("현재 체력")]
    public float health;

    [Header("최대 체력")]
    public float maxHealth;

    [Header("공격력")]
    public float damage;

    private float knockbackPower;
    private bool isLive;
    private bool isPlayerInRange = true;
    private bool isSkillAttack = true;

    Animator anim;
    Rigidbody2D rigid;
    Transform trans;
    Transform originalTransform;
    WaitForFixedUpdate wait;
    GameManager gameManager;
    Vector3 EnemyOrig, EnemyFlip;
    
    private void Awake()
    {
        gameManager = GameObject.FindWithTag("GM").GetComponent<GameManager>();
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        originalTransform = GetComponent<Transform>();
        trans = GetComponentInChildren<Transform>();
        wait = new WaitForFixedUpdate();
    }
    private void Start()
    {
        EnemyOrig = new Vector3(trans.transform.localScale.x, trans.transform.localScale.y, trans.transform.localScale.z);
        EnemyFlip = new Vector3(-trans.transform.localScale.x, trans.transform.localScale.y, trans.transform.localScale.z);
        target = gameManager.player.GetComponent<Rigidbody2D>();
        isLive = true;
        health = maxHealth;
    }
    private void FixedUpdate()
    {
        //Enemy가 죽거나 피격당하고 있을 때 이동 Update를 잠시 멈춘다.
        if(!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;
            
        //캐릭터의 위치 - 몬스터의 위치 = 방향
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;//프레임 상관없이 일정 거리 이동
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;//물리 속도가 이동에 영향을 주지 않도록 속도 제거
    }

    private void LateUpdate()
    {
        //캐릭터와 몬스터 사이의 거리가 0.005보다 낮을경우 계속 flip하여 몬스터의 모습이 어색함.
        //이를 방지하기 위해 target(PlayerStatus)과 rigid(Enemy)의 거리가 0.01보다 클 때만 flip하도록 설정
        float distance = Vector2.Distance(target.position, rigid.position);
        if (distance > 0.05f)
        {
            //캐릭터가 몬스터 기준 왼쪽에 있는 경우 scale 그대로
            //캐릭터가 몬스터 기준 오른쪽에 있는 경우 scale * -1
            if (target.position.x > rigid.position.x)
            {
                trans.transform.localScale = EnemyFlip;
            }
            else
            {
                trans.transform.localScale = EnemyOrig;
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //피격 당할 때
        if (collision.CompareTag("Bullet"))
        {
            health -= collision.GetComponent<Bullet>().damage;
            knockbackPower = collision.GetComponent<Bullet>().knockbackPower;
            if (health > 0)
            {
                Hit();
            }

            else
            {
                // Die
                Dead();
            }
        }

        // 스킬 피격
        if (collision.CompareTag("Skill"))
        {
            Skill curSkill = collision.GetComponent<Skill>() ? collision.GetComponent<Skill>() : collision.GetComponentInParent<Skill>();
            switch (curSkill.skillType)
            {
                case SkillType.continuousAttack:
                    break;
                case SkillType.grenadeAttack:
                    health -= curSkill.damage;
                    knockbackPower = curSkill.knockbackPower;
                    if (health > 0)
                    {
                        Hit();
                    }

                    else
                    {
                        // Die
                        Dead();
                    }
                    break;
            }
        }

        //플레이어를 공격할 때
        /*
         * 플레이어를 공격할 수 있는 범위인 atked Range와 충돌
         * playerInRange Trigger를 실행하여 EnemyAtk anim를 실행
         */
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("isAttack", true);
        }

    }
    IEnumerator KnockBack()
    {
        // 하나의 물리 프레임 딜레이
        yield return wait;

        // 플레이어 위치와 반대 방향으로 넉백
        Vector3 playerPos = gameManager.player.transform.position;
        Vector3 dirVec = originalTransform.position - playerPos;
        rigid.AddForce(dirVec.normalized * knockbackPower, ForceMode2D.Impulse);
    }

    // Enemy 공격 로직
    // 1. 플레이어의 피격 범위에 몬스터가 진입
    // 2. 플레이어의 피격 범위 내에 몬스터가 있는 동안 EnemyAttack Coroutine 실행
    // 3. 몬스터의 공격속도에 따라 플레이어의 hp 감소
    // 4. 피격 범위 밖으로 나갈 경우 공격모션을 종료
    // 5. EnemyAttack Coroutine 종료
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isPlayerInRange&&isLive)
            {
                StartCoroutine("EnemyAttack");
            }
        }

        //피격 당할 때
        // 스킬 피격
        if (collision.CompareTag("Skill"))
        {
            Skill curSkill = collision.GetComponent<Skill>() ? collision.GetComponent<Skill>() : collision.GetComponentInParent<Skill>();
            switch (curSkill.skillType)
            {
                case SkillType.continuousAttack:
                    if (isSkillAttack)
                    {
                        StartCoroutine(skillAttack(collision));
                    }
                    break;
                case SkillType.grenadeAttack:
                    break;
            }
        }
    }
    public void TakeDamage(float damage, float knockbackPower)
    {
        Debug.Log("IM hit");
        health -= damage;
        this.knockbackPower = knockbackPower;
        if (health > 0)
        {
            Hit();
        }

        else
        {
            // Die
            Dead();
        }
    }

    IEnumerator skillAttack(Collider2D collision)
    {
        isSkillAttack = false;
        health -= collision.GetComponent<Skill>().damage;
        knockbackPower = collision.GetComponent<Skill>().knockbackPower;
        if (health > 0)
        {
            // 피격시 몬스터 Hit animation 추가
            anim.SetTrigger("Hit");
            StartCoroutine("KnockBack");
        }

        else
        {
            // Die
            Dead();
        }
        yield return new WaitForSeconds(collision.GetComponent<Skill>().attackTime);
        isSkillAttack = true;
    }

    /*
     피해당하는 로직을 Player로 이식 필요
     */
    IEnumerator EnemyAttack()
    {
        isPlayerInRange = false;
        yield return new WaitForSecondsRealtime(1f);//몬스터 공격속도
        gameManager.player.curHp -= damage;
        gameManager.player.anim.SetTrigger("hurt");
        isPlayerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("isAttack", false);
            StopCoroutine("EnemyAttack");
            isPlayerInRange = true;
        }
    }

    private void Hit()
    {
        // 피격시 몬스터 Hit animation 추가
        anim.SetTrigger("Hit");
        StartCoroutine("KnockBack");
    }

    private void Dead()
    {
        // 비활성화, 화면에 젠 되어 있는 EnemyCount --
        isLive = false;
        StopCoroutine("EnemyAttack");
        isPlayerInRange = true;
        Destroy(this.gameObject);
        gameManager.killCount++;
    }
}
