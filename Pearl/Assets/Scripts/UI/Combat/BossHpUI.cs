using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHpUI : MonoBehaviour
{
    public TextMeshProUGUI BossHpText;
    public Image BossHpGauge;
    public Boss bossMonster;

    private float maxHealth;
    private float curHealth;
    private void Start()
    {
        maxHealth = bossMonster.maxHealth;
    }
    void Update()
    {
        curHealth = bossMonster.health;
        if (curHealth < 0)
        {
            curHealth = 0;
            GameObject.FindWithTag("GM").GetComponent<GameManager>().isClear = true;
        }
        BossHpGauge.fillAmount = curHealth / maxHealth;
        BossHpText.text = curHealth.ToString() + " / " + maxHealth.ToString();
    }
}

