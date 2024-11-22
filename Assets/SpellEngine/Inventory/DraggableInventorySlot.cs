using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class DraggableInventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform slotRectTransform;
    private Vector2 initialPanelPosition;
    private Transform originalParent;
    private Canvas canvas;
    private int originalSiblingIndex;
    public GridLayoutGroup gridLayoutGroup;
    public InventorySlot invenSlot;
    private Vector2 dragOffset;
    private GraphicRaycaster graphicRaycaster;
    public Image image;
    private Transform higherParent;

    // Start is called before the first frame update
    void Start()
    {
        gridLayoutGroup = GetComponentInParent<GridLayoutGroup>();
        canvas = GetComponentInParent<Canvas>();
        slotRectTransform = GetComponent<RectTransform>();
        graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        initialPanelPosition = slotRectTransform.anchoredPosition;
        image.raycastTarget = false;

        // Save the original parent and sibling index
        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();

        // Move the item two levels up in the hierarchy and set as the last child
        higherParent = originalParent.parent?.parent?.parent?.parent;
        if (higherParent != null)
        {
            transform.SetParent(higherParent);
            transform.SetAsLastSibling();
        }

        // Calculate the offset between the mouse pointer and the item's position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform, eventData.position, eventData.pressEventCamera, out var localMousePosition);

        dragOffset = (Vector2)slotRectTransform.localPosition - localMousePosition;



        // Disable GridLayoutGroup on original parent
        if (gridLayoutGroup != null)
        {
            gridLayoutGroup.enabled = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update position considering the drag offset
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform, eventData.position, eventData.pressEventCamera, out var localMousePosition);

        slotRectTransform.localPosition = localMousePosition + dragOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        slotRectTransform.anchoredPosition = initialPanelPosition;

        // Return the item to its original parent and sibling index
        transform.SetParent(originalParent);
        transform.SetSiblingIndex(originalSiblingIndex);

        if (gridLayoutGroup != null)
        {
            gridLayoutGroup.enabled = true;
        }

        // Perform raycast to check if dropped on an inventory slot
        InventorySlot hoveredSlot = GetInventorySlotUnderPointer(eventData);
        if (hoveredSlot != null)
        {
            Debug.Log($"Dropped on inventory slot: {hoveredSlot.name}");
            hoveredSlot.AddItem(invenSlot.item);
            invenSlot.ClearSlot();
            // Add your logic here for handling dropping on a slot
        }
        else
        {
            Debug.Log("Not dropped on any inventory slot.");
        }
        image.raycastTarget = true;
    }

    private InventorySlot GetInventorySlotUnderPointer(PointerEventData eventData)
    {
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        graphicRaycaster.Raycast(eventData, raycastResults);

        if (raycastResults.Count > 0)
        {
            Debug.Log("Raycast hits:");
            foreach (var result in raycastResults)
            {
                Debug.Log($"Hit: {result.gameObject.name} (Layer: {result.gameObject.layer})");

                // Check if the hit object is named "ItemButton"
                if (result.gameObject.name == "ItemButton")
                {
                    // Get the parent of the ItemButton
                    Transform parent = result.gameObject.transform.parent;

                    // Check if the parent has an InventorySlot component
                    InventorySlot slot = parent.GetComponent<InventorySlot>();
                    if (slot != null)
                    {
                        // Compare it to the current InventorySlot
                        if (slot == invenSlot)
                        {
                            Debug.Log("The parent of ItemButton matches the current inventory slot.");
                        }
                        else
                        {
                            Debug.Log("The parent of ItemButton does NOT match the current inventory slot.");
                            return slot; // Return the matching InventorySlot
                        }

                    }
                }
            }
        }

        return null; // Return null if no match found
    }



}
