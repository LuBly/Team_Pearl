using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    /*
     * 무기 관리를 위한 Script
     */
    public int id;               // 무기 종류(AR,SR,SG)
    public int prefabId;         // 총알 종류(SR1,SR2~~)
    public float knockbackPower; // 총알 넉백 파워
    public float damage;         // 총알 데미지
    public float gunRapid;       // 공격 속도(몇 초에 한번 발사하는가)
    public int count;            // 관통력(체크용)
    float timer = 0f;
    Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }
    private void Update()
    {
        switch (id)
        {
            case 0:
                timer += Time.deltaTime;
                if (timer > gunRapid)
                {
                    Fire();
                    timer = 0f;
                }
                break;
            case 1:
                break;
            case 2:
                break;
        }
    }
    public void Init()
    {
        switch (id)
        {
            case 0://AR
                break;
            case 1://SR
                break;
            case 2://SG
                break;
        }
    }
    //총알을 BulletManager에서 가지고 오면 된다.
    void Fire()
    {   
        //대상이 없다면 발사 X
        if (!player.scanner.nearestTarget)
        {
            return;
        }
        //대상이 있다면 발사
        Vector3 targetPos = player.scanner.nearestTarget.position;
        // 속도(크기를 포함한 방향벡터) = 목표 위치 - 내 위치
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;//정규화
        Transform bullet = GameManager.instance.pool.BulletGet(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.down, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, knockbackPower, dir);
    }
}
