using UnityEngine;
using UnityEngine.UIElements;
/// <summary>
/// 범위 내에 적이 들어오면 시전
/// 보스는 멈춰선다.
/// 
/// attackRange = Rush 공격을 시전할 플레이어와 보스와의 거리
/// rushSpeed = Rush 공격 시 움직이는 속도
/// targetSearchTime = indicator가 타겟을 서칭하는 시간
/// DirIndicator = 방향표시 indicator
/// rushDelay = indicator가 다 차고 돌진시작까지 걸리는 시간
/// </summary>
public class Boss0_Skill0 : Skill
{
    public EnemySkill enemySkill;
    public float rushSpeed;
    public float minTargetSearchTime, maxTargetSearchTime;
    public float minRushDelay, maxRushDelay;
    public Transform DirIndicatorBG;
    public Transform DirIndicator;
    public Transform Player;
    private float targetSearchTime;
    private float rushDelay;
    private bool targetSearch;
    private float curTime;
    private Vector3 DirOrig, DirFlip;
    
    private void OnEnable()
    {
        CheckStop();
        skillType = SkillType.rushAtk;
        targetSearchTime = Random.Range(minTargetSearchTime, maxTargetSearchTime);
        rushDelay = Random.Range(minRushDelay, maxRushDelay);
        targetSearch = false;
        curTime = 0;

        bool parentFlipped = transform.lossyScale.x < 0;

        // 자식 객체의 스케일을 조정하여 뒤집힘을 방지합니다.
        if (parentFlipped)
        {
            DirOrig = new Vector3(-DirIndicatorBG.transform.localScale.x, DirIndicatorBG.transform.localScale.y, DirIndicatorBG.transform.localScale.z);
            DirFlip = new Vector3(DirIndicatorBG.transform.localScale.x, DirIndicatorBG.transform.localScale.y, DirIndicatorBG.transform.localScale.z);
        }
        else
        {
            DirOrig = new Vector3(DirIndicatorBG.transform.localScale.x, DirIndicatorBG.transform.localScale.y, DirIndicatorBG.transform.localScale.z);
            DirFlip = new Vector3(-DirIndicatorBG.transform.localScale.x, DirIndicatorBG.transform.localScale.y, DirIndicatorBG.transform.localScale.z);
        }
    }

    private void Update()
    {
        if (!targetSearch)
        {
            // 플레이어를 향하는 방향 벡터를 계산
            Vector3 directionToPlayer = (transform.position - Player.position).normalized;
            // 방향 벡터를 각도로 변환
            float angleToPlayer = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            // DirIndicatorBG의 회전을 설정 Y축을 기준으로 플레이어의 방향을 바라보도록 설정
            DirIndicatorBG.rotation = Quaternion.Euler(0f, 0f, angleToPlayer);

            // 부모 객체의 스케일이 뒤집히는지 확인
            bool parentFlipped = transform.lossyScale.x < 0;

            // 자식 객체의 스케일을 조정하여 뒤집힘을 방지
            if (parentFlipped)
                DirIndicatorBG.localScale = DirFlip;
            else
                DirIndicatorBG.localScale = DirOrig;


            curTime += Time.deltaTime;

            if (curTime >= targetSearchTime)
            {
                targetSearch = true;
                // 이제 오브젝트가 이동을 시작해야 합니다. rushDelay 시간 후에 이동 시작합니다.
                Debug.Log("TargetFind");
                StartRush();
            }
        }
    }

    private void StartRush()
    {
        // 설정된 방향으로 오브젝트를 이동시킵니다.
        //transform.Translate(Vector2.forward * rushSpeed * Time.deltaTime);
        EndRush();
    }

    private void EndRush()
    {
        this.gameObject.SetActive(false);
        enemySkill.isSkillActive = false;
        EndSkill();
        StopAllCoroutines();
    }
}
