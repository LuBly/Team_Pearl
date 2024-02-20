using UnityEngine;

public enum SkillType
{
    defaultAtk,
    continuousAttack, // 지속 공격 ex) 제압사격
    grenadeAttack,    // 즉발 공격 ex) 수류탄
    snipperAttack,    // 범위 선택 공격 ex) 포격요청
    autoAttack,       // 자동 공격 ex) 드론 공격
}
public class Skill : MonoBehaviour
{
    [HideInInspector][SerializeField] public SkillType skillType;
    [HideInInspector][SerializeField] public GameManager gameManager;

    // 항상 사용
    public float damage;
    public float knockbackPower;
    public LayerMask enemyLayer;
    public bool isStopFire;
    
    private void Awake()
    {
        gameManager = GameObject.FindWithTag("GM").GetComponent<GameManager>();
    }

    private void Start()
    {
        if (isStopFire)
        {
            gameManager.isStopFire = true;
        }
    }

    private void OnDestroy()
    {
        if(gameManager != null)
            gameManager.isStopFire = false;
    }
}