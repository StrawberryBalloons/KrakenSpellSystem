using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public Transform rightHand;
    public Transform leftHand;

    public List<Item> items = new List<Item>();

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

    public bool Add(Item item)
    {
        if (!item.isDefault)
        {
            if (items.Count >= space)
            {
                Debug.Log("Not enough Inven space");
                return false;
            }
            items.Add(item);

            if (onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();
            }
        }
        item.itemOwner = transform;
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
        item.itemOwner = null;
    }


}
