using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public Animator anim;
    // 여러개의 joystick 사용을 위해 조이스틱의 instance화 해제
    public JoystickMovement joystickMovement;
    [Header("Player curHp")]
    public float curHp;
    [Header("무적시간 (초)")]
    public float invincibleTime;
    
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
    // 총기 발사 방향 
    [Header("총기 발사 방향")]
    public AttackDirUI dirInput;
    private Rigidbody2D rb;
    private Transform trans;
    private float scale;
    private void Awake()
    {
        Init();
        rb = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
        anim = GetComponentInChildren<Animator>();
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
        if (joystickMovement.joyVec.x != 0 || joystickMovement.joyVec.y != 0)
        {
            rb.velocity = new Vector3(joystickMovement.joyVec.x, joystickMovement.joyVec.y,0)*moveSpeed;
        }
        else//x,y 둘다 0 일때 멈추기
        {
            rb.velocity = Vector3.zero;
        }
    }
    private void LateUpdate()
    {
        //조이스틱 값이 들어갈 때 run anim 상태로 변환
        if (joystickMovement.joyVec == Vector3.zero)
        {
            anim.SetBool("isAttack", false);
        }
        else
        {
            anim.SetBool("isAttack", true);
        }
        
        //공격 방향에 따른 좌우 반전
        if (dirInput.dir == Vector3.left)
        {
            trans.localScale = new Vector3(-scale, scale, 1.0f);
        }

        else if (dirInput.dir == Vector3.right)
        {
            trans.localScale = new Vector3(scale, scale, 1.0f);
        }

    }
}
