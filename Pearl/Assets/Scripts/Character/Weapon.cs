using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    enum weaponType
    {
        AR = 1,
        SG = 2,
        SR = 3
    }

    /*
     * 무기 관리를 위한 Script
     */
    public int weaponIdx;                       // 무기 종류(AR,SG,SR)
    public int prefabId;                        // 총알 종류(SR1,SR2~~)
    public float knockbackPower;                // 총알 넉백 파워
    public float damage;                        // 총알 데미지
    public float gunRapid;                      // 공격 속도(몇 초에 한번 발사하는가)
    public int count;                           // 관통력(체크용)
    public GameManager gameManager;
    [Header("개발용 데이터")]
    [Header("AR 사정거리 및 적 스캔범위")]
    public float AssertRifleLifeTime = 0.5f;    // AR 사정거리 
    //public float AssertRifleRange = 5.0f;       // AR 적 스캔범위
    
    [Header("SG 사정거리 및 적 스캔범위")]
    public float ShotGunlifeTime = 0.25f;       // SG 사정거리
    //public float ShotGunRange = 2.5f;           // SG 적 스캔범위

    [Header("SR 사정거리 및 적 스캔범위")]
    public float SniperRifleLifeTime = 0.75f;   // SR 사정거리
    //public float SniperRifleRange = 7.5f;       // SR 적 스캔범위

    [Header("삿건 총알간 각도 (단위 : 도)")]
    public float ShotGunAngle = 10f;            // SG 총알간 각도

    //[Header("총기 각도 담당 Bone")]
    //public Transform weaponBone;

    float timer = 0f;
    PlayerStatus player;

    private void Awake()
    {
        player = GetComponentInParent<PlayerStatus>();
    }
    
    private void Start() 
    {
        Init();
    }
    
    private void Update()
    {
        switch (weaponIdx)
        {
            case (int)weaponType.AR:
                timer += Time.deltaTime;
                if (timer > gunRapid)
                {
                    RifleFire();
                    timer = 0f;
                }
                break;
            case (int)weaponType.SG:
                timer += Time.deltaTime;
                if (timer > gunRapid)
                {
                    ShotGunFire();
                    timer = 0f;
                }
                break;
            case (int)weaponType.SR:
                timer += Time.deltaTime;
                if (timer > gunRapid)
                {
                    RifleFire();
                    timer = 0f;
                }
                break;
        }
    }

    public void Init()
    {
        weaponIdx = player.id / 100;
        prefabId = player.id % 100;
        damage = player.gunAtk;
        gunRapid = player.gunRapid / 1000f; 
        switch (weaponIdx)
        {
            case (int)weaponType.AR://AR
                //player.scanner.scanRange = AssertRifleRange;
                count = 0;
                break;
            case (int)weaponType.SG://SG
                //player.scanner.scanRange = ShotGunRange;
                count = 0;
                break;
            case (int)weaponType.SR://SR
                //player.scanner.scanRange = SniperRifleRange;
                count = 2;
                break;
        }
    }

    //총알을 BulletManager에서 가지고 오면 된다.
    void RifleFire()
    {
        // 수정 -> 무조건 발사
        // Vector3 targetPos = player.scanner.nearestTarget.position;
        // 속도(크기를 포함한 방향벡터) = 목표 위치 - 내 위치
        Vector3 dir = player.dirInput.dir;
        dir = dir.normalized;//정규화
        Transform bullet = gameManager.pool.BulletGet(prefabId).transform;
        
        // 총알의 시작지점
        bullet.position = transform.position;
        
        // 총알이 바라보는 방향(dir = 캐릭터 -> 몬스터 방향)
        bullet.rotation = Quaternion.FromToRotation(Vector3.down, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, knockbackPower, AssertRifleLifeTime, dir);
    }
    
    void ShotGunFire()
    {   
        Vector3 dir = player.dirInput.dir;
        dir = dir.normalized;//정규화
        Vector3[] dirs = new Vector3[5];
        // 총알 발사 각도 계산 및 저장
        dirs[0]= dir;
        dirs[1]= Quaternion.Euler(0,0,ShotGunAngle)*dir;
        dirs[2]= Quaternion.Euler(0,0,-ShotGunAngle)*dir;
        dirs[3]= Quaternion.Euler(0,0,ShotGunAngle*2)*dir;
        dirs[4]= Quaternion.Euler(0,0,-ShotGunAngle*2)*dir;
        
        /*
        // 총구 회전
        if (dir.x > 0)
        {
            weaponBone.rotation = Quaternion.FromToRotation(Vector3.right, dir);
        }
        // 몬스터가 왼쪽에 있는 경우
        if (dir.x < 0)
        {
            weaponBone.rotation = Quaternion.FromToRotation(Vector3.left, dir);
        }
        */

        Transform[] bullets = new Transform[5];
        for(int bulletIdx = 0; bulletIdx < 5 ; bulletIdx++)
        {
            //Pool에서 bullet 생성
            bullets[bulletIdx] = gameManager.pool.BulletGet(prefabId).transform;
            
            // 총알의 시작지점
            bullets[bulletIdx].position = transform.position;

            // 총알이 바라보는 방향 설정
            bullets[bulletIdx].rotation = Quaternion.FromToRotation(Vector3.down, dirs[bulletIdx]);

            // 총알 초기화
            bullets[bulletIdx].GetComponent<Bullet>().Init(damage, count, knockbackPower, ShotGunlifeTime, dirs[bulletIdx]);
        }
    }
}
