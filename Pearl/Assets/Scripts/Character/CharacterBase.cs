using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
해당 문서는 캐릭터 기본 스테이터스를 저장하기 위한 문서입니다.
*/


public class CharacterBase : MonoBehaviour
{
    public int id; // 총기 id
    public int atk; // 공격력
    public int gunAtk; // 총기 공격력
    public int gunProficiency; // 총기 숙련도
    public float gunRapid; // 총기 공격속도 (공격속도 수치가 공격 간격입니다. ex 0.5 = 0.5초에 한번 공격, 1 = 1초에 한번 공격)
    public int health; // 체력
    public int critical; // 크리티컬 확률
    public int criticalDmg; // 크리티컬 데미지
    public int moveSpeed; // 이동속도

    void Start()
    {
        GameObject.Find("MainDisplay").GetComponent<SaveManager>().Load();
        GameObject.Find("EquipPanel").GetComponent<EquipManager>().EquipClick();
    }
}
