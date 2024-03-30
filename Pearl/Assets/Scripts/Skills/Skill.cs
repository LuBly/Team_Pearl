using UnityEngine;

public enum SkillType
{
    rushAtk,          // 돌진 공격 ex) 돌진공격
    continuousAttack, // 지속 공격 ex) 제압사격
    grenadeAttack,    // 즉발 공격 ex) 수류탄
    snipperAttack,    // 범위 선택 공격 ex) 포격요청
    autoAttack,       // 자동 공격 ex) 드론 공격
}
public class Skill : MonoBehaviour
{
    [HideInInspector][SerializeField] public SkillType skillType;
    
    public GameObject caster;
    // 항상 사용
    public float damage;
    public float knockbackPower;
    // Boss Skill에서만 사용
    [Header("for BossSkill")]
    public float coolDown;
    public LayerMask enemyLayer;
    public bool isEnemy;
    public bool isStop;
    
    public void CheckStop()
    {
        if (isStop)
        {
            if (isEnemy)
            {
                caster.GetComponent<Enemy>().isStop = true;
            }
            else
            {
                GameObject.FindWithTag("GM")
                          .GetComponent<GameManager>().isStopFire = true;
            }
        }
    }
    public void EndSkill()
    {
        if(isEnemy)
        {
            caster.GetComponent<Enemy>().isStop = false;
        }
        else
        {
            if (GameObject.FindWithTag("GM").GetComponent<GameManager>() != null)
                GameObject.FindWithTag("GM").GetComponent<GameManager>().isStopFire = false;
        }
    }
}