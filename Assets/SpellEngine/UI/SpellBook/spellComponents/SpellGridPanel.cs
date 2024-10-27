using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpellGridPanel : MonoBehaviour, IDropHandler
{
    public GameObject actionsList;
    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("OnDrop");

        // Check if the dragged object has a DraggableItem component
        DraggableItem draggableItem = eventData.pointerDrag.GetComponent<DraggableItem>();
        if (draggableItem != null)
        {
            // Instantiate the spellNodeSlotPrefab
            GameObject spellNodeSlotInstance = Instantiate(draggableItem.spellNodeSlotPrefab);

            // Convert the screen position to a local position within the canvas
            Vector3 worldPoint;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                transform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out worldPoint
            );
            // Debug.Log("Pointer drop location: " + eventData.position);
            // Debug.Log("World drop location: " + worldPoint);
            // Debug.Log("Local drop location: " + transform.GetComponent<RectTransform>().InverseTransformPoint(worldPoint));


            // Set the local position of the instantiated object
            spellNodeSlotInstance.transform.localPosition = transform.GetComponent<RectTransform>().InverseTransformPoint(worldPoint);

            // Parent the instantiated object to the current transform
            spellNodeSlotInstance.transform.SetParent(transform, false);

            // Get the SpellNodeSlot component and initialize it
            SpellNodeSlot spellNodeSlot = spellNodeSlotInstance.GetComponent<SpellNodeSlot>();
            List<object> parameters = draggableItem.parameterPrefab.GetComponent<SpellParameters>().parameters;
            if (spellNodeSlot != null)
            {
                spellNodeSlot.InitializeNode(draggableItem.castAction, spellNodeSlotInstance.transform.position, parameters);
            }

            // Assign the sprite from the draggable item to the Image component of the new instance
            Image imageComponent = spellNodeSlotInstance.GetComponent<Image>();
            if (imageComponent != null)
            {
                imageComponent.sprite = draggableItem.image.sprite;
            }
        }
    }


    //     //QUESTIONS:
    //     // Don't I just need the reference to the spell list?
    //     /* Requirements:
    //     Node
    //     Position
    //     ICast script reference
    //     */



    //     //for each item in spellListManager

    //     //Find the Nth items ICast counterpart in ActionsList

    //     //Instantiate the Nth-Counterpart spellNodeSlotPrefab as child of transform with position in spellListManager
    //     //store it in a list

    //     // // Get the SpellNodeSlot component and initialize it
    //     // // take the node and original position to assign to the spellNodeSlot
    //     // SpellNodeSlot spellNodeSlot = spellNodeSlotInstance.GetComponent<SpellNodeSlot>();
    //     // if (spellNodeSlot != null)
    //     // {

    //     // }

    //     // // Assign the sprite from the Nth-Counterpart to the Image component
    //     // Image imageComponent = spellNodeSlotInstance.GetComponent<Image>();
    //     // if (imageComponent != null)
    //     // {
    //     //     imageComponent.sprite = draggableItem.image.sprite;
    //     // }

    //     //After everything is instantiated
    //     //loop over the stored list
    //     //For each node.next of Nth, use SpellNodeSlot.LoadNextNode on the gameobject with the same position as the next node


    public void Instantiate(SpellListManager referenceManager)
    {
        List<GameObject> spellNodeSlots = new List<GameObject>();
        Debug.Log("referenceManager count" + referenceManager.nodelist.Count);

        // Loop through the nodelist
        for (int i = 0; i < referenceManager.nodelist.Count; i++)
        {

            SpellNode currentNode = referenceManager.nodelist[i];
            Debug.Log("node name" + currentNode.spell);

            // Find the corresponding action GameObject from actionsList
            GameObject matchingAction = FindMatchingAction(currentNode.spell);

            if (matchingAction != null)
            {
                // Instantiate the SpellNodeSlot prefab at the node's position
                GameObject spellNodeSlotInstance = Instantiate(matchingAction.GetComponent<DraggableItem>().spellNodeSlotPrefab, currentNode.position, Quaternion.identity, this.transform);

                // Set the local rotation of the RectTransform
                RectTransform rectTransform = spellNodeSlotInstance.GetComponent<RectTransform>();
                rectTransform.localRotation = Quaternion.Euler(0, 0, 0); // Set rotation to 45 degrees on the Z axis (modify as needed) 
                rectTransform.localPosition = currentNode.position;
                Debug.Log("Set rectTransform.localPosition to: " + currentNode.position);

                // Store the instantiated slot for later use
                spellNodeSlots.Add(spellNodeSlotInstance);

                // Initialize the SpellNodeSlot with the SpellNode and DraggableItem action data
                SpellNodeSlot spellNodeSlot = spellNodeSlotInstance.GetComponent<SpellNodeSlot>();
                if (spellNodeSlot != null)
                {
                    // Initialize with current node's data
                    spellNodeSlot.node = currentNode;
                    spellNodeSlot.originalPosition = currentNode.position;

                    // Optionally set the sprite using the matching DraggableItem
                    DraggableItem draggableItem = matchingAction.GetComponent<DraggableItem>();
                    if (draggableItem != null)
                    {
                        Image imageComponent = spellNodeSlotInstance.GetComponent<Image>();
                        if (imageComponent != null)
                        {
                            // Assign the sprite from DraggableItem
                            imageComponent.sprite = draggableItem.image.sprite;
                        }
                    }
                }
            }
            else
            {
                Debug.Log("No Matching Action found");
            }
        }


        // After instantiating all SpellNodeSlots, link the nodes based on 'next' references
        for (int i = 0; i < referenceManager.nodelist.Count; i++)
        {
            SpellNode currentNode = referenceManager.nodelist[i];

            // If this node has any 'next' nodes, load them
            foreach (SpellNode nextNode in currentNode.next)
            {
                // Find the SpellNodeSlot corresponding to the next node
                GameObject nextNodeSlot = spellNodeSlots.Find(slot => slot.GetComponent<SpellNodeSlot>().node == nextNode);
                if (nextNodeSlot)
                {
                    Debug.Log("Found next node: " + nextNodeSlot);
                    // Link the next node to the current node's slot
                    spellNodeSlots[i].GetComponent<SpellNodeSlot>().LoadNextNode(nextNodeSlot.GetComponent<SpellNodeSlot>());
                }
            }
        }
    }


    public GameObject FindMatchingAction(ICast spellAction)
    {

        // Loop through each child in actionsList
        foreach (Transform child in actionsList.transform)
        {
            GameObject actionObject = child.gameObject;
            // Try to get the DraggableItem component
            DraggableItem draggableItem = actionObject.GetComponent<DraggableItem>();

            // Check if DraggableItem has a castAction and if it matches the spellAction
            if (draggableItem != null && draggableItem.castAction == spellAction)
            {
                // Return the GameObject with the matching ICast action
                return actionObject;
            }
        }

        // If no match is found, return null or handle accordingly
        return null;
    }

}
