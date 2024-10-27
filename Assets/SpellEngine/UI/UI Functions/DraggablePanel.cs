using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggablePanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform panelRectTransform;
    private Vector2 offset;
    private Vector2 boundsMin = new Vector2(-700f, -700f);
    private Vector2 boundsMax = new Vector2(700f, 700f);
    private Vector2 originalPosition;

    void Start()
    {
        panelRectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        offset = eventData.position - (Vector2)panelRectTransform.anchoredPosition;
        originalPosition = panelRectTransform.anchoredPosition;
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update the panel's anchoredPosition based on the mouse cursor, clamped within the specified bounds
        Vector2 rawPanelPos = eventData.position - offset;

        Vector2 clampedPosition = new Vector2(
            Mathf.Clamp(rawPanelPos.x, boundsMin.x, boundsMax.x),
            Mathf.Clamp(rawPanelPos.y, boundsMin.y, boundsMax.y)
        );

        panelRectTransform.anchoredPosition = clampedPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Additional logic to handle the end of dragging, if needed.
    }

    public void CentrePanel()
    {
        panelRectTransform.anchoredPosition = originalPosition;
    }
}
