using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    static public CameraController instance;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private BoxCollider2D edge;
    
    // box Collider 영역의 최소 최대 xyz값을 지님
    private Vector3 minBound;
    private Vector3 maxBound;

    // 카메라의 반너비, 반높이 값을 지닐 변수
    private float halfWidth;
    private float halfHeight;

    // 카메라의 반높이 값을 구할 속성 이용
    private Camera theCamera;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance= this;
        }
    }
    void Start()
    {
        theCamera = GetComponent<Camera>();
        minBound = edge.bounds.min; 
        maxBound = edge.bounds.max;

        halfHeight = theCamera.orthographicSize;           // 카메라의 반높이
        halfWidth = halfHeight*Screen.width/Screen.height; // 카메라의 반너비
    }

    void LateUpdate()
    {
        transform.position = new Vector3(player.position.x,player.position.y+0.5f,-10f);
        float clampedX = Mathf.Clamp(transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
        float clampedY = Mathf.Clamp(transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);

        transform.position = new Vector3(clampedX,clampedY, transform.position.z);
    }
}
