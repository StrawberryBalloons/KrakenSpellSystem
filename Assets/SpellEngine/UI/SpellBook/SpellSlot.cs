using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellSlot : MonoBehaviour, IDropHandler
{
    public Image storedImage;
    public Button removeButton;
    public ICast castAction;
    public GameObject placeHolder;
    private bool doOnDrop = true;

    void Awake()
    {
        if (castAction == null)
        {
            castAction = placeHolder.GetComponent<ICast>();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && doOnDrop)
        {
            // Get the DraggableItem component attached to the dragged object
            DraggableItem draggableItem = eventData.pointerDrag.GetComponent<DraggableItem>();

            if (draggableItem != null)
            {
                // Store the Image component of the dragged object in this slot
                SetSprite(draggableItem.image);
                SetSpell(draggableItem.castAction);
            }
        }
    }

    public void SetSpell(ICast newSpell)
    {
        castAction = newSpell;
        // Implement logic to update UI or display the spell icon, etc.
    }

    public void RemoveSprite()
    {
        // Clear the InventorySlot image
        storedImage.sprite = null;
        doOnDrop = true;
        removeButton.interactable = true;
    }

    public void SetSprite(Image image)
    {
        // Set the InventorySlot image
        storedImage.sprite = image.sprite;

        // Make it uninteractable
        doOnDrop = false;

        // Disable the removeButton
        removeButton.interactable = false;
    }
}

/*
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellSlot : MonoBehaviour, IDropHandler
{
    public GameObject spellNodeSlotPrefab; // Reference to the SpellNodeSlot prefab

    public void OnDrop(PointerEventData eventData)
    {
        // Instantiate SpellNodeSlot prefab at drop position
        GameObject spellNodeSlotInstance = Instantiate(spellNodeSlotPrefab, eventData.position, Quaternion.identity);
        // Set parent to the drop parent (e.g., spell list panel)
        spellNodeSlotInstance.transform.SetParent(transform.parent);
    }
}




*/