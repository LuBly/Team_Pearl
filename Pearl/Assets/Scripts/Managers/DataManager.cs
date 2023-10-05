using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public CharacterBase characterData;
    public int id; // 총기 id
    public int atk; // 공격력
    public int gunAtk; // 총기 공격력
    public int gunProficiency; // 총기 숙련도
    public float gunRapid; // 총기 공격속도 (공격속도 수치가 공격 간격입니다. ex 0.5 = 0.5초에 한번 공격, 1 = 1초에 한번 공격)
    public int health; // 체력
    public int critical; // 크리티컬 확률
    public int criticalDmg; // 크리티컬 데미지
    public int moveSpeed; // 이동속도

    public void Load()
    {
        id = characterData.id;
        atk = characterData.atk;
        gunAtk = characterData.gunAtk;
        gunProficiency = characterData.gunProficiency;
        gunRapid = characterData.gunRapid;
        health = characterData.health;
        critical = characterData.critical;
        criticalDmg = characterData.criticalDmg;
        moveSpeed = characterData.moveSpeed;
    }
}
