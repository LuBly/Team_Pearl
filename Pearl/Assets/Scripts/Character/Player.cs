using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Transform trans;
    Animator anim;
    [Header("Player 이동속도")]
    public float moveSpeed = 5f;
    [Header("Player MaxHp")]
    public float maxHp = 100;
    [Header("Player curHp")]
    public float curHp;
    [Header("무적시간 (초)")]
    public float invincibleTime;
    [Header("(몬스터와 Player사이의)최소 거리")]
    public float minDistance;
    public TextMeshProUGUI health;
    public Scanner scanner;
    private float scale;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        scale = trans.localScale.x;
        curHp = maxHp;
    }

    void FixedUpdate()
    {
        if (JoystickMovement.Instance.joyVec.x != 0 || JoystickMovement.Instance.joyVec.y != 0)
        {
            rb.velocity = new Vector3(JoystickMovement.Instance.joyVec.x, JoystickMovement.Instance.joyVec.y,0)*moveSpeed;
        }
        else//x,y 둘다 0 일때 멈추기
        {
            rb.velocity = Vector3.zero;
        }
    }
    private void LateUpdate()
    {
        health.text = "Health : "+curHp.ToString();
        //주변에 적이 있을 때 attack anim 상태로 변환
        if (scanner.nearestTarget)
        {
            float distance = Vector2.Distance(scanner.nearestTarget.position, trans.position);
            if(distance > minDistance)
                anim.SetBool("isAttack", true);
        }
        else
            anim.SetBool("isAttack", false);

        //조이스틱 값이 들어갈 때 run anim 상태로 변환
        if (JoystickMovement.Instance.joyVec == Vector3.zero)
        {
            anim.SetBool("isRun", false);
        }
        else
        {
            anim.SetBool("isRun", true);
        }
        
        //조이스틱 방향에 따른 좌우 반전
        if (JoystickMovement.Instance.joyVec.x != 0)
        {
            trans.localScale = JoystickMovement.Instance.joyVec.x > 0
                ? new Vector3(scale, scale, 1.0f) 
                : new Vector3(-scale, scale, 1.0f);
        }

    }
}
