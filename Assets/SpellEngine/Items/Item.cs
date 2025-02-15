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
    public GameObject spritePrefab;
    private bool spriteEquip = false;
    private GameObject spriteInstance = null;

    public virtual void Use()
    {
        if (spriteEquip = true)
        {
            Debug.Log("Attacking with: " + name);
        }
        else
        {
            Debug.Log("Using: " + name);
        }
    }

    public void RemoveFromInventory()
    {
        itemOwner.GetComponent<Inventory>().Remove(this);
    }

    public void ShowInHand()
    {
        Inventory inven = this.itemOwner.GetComponent<Inventory>();
        if (spritePrefab == null)
        {
            //Debug.LogError("Sprite prefab is not assigned.");
            Debug.Log("Sprite prefab is not assigned.");
            return;
        }
        if (inven == null)
        {
            Debug.LogError("Inventory reference (inven) is null.");
            return;
        }
        if (inven.rightHand == null)
        {
            Debug.LogError("Right hand transform is not assigned in Inventory.");
            return;
        }

        // Instantiate the weapon without affecting its original rotation or scale
        spriteInstance = Instantiate(spritePrefab, inven.rightHand.position, inven.rightHand.rotation, inven.rightHand);

        // Adjust rotation: Set X-axis to 90 degrees while keeping the rightHand's rotation
        Quaternion newRotation = inven.rightHand.rotation * Quaternion.Euler(90f, 0f, 0f);
        spriteInstance.transform.rotation = newRotation;

        // Scale the weapon to 1/100th of its original size
        spriteInstance.transform.localScale = spritePrefab.transform.localScale * 0.01f;

        //Set Icon of SpriteInstance to be the same as the Icon above
        spriteInstance.GetComponent<SpriteRenderer>().sprite = icon;
    }

    public void HideHeldItem()
    {
        //Delete Sprite game object
        Destroy(spriteInstance);
        spriteEquip = false;
    }

}
