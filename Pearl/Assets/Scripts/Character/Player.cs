using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public Animator anim;
    [Header("Player curHp")]
    public float curHp;
    [Header("무적시간 (초)")]
    public float invincibleTime;
    [Header("(몬스터와 Player사이의)최소 거리")]

    public float minDistance;
    public TextMeshProUGUI playerHp;
    public Scanner scanner;

    // CharacterBase에서 가져올 데이터
    [Header("총기 id")] public int id;             
    [Header("공격력")] public int atk;
    [Header("총기 공격력")] public int gunAtk;
    [Header("총기 숙련도")] public int gunProficiency;
    [Header("총기 공격속도")] public float gunRapid;
    [Header("체력")] public int health;
    [Header("크리티컬 확률")] public int critical;
    [Header("크리티컬 데미지")] public int criticalDmg;
    [Header("이동속도")] public int moveSpeed;
    //

    private Rigidbody2D rb;
    private Transform trans;
    private float scale;
    private void Awake()
    {
        Init();
        rb = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
        anim = GetComponentInChildren<Animator>();
        scanner = GetComponent<Scanner>();
    }
    private void Start()
    {
        scale = trans.localScale.x;
        curHp = health;
    }
    private void Init()
    {
        id              = DataManager.Instance.id;
        atk             = DataManager.Instance.atk;
        gunAtk          = DataManager.Instance.gunAtk;
        gunProficiency  = DataManager.Instance.gunProficiency;
        gunRapid        = DataManager.Instance.gunRapid;
        health          = DataManager.Instance.health;
        critical        = DataManager.Instance.critical;
        criticalDmg     = DataManager.Instance.criticalDmg;
        moveSpeed       = DataManager.Instance.moveSpeed;
    }
    private void FixedUpdate()
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
        playerHp.text = "Health : "+curHp.ToString();
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
