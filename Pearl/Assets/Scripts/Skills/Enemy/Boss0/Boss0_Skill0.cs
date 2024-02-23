using UnityEngine;
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
    public float rushSpeed;
    public float minTargetSearchTime, maxTargetSearchTime;
    public float minRushDelay, maxRushDelay;
    public Transform DirIndicator;
    private float targetSearchTime;
    private float rushDelay;
    private void Start()
    {
        CheckStop();
        skillType = SkillType.rushAtk;
        targetSearchTime = Random.Range(minTargetSearchTime, maxTargetSearchTime);
        rushDelay = Random.Range(minRushDelay, maxRushDelay);
    }

}
