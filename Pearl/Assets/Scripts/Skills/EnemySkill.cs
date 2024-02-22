using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkill : MonoBehaviour
{
    public GameObject database;

    private SkillData dataSkill;
    private void Awake()
    {
        dataSkill = database.GetComponent<SkillData>();
    }
}
