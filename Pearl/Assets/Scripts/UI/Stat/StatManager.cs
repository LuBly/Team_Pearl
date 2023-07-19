using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
인 게임 스탯 UI 관련해서 관리해주는 스크립트입니다.
*/

public class StatManager : MonoBehaviour
{
    CharacterBase stat;
    public TextMeshProUGUI speedText, atkText, gunAttackText, gunLevelText, healthText, criticalText, criticalDmgText;

    void Update()
    {
        stat = GameObject.Find("MainChar").GetComponent<CharacterBase>(); // 변수에 Main_Char 오브젝트의 CharacterBase 값 가져오기.

        speedText.text = stat.moveSpeed.ToString();
        atkText.text = stat.atk.ToString() + "%";
        gunAttackText.text = stat.gunAtk.ToString();
        gunLevelText.text = stat.gunProficiency.ToString();
        healthText.text = stat.health.ToString();
        criticalText.text = stat.critical.ToString();
        criticalDmgText.text = stat.criticalDmg.ToString();
    }

}
