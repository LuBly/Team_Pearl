using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
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
    public float minRushRatio, maxRushRatio;

    public Transform DirIndicatorBG;
    public Transform DirIndicator;
    public Transform Player;


    private float targetSearchTime;
    private float rushDelay;
    private float rushRatio;
    private float curTime;

    private bool targetSearch;
    private bool isRushStart;

    private Vector3 targetPosition;
    
    private void OnEnable()
    {
        CheckStop();
        skillType = SkillType.rushAtk;
        targetSearchTime = Random.Range(minTargetSearchTime, maxTargetSearchTime);
        rushDelay = Random.Range(minRushDelay, maxRushDelay);
        rushRatio = Random.Range(minRushRatio, maxRushRatio);
        transform.position = caster.transform.position + new Vector3(0, -1, 0);

        DirIndicator.localScale = new Vector3(1, 0, 0);
        DirIndicatorBG.gameObject.SetActive(true);
        targetSearch = false;
        isRushStart = false;
        curTime = 0;
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
            transform.rotation = Quaternion.Euler(0f, 0f, angleToPlayer);
            curTime += Time.deltaTime;

            if (curTime >= targetSearchTime)
            {
                targetPosition = Player.position;
                targetSearch = true;
                StartCoroutine(RushCountDown());   
            }
        }

        if (isRushStart)
        {
            StartRush(targetPosition);
        }
    }

    private IEnumerator RushCountDown()
    {
        float elapsedTime = 0f;
        float totalRushDelay = rushDelay;

        while (elapsedTime < totalRushDelay)
        {
            float scaleFactor = Mathf.Clamp01(elapsedTime / totalRushDelay);
            DirIndicator.localScale = new Vector3(1, scaleFactor, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        DirIndicator.localScale = new Vector3(1, 1, 0);
        isRushStart = true;
    }

    private void StartRush(Vector3 targetPosition)
    {
        DirIndicatorBG.gameObject.SetActive(false);
        caster.transform.position = Vector3.MoveTowards(caster.transform.position, targetPosition * rushRatio, rushSpeed * Time.deltaTime);

        // 달리기가 목표 지점에 도달했다면 스킬 종료.
        float dis = Vector3.Distance(targetPosition * rushRatio, caster.transform.position);
        
        if (dis < 1.05f)
        {
            EndRush();
        }
    }
    private void EndRush()
    {
        enemySkill.isSkillActive = false;
        EndSkill();
        StopAllCoroutines();
        this.gameObject.SetActive(false);
    }
}
