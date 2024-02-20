using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrenadeAtk : Skill
{
    public Scanner scanner;
    public GameObject skillRange;
    public GameObject attackPoint;
    public GameObject skillImpact;
    public JoystickMovement skillJoystickMovement;

    private void Awake()
    {
        scanner = GameObject.FindWithTag("Player").GetComponent<Scanner>();
        skillJoystickMovement = GameObject.FindWithTag("Skill_Grenade").GetComponent<JoystickMovement>();
    }

    private void Start()
    {
        skillType = SkillType.grenadeAttack;

        bool isDrag = skillJoystickMovement.isDrag;
        // Drag 공격
        if (isDrag)
        {
            skillRange.SetActive(true);
            attackPoint.SetActive(true);
            attackPoint.GetComponent<CapsuleCollider2D>().enabled = false;
        }

        // 일반 공격
        else
        {
            if (scanner.nearestTarget)
            {
                attackPoint.SetActive(true);
                skillImpact.SetActive(true);
                attackPoint.transform.position = scanner.nearestTarget.position;

                //쿨타임 돌게 신호 전달
            }
            else
            {
                Debug.Log("No Enemy");
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (skillJoystickMovement.isDrag == true)
        {
            // 방향 (JoyVec)
            // 크기 L2 = L1*R/r
            attackPoint.transform.position =
                skillRange.transform.position
                + skillJoystickMovement.joyVec
                * skillJoystickMovement.stickDistance
                / 20f;
        }
    }

    private void OnDestroy()
    {
        attackPoint.SetActive(false);
        skillImpact.SetActive(false);
        skillRange.SetActive(false);
        StopCoroutine(ActiveInstantSkill());
        attackPoint.GetComponent<CapsuleCollider2D>().enabled = true;
    }

    /*
     * 1. attackPoint 내부에 적이있다
     * 2. 모든 enemy TakeDamage
     * 3. 이펙트 생성
     * 4. Destroy
     */

    public void DragSkillFire()
    {
        StartCoroutine(ActiveInstantSkill());
    }

    IEnumerator ActiveInstantSkill()
    {
        attackPoint.GetComponent<CapsuleCollider2D>().enabled = true;
        skillImpact.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        attackPoint.GetComponent<CapsuleCollider2D>().enabled = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // 스킬 피격
        if (collision.CompareTag("Enemy"))
        {
            skillAttack(collision);
        }
    }

    private void skillAttack(Collider2D collision)
    {
        collision.GetComponentInParent<Enemy>().TakeDamage(damage, knockbackPower);
    }

    
}
