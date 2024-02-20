using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SnipperAtk : Skill
{
    [Header("공격 범위")]
    public float attackRange;
    [Header("탄창")]
    public int fullAmmo;
    public int curAmmo;
    [Header("영역 전개 시간")]
    public float castTime;

    [Header("느려지는 정도 _ 낮을 수록 많이 느려집니다.")]
    [Range(0f, 1f)]
    public float targetDeltaTime;

    public GameObject fireEffect;
    public TextMeshProUGUI ammoText;
    
    private Image fillImage;

    private void Start()
    {
        skillType = SkillType.snipperAttack;
        curAmmo = fullAmmo;
        fillImage = GetComponent<Image>();
        StartCoroutine(ActiveArea());
    }

    // Update is called once per frame
    private void Update()
    {
        ammoText.text = curAmmo.ToString() + " / " + fullAmmo.ToString();
        if (curAmmo == 0)
        {
            DeActivateSkill();
        }
    }

    public void TouchPointAttack()
    {
        Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        curAmmo--;
        // touchPos에 이펙트 생성 및 데미지
        Instantiate(fireEffect, touchPos, Quaternion.identity, transform);

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(touchPos, attackRange, enemyLayer);
        foreach (Collider2D hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage, knockbackPower);
            }
        }
    }

    public void DeActivateSkill()
    {
        Destroy(this.gameObject);
        gameManager.hud.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        Time.timeScale = 1f;
    }

    // 포격요청 Area 선언 및 시간 정지
    IEnumerator ActiveArea()
    {
        float elapsedTime = 0f;
        float startFillAmount = 0f;
        float targetFillAmount = 1.1f; // 목표 fillAmount

        Time.timeScale = targetDeltaTime;

        while (elapsedTime < castTime)
        {
            fillImage.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, elapsedTime / castTime);
            elapsedTime += Time.deltaTime * (1 / Time.timeScale); // Time.timeScale 고려
            yield return null; // 한 프레임 기다립니다.
        }
    }
}
