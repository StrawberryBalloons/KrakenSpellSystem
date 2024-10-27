using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZoomablePanel : MonoBehaviour, IScrollHandler
{
    [SerializeField]
    private float zoomSpeed = 0.1f; // Adjust this value to control the zoom speed
    [SerializeField]
    private float minZoom = 0.5f; // Set the minimum zoom level
    [SerializeField]
    private float maxZoom = 2.0f; // Set the maximum zoom level

    private RectTransform panelRectTransform;

    void Start()
    {
        panelRectTransform = GetComponent<RectTransform>();
    }

    public void OnScroll(PointerEventData eventData)
    {
        float zoomDelta = eventData.scrollDelta.y * zoomSpeed;

        // Calculate the new scale based on the zoom delta
        float newScale = Mathf.Clamp(panelRectTransform.localScale.x + zoomDelta, minZoom, maxZoom);

        // Apply the new scale to the panel
        panelRectTransform.localScale = new Vector3(newScale, newScale, 1.0f);
    }
}