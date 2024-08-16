using UnityEngine;
using UnityEngine.EventSystems;

public class MapController : MonoBehaviour, IDragHandler, IScrollHandler
{
    public float zoomSpeed = 0.5f;
    public float minScale = 8.0f;
    public float maxScale = 30.0f;

    private RectTransform rectTransform;
    private bool isDragging = false;
    private PointerEventData dragEventData;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = new Vector3(minScale, minScale);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnScroll(PointerEventData eventData)
    {
        float scrollDelta = eventData.scrollDelta.y;
        Vector3 scale = rectTransform.localScale;
        float newScale = scale.x + scrollDelta * zoomSpeed;
        newScale = Mathf.Clamp(newScale, minScale, maxScale);
        rectTransform.localScale = new Vector3(newScale, newScale);
    }
}