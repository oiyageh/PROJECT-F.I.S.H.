using UnityEngine;
using UnityEngine.EventSystems;

public class OrganUIDrag : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private Vector2 startPos;
    private Transform startParent;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = rectTransform.anchoredPosition;
        startParent = transform.parent;

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.7f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }

    public void SnapTo(Transform slot)
    {
        transform.SetParent(slot);
        rectTransform.anchoredPosition = Vector2.zero;
    }

    public void ResetPosition()
    {
        transform.SetParent(startParent);
        rectTransform.anchoredPosition = startPos;
    }
}