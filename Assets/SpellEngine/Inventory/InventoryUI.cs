using UnityEngine;
using System.Linq;


public class InventoryUI : MonoBehaviour
{
    Inventory inventory;
    public GameObject inventoryUI;
    public Transform itemsParent;
    public Transform toolParent;

    public KeyCode interactKey = KeyCode.I;
    InventorySlot[] slots;


    int currentSlotIndex = 0; // To track the currently selected slot index
    public InventorySlot currentSlot; // Public variable to hold the current slot

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;
        // Get components from both parents and combine them into a single array
        slots = itemsParent.GetComponentsInChildren<InventorySlot>()
            .Concat(toolParent.GetComponentsInChildren<InventorySlot>())
            .ToArray();

        if (toolParent.childCount > 0)
        {
            currentSlot = toolParent.GetChild(currentSlotIndex).GetComponent<InventorySlot>();

            // Get the child named "selected" and make it active
            activateSlot();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            if (inventoryUI.activeSelf)
            {
                RevealCursor();
            }
            else
            {
                HideCursor();
            }
        }

        // Handle scrolling to switch slots
        if (Input.mouseScrollDelta.y > 0) // Scroll up
        {
            MoveToNextSlot();
        }
        else if (Input.mouseScrollDelta.y < 0) // Scroll down
        {
            MoveToPreviousSlot();
        }

        // Handle left and right mouse clicks
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            currentSlot?.UseItem();
        }
        else if (Input.GetMouseButtonDown(1)) // Right click
        {
            currentSlot?.UseItem();
        }
    }

    void MoveToNextSlot()
    {
        if (toolParent.childCount == 0) return;

        deactivateSlot();

        currentSlotIndex = (currentSlotIndex + 1) % toolParent.childCount;
        UpdateCurrentSlot();
    }

    void MoveToPreviousSlot()
    {
        if (toolParent.childCount == 0) return;

        deactivateSlot();

        currentSlotIndex = (currentSlotIndex - 1 + toolParent.childCount) % toolParent.childCount;
        UpdateCurrentSlot();
    }

    void deactivateSlot()
    {
        // Get the child named "selected" and make it active
        Transform selectedChild = currentSlot.transform.Find("Selected");
        if (selectedChild != null)
        {
            selectedChild.gameObject.SetActive(false);
        }
    }
    void activateSlot()
    {
        // Get the child named "selected" and make it active
        Transform selectedChild = currentSlot.transform.Find("Selected");
        if (selectedChild != null)
        {
            selectedChild.gameObject.SetActive(true);
        }
    }

    void UpdateCurrentSlot()
    {
        // Sheathe the weapon if the current slot item is a weapon
        if (currentSlot.item is Weapons currentWeapon)
        {
            Debug.Log("The item in the current slot is a weapon. Running Sheathe().");
            currentWeapon.Sheathe();
        }

        // Update the current slot
        currentSlot = toolParent.GetChild(currentSlotIndex).GetComponent<InventorySlot>();
        Debug.Log($"Current slot updated to: {currentSlot.name}");

        // Wield the weapon if the new current slot item is a weapon
        if (currentSlot.item is Weapons newWeapon)
        {
            Debug.Log("The item in the current slot is a weapon. Running Wield().");
            newWeapon.Wield();
        }

        activateSlot();
    }



    void UpdateUI()
    {
        Debug.Log("UPDATING UI");
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].ClearSlot();
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
        }
    }

    void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the game window
        Cursor.visible = false; // Hide the cursor
    }
    void RevealCursor()
    {
        Cursor.lockState = CursorLockMode.None; // Release the cursor from the game window
        Cursor.visible = true; // Show the cursor
    }
}
