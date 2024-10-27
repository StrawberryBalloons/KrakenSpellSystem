using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MoveSpellNode : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform panelRectTransform;
    private Canvas canvas;
    private Vector2 offset;
    private Vector2 boundsMin = new Vector2(-700f, -700f);
    private Vector2 boundsMax = new Vector2(700f, 700f);
    private Vector2 originalPosition;



    public void OnBeginDrag(PointerEventData eventData)
    {
        panelRectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        // Debug.Log("Begin Drag");
        // offset = eventData.position - (Vector2)panelRectTransform.anchoredPosition;

        // Convert screen point to local point in the context of the canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPointerPosition);

        // Calculate the offset from the current anchored position to the pointer position
        offset = (Vector2)panelRectTransform.anchoredPosition - localPointerPosition;
        originalPosition = panelRectTransform.anchoredPosition;

        GetComponent<Image>().raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update the panel's anchoredPosition based on the mouse cursor, clamped within the specified bounds
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPointerPosition))
        {
            Vector2 rawPanelPos = localPointerPosition + offset;

            Vector2 clampedPosition = new Vector2(
                Mathf.Clamp(rawPanelPos.x, boundsMin.x, boundsMax.x),
                Mathf.Clamp(rawPanelPos.y, boundsMin.y, boundsMax.y)
            );

            panelRectTransform.anchoredPosition = clampedPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Additional logic to handle the end of dragging, if needed.
        GetComponent<Image>().raycastTarget = true;
    }

    public void ResetPosition()
    {
        panelRectTransform.anchoredPosition = originalPosition;
    }

}
