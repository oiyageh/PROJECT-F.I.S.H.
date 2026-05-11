using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OrganUISlot : MonoBehaviour,
    IDropHandler,
    IPointerEnterHandler,
    IPointerExitHandler
{
    [Header("Correct Organ Name")]
    public string correctOrgan;

    private Image image;
    private Color normalColor;

    private OrganUIDrag currentOrgan;

    [Header("Colors")]
    public Color hoverColor = Color.yellow;
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;

    void Awake()
    {
        image = GetComponent<Image>();
        normalColor = image.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OrganUIDrag organ = eventData.pointerDrag?.GetComponent<OrganUIDrag>();
        if (organ == null) return;

        OrganUIData data = organ.GetComponent<OrganUIData>();

        if (data.organName == correctOrgan)
            image.color = correctColor;
        else
            image.color = wrongColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = normalColor;
    }

    public void OnDrop(PointerEventData eventData)
    {
        OrganUIDrag organ = eventData.pointerDrag?.GetComponent<OrganUIDrag>();
        if (organ == null) return;

        OrganUIData data = organ.GetComponent<OrganUIData>();

        if (data.organName == correctOrgan)
        {
            organ.SnapTo(transform);
            currentOrgan = organ;
            image.color = correctColor;
        }
        else
        {
            organ.ResetPosition();
            image.color = wrongColor;
        }
    }

    public bool IsCorrect()
    {
        if (currentOrgan == null) return false;

        return currentOrgan.GetComponent<OrganUIData>().organName == correctOrgan;
    }

    public bool IsFilled()
    {
        return currentOrgan != null;
    }

    public void ResetSlot()
    {
        image.color = normalColor;
    }
}