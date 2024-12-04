using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
public class InventorySlot : MonoBehaviour
{
    public Item item;
    public Button remove;
    public Image icon;
    private RectTransform slotRectTransform;
    private Vector2 offset;
    [HideInInspector]
    public Transform parentAfterDrag;

    public void AddItem(Item newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
        remove.interactable = true;
    }

    public void ClearSlot()
    {

        item = null;
        icon.sprite = null;
        icon.enabled = false;
        remove.interactable = false;
        icon.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }

    public void OnRemoveButton()
    {
        Inventory.instance.Remove(item);
    }

    public void UseItem()
    {
        if (item != null)
        {
            item.Use();
        }
    }

}
