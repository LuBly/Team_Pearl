using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickMovement : MonoBehaviour
{
    public static JoystickMovement Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<JoystickMovement>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("JoystickMovement");
                    instance = instanceContainer.AddComponent<JoystickMovement>();
                }
            }
            return instance;
        }
    }

    private static JoystickMovement instance;

    public GameObject smallStick;
    public GameObject bGStick;
    
    public Vector3 joyVec;
    Vector3 stickFirstPosition;
    float stickRadius;
    RectTransform panelRect;

    private void Awake()
    {
        panelRect = GetComponent<RectTransform>();
    }
    void Start()
    {
        stickRadius = bGStick.gameObject.GetComponent<RectTransform>().sizeDelta.y / 2;
    }

    public void PointDown()
    {
        bGStick.SetActive(true);
        bGStick.transform.position = Input.mousePosition;
        smallStick.transform.position = Input.mousePosition;
        stickFirstPosition= Input.mousePosition;
    }
    public void Drag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector3 dragPosition = pointerEventData.position;

        // 패널의 크기를 가져옴
        Vector2 panelSize = panelRect.rect.size;

        // 패널의 경계를 화면 좌표로 변환
        Vector3 minPanelBounds = panelRect.TransformPoint(new Vector3(-panelSize.x / 2, -panelSize.y / 2, 0));
        Vector3 maxPanelBounds = panelRect.TransformPoint(new Vector3(panelSize.x / 2, panelSize.y / 2, 0));

        // 드래그된 위치가 패널 범위 내에 있는지 확인
        if (dragPosition.x >= minPanelBounds.x && dragPosition.x <= maxPanelBounds.x &&
            dragPosition.y >= minPanelBounds.y && dragPosition.y <= maxPanelBounds.y)
        {
            // 패널 범위 내에서 드래그 중이므로 조이스틱을 업데이트
            joyVec = (dragPosition - stickFirstPosition).normalized;
            float stickDistance = Vector3.Distance(dragPosition, stickFirstPosition);

            if (stickDistance < stickRadius)
            {
                smallStick.transform.position = stickFirstPosition + joyVec * stickDistance;
            }
            else
            {
                smallStick.transform.position = stickFirstPosition + joyVec * stickRadius;
            }
        }
        else
        {
            // 패널 범위를 벗어났으므로 Drop 함수 호출
            Drop();
        }
    }

    public void Drop()
    {
        joyVec = Vector3.zero;
        bGStick.SetActive(false);
    }
    
    
}
