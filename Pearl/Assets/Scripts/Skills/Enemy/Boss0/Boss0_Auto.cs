using UnityEngine;

public class Boss0_Auto : Skill
{
    public float bulletSpeed;
    public float skillRange;
    public float attackDelay;
    public GameObject bulletPrefab;

    private float curTime;
    private Scanner scanner;

    private void Awake()
    {
        scanner = GetComponent<Scanner>();
        curTime = 0;
        GameObject.FindWithTag("GM").GetComponent<GameManager>().pool.bossSkillBulletPrefab = bulletPrefab;
    }

    private void Start()
    {
        scanner.scanRange = skillRange;
        scanner.targetLayer = enemyLayer;
    }

    private void Update()
    {
        transform.position = caster.transform.position;

        curTime += Time.deltaTime;
        if (curTime >= attackDelay)
        {
            // 스캐너에 걸리는 몬스터가 있다면 
            if (scanner.nearestTarget != null)
            {
                Attack();
                curTime = 0f;
            }
        }
    }

    /*
     * Player를 향해 미사일이 날아감
     * 원형의 미사일이기 때문에 회전x
     * 일단 발사되면 플레이어에 맞을때까지 날아간다.
     * TrackingBullet.cs를 따로 만들지, bullet함수를 수정할지 고민
     * ... 데미지 처리 방식, bullet의 이동
     */
    private void Attack()
    {
        Transform bullet = GameObject.FindWithTag("GM")
                                     .GetComponent<GameManager>().pool
                                     .BossSkillBulletGet().transform;

        bullet.GetComponent<TrackingBullet>().Init(damage, bulletSpeed, enemyLayer);

    }
}
