using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public Item item;
    public int quantity;

    public InventoryItem(Item newItem, int newQuantity)
    {
        item = newItem;
        quantity = newQuantity;
    }
}

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public Transform rightHand;
    public Transform leftHand;

    public List<InventoryItem> items = new List<InventoryItem>();

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    public int space = 20;

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("There is more than one instance of inventory detected");
            return;
        }
        instance = this;
    }

    public bool Add(Item item, int amount = 1)
    {
        if (!item.isDefault)
        {
            // Check if item is already in inventory and can be stacked
            foreach (InventoryItem slot in items)
            {
                if (slot.item == item)
                {
                    slot.quantity += amount;
                    onItemChangedCallback?.Invoke();
                    item.itemOwner = transform;
                    return true;
                }
            }

            // Check if there is space for a new item
            if (items.Count >= space)
            {
                Debug.Log("Not enough inventory space");
                return false;
            }

            // Add new item to inventory
            items.Add(new InventoryItem(item, amount));
            onItemChangedCallback?.Invoke();
        }
        item.itemOwner = transform;
        return true;
    }

    public void Remove(Item item, int amount = 1)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item == item)
            {
                items[i].quantity -= amount;

                if (items[i].quantity <= 0)
                {
                    items.RemoveAt(i);
                }

                onItemChangedCallback?.Invoke();
                item.itemOwner = null;
                return;
            }
        }
    }

    public int GetItemCount(Item item)
    {
        foreach (InventoryItem slot in items)
        {
            if (slot.item == item)
            {
                return slot.quantity;
            }
        }
        return 0;
    }
}
