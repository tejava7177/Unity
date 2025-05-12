using UnityEngine;
using UnityEngine.EventSystems;

public class TouchCtrl : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform handle;           // 조이스틱 핸들
    public RectTransform background;       // 조이스틱 배경
    public float dragRadius = 50f;         // 최대 이동 반경
    public TankController player;          // 제어 대상

    private Vector2 inputVector = Vector2.zero;

    void Start()
    {
        if (background == null)
            background = GetComponent<RectTransform>();

        if (handle != null)
            handle.anchoredPosition = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out pos);

        Vector2 clampedPos = Vector2.ClampMagnitude(pos, dragRadius);
        handle.anchoredPosition = clampedPos;

        inputVector = clampedPos / dragRadius;

        if (player != null)
            player.SetInput(inputVector);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;

        if (player != null)
            player.SetInput(Vector2.zero);
    }
}