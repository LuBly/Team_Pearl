using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HealthUI : MonoBehaviour
{
    public Image healthBar;
    public Player player;
    public TextMeshProUGUI healthText;
    float maxHealth;
    float curHealth;

    void Start()
    {
        maxHealth = player.health;
    }
    
    // Update is called once per frame
    void Update()
    {
        curHealth = player.curHp;
        if(curHealth <= 0)
        {
            curHealth = 0;
            GameManager.instance.isFail = true;
        }
        healthBar.fillAmount = curHealth / maxHealth;
        healthText.text = ((curHealth / maxHealth)*100).ToString() + "%";
    }
}
