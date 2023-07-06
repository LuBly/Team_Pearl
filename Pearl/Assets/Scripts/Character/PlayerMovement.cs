using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerMovement>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("PlayerMovement");
                    instance = instanceContainer.AddComponent<PlayerMovement>();
                }
            }
            return instance;
        }
    }
    private static PlayerMovement instance;

    Rigidbody2D rb;
    Transform trans;
    Animator anim;
    public float moveSpeed = 5f;
    private float scale = 0.7f;
    void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
        anim=GetComponent<Animator>();
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
