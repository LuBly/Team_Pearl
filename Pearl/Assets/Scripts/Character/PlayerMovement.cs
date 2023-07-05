using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    SpriteRenderer spriter;
    Animator anim;
    public float moveSpeed = 5f;
    void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
        spriter=GetComponent<SpriteRenderer>();
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
        anim.SetFloat("Speed",rb.velocity.magnitude);

        if (JoystickMovement.Instance.joyVec.x != 0)
        {
            spriter.flipX = JoystickMovement.Instance.joyVec.x > 0;
        }
    }
}
