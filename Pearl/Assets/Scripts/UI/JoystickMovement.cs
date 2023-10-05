using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickMovement : MonoBehaviour
{ 
    public GameObject smallStick;
    public GameObject bGStick;
    
    public Vector3 joyVec;
    public float stickDistance;
    public bool isDrag;
    Vector3 stickFirstPosition;
    float stickRadius;

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
        isDrag = true;
        //조이스틱이 꺼져있는 상태라면 on
        if (bGStick.activeSelf == false) 
        {
            bGStick.SetActive(true);
            smallStick.transform.position = bGStick.transform.position;
            stickFirstPosition = smallStick.transform.position;
        } 

        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector3 dragPosition = pointerEventData.position;

        joyVec = (dragPosition - stickFirstPosition).normalized;
        stickDistance = Vector3.Distance(dragPosition, stickFirstPosition);

        if (stickDistance < stickRadius)
        {
            smallStick.transform.position = stickFirstPosition + joyVec * stickDistance;
        }
        else
        {
            stickDistance = stickRadius;
            smallStick.transform.position = stickFirstPosition + joyVec * stickRadius;
        }
    }
    
    public void OnEndDrag()
    {
        isDrag = false;
        joyVec = Vector3.zero;
        bGStick.SetActive(false);
    }
}
