using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
배경 이미지 스크롤링 용 스크립트(Background 오브젝트에 삽입)
*/

public class BackGroundScroll : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent ScrollDone; // 몬스터 및 배경 스크롤 완료 이벤트
    public float scrollSpeed; // 배경 스크롤 속도
    public Image background;
    public EnemyManager eManager;

    float moveSpeed; // 몬스터 이동 속도 
    //(배경 이동속도와 동일하게 이동하도록 설정. 배경 offset 1이 변하면 배경이 한번 완전히 이동한것. 배경 스크롤 속도 0.01 = 배경 크기의 1/100 속도로 이동이므로 몬스터 이동속도를 배경 사이즈의 1/100으로 설정하면 동일해짐)

    public GameObject enemy;
    public GameObject criterion; // 몬스터 정지 위치 기준
    float enemy_x;
    Vector3 defaultPos = new Vector3();

    private Vector2 offset = Vector2.zero; // 배경 오프셋 기본 값

    void Start()
    {
        background = GetComponent<Image>();
        moveSpeed = background.GetComponent<RectTransform>().rect.width * scrollSpeed;
        defaultPos = enemy.transform.position;
        OnScroll();
    }

    public void OnScroll()
    {
        enemy_x = defaultPos.x;
        enemy.transform.position = new Vector3(enemy_x, enemy.transform.position.y, enemy.transform.position.z);
        eManager.EnemySet();
        StartCoroutine(Scrolling());
    }

    IEnumerator Scrolling() // 스크롤 함수
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(0.02f);
            offset.x += scrollSpeed;
            background.material.mainTextureOffset = offset; // 배경 오프셋 변경
            enemy_x -= moveSpeed; // 몬스터 x 좌표 값
            enemy.transform.position = new Vector3(enemy_x, enemy.transform.position.y, enemy.transform.position.z); // 몬스터 좌표 변경

            if(enemy.transform.position.x < criterion.transform.position.x) break;
        }
        ScrollDone.Invoke();
        yield break;
    }
}
