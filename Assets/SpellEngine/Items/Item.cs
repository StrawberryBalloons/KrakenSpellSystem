using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public bool isDefault = false;
    public Transform itemOwner = null;

    public virtual void Use()
    {
        Debug.Log("Using: " + name);
    }

    public void RemoveFromInventory()
    {
        itemOwner.GetComponent<Inventory>().Remove(this);
    }

}
