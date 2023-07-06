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
        else//x,y �Ѵ� 0 �϶� ���߱�
        {
            rb.velocity = Vector3.zero;
        }
    }
    private void LateUpdate()
    {
        //���̽�ƽ ���� �� �� run anim ���·� ��ȯ
        if (JoystickMovement.Instance.joyVec == Vector3.zero)
        {
            anim.SetBool("isRun", false);
        }
        else
        {
            anim.SetBool("isRun", true);
        }
        
        //���̽�ƽ ���⿡ ���� �¿� ����
        if (JoystickMovement.Instance.joyVec.x != 0)
        {
            trans.localScale = JoystickMovement.Instance.joyVec.x > 0
                ? new Vector3(scale, scale, 1.0f) 
                : new Vector3(-scale, scale, 1.0f);
        }
    }
}
