using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Item item;
    public Button remove;
    public Image icon;
    private RectTransform slotRectTransform;
    private Vector2 offset;
    [HideInInspector]
    public Transform parentAfterDrag;
    public TMP_Text itemCount;


    public void AddItem(Item newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
        remove.interactable = true;
        UpdateCount(Inventory.instance.GetItemCount(item));
        //Add a count for how many of this item is in the inventory
    }
    private void UpdateCount(int count)
    {
        if (count < 2)
        {
            itemCount.text = "";
        }
        else
        {
            itemCount.text = count.ToString();
        }
    }

    public void ClearSlot()
    {
        UpdateCount(0);
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        remove.interactable = false;
        icon.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }

    public void OnRemoveButton()
    {
        Inventory.instance.Remove(item);
        UpdateCount(Inventory.instance.GetItemCount(item));
    }

    public void UseItem()
    {
        if (item != null)
        {
            item.Use();
        }
    }

}
