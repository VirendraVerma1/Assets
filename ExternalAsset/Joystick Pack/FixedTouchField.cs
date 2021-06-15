using UnityEngine;
using UnityEngine.EventSystems;

public class FixedTouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]
    public Vector2 TouchDist;
    [HideInInspector]
    public Vector2 PointerOld;
    [HideInInspector]
    protected int PointerId;
    [HideInInspector]
    public bool Pressed;

    public bool isThisFieldOn=false;

    // void Awake()
    // {
    //     isThisFieldOn=true;
    // }

    // Use this for initialization
    void OnEnable()
    {
        isThisFieldOn=true;
    }

    void OnDisable()
    {
        isThisFieldOn=false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isThisFieldOn)
        {
            if (Pressed)
            {
                if (PointerId >= 0 && PointerId < Input.touches.Length)
                {
                    TouchDist = Input.touches[PointerId].position - PointerOld;
                    PointerOld = Input.touches[PointerId].position;
                }
                else
                {
                    TouchDist = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - PointerOld;
                    PointerOld = Input.mousePosition;
                }
            }
            else
            {
                TouchDist = new Vector2();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(isThisFieldOn)
        {
            Pressed = true;
            PointerId = eventData.pointerId;
            PointerOld = eventData.position;
        }
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        if(isThisFieldOn)
        {
            Pressed = false;
        }
    }
    
}