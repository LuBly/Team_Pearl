using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    
    public int id; // 총기 id
    public int atk; // 공격력
    public int gunAtk; // 총기 공격력
    public int gunProficiency; // 총기 숙련도
    public float gunRapid; // 총기 공격속도 (공격속도 수치가 공격 간격입니다. ex 0.5 = 0.5초에 한번 공격, 1 = 1초에 한번 공격)
    public float health; // 체력
    public int critical; // 크리티컬 확률
    public int criticalDmg; // 크리티컬 데미지
    public int moveSpeed; // 이동속도
    public string stageInfo;
    CharacterBase characterData;
    public int weaponIdx;
    public void Load()
    {
        characterData = GameObject.Find("MainChar").GetComponent<CharacterBase>();
        id = characterData.id;
        atk = characterData.atk;
        gunAtk = characterData.gunAtk;
        gunProficiency = characterData.gunProficiency;
        gunRapid = characterData.gunRapid;
        health = characterData.health;
        critical = characterData.critical;
        criticalDmg = characterData.criticalDmg;
        moveSpeed = characterData.moveSpeed;

        weaponIdx = id / 100 - 1;
        //stageInfo 입력 업데이트 필요

    }

    public void Init()
    {
        // 부모 오브젝트가 있는 경우 
        if (transform.parent != null && transform.root != null)
        {
            Destroy(transform.root.gameObject);
        }
        // 본인 스스로가 최상위라면
        else
        {
            Destroy(gameObject);
        }
    }
}
