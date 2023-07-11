using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
해당 문서는 캐릭터 기본 스테이터스를 저장하기 위한 문서입니다.
*/


public class CharacterBase : MonoBehaviour
{
    public int Lv; // 레벨
    public int ATK; // 공격력
    public int Gun_ATK; // 총기 공격력
    public int Gun_proficiency; // 총기 숙련도
    public float Gun_Rapid; // 총기 공격속도 (공격속도 수치가 공격 간격입니다. ex 0.5 = 0.5초에 한번 공격, 1 = 1초에 한번 공격)
    public int Health; // 체력
    public int Critical; // 크리티컬 확률
    public int Critical_Dmg; // 크리티컬 데미지
}
