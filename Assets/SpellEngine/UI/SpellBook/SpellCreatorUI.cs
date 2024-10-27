using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

public class SpellCreatorUI : MonoBehaviour
{
    public SpellListHolder spellListHolder;
    public TMP_Dropdown spellListDropdown;
    public Button addButton;
    public Button removeButton;
    public Button clearButton;
    public TMP_InputField inputField; // TMP Input Field to display the spell name
    public TMP_InputField keyField;

    public List<SpellNodeSlot> spellNodeSlots; // List of SpellNodeSlot items
    private SpellListManager selectedSpellList;
    public GameObject gridParent;
    public GameObject spellNodeSlotPrefab; // Reference to the SpellNodeSlot prefab
    private bool inputSelected = false;
    private bool waitingForKey = false;

    void Start()
    {

        keyField.onSelect.AddListener(OnInputFieldSelect);

        PopulateDropdown();
        AddListeners();
        //InitializeKeyDropdown();
    }

    void Update()
    {
        //Gets keycode from input when triggered
        if (waitingForKey)
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    // Set the input field's text to the name of the key pressed
                    keyField.text = keyCode.ToString();
                    waitingForKey = false;
                    return;
                }
            }
        }
        if (inputSelected)
        {
            //reset colour if needed
            ColorBlock cb = keyField.colors;
            cb.normalColor = Color.white;
            keyField.colors = cb;

            //Flip bools to wait for key
            waitingForKey = true;
            inputSelected = false;
        }
    }

    void PopulateDropdown()
    {
        spellListDropdown.ClearOptions();
        List<string> options = new List<string>();

        foreach (SpellListManager spellListManager in spellListHolder.allSpellLists)
        {
            options.Add(spellListManager.name);
        }

        spellListDropdown.AddOptions(options);

        // List<string> keyOptions = new List<string>();
        // foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        // {
        //     keyOptions.Add(keyCode.ToString());
        // }
        // keyDropdown.AddOptions(keyOptions);
    }

    // void InitializeKeyDropdown()
    // {
    //     keyDropdown.value = (int)KeyCode.Space; // For example, selecting Space key by default
    // }



    void PopulateSpellSlots()
    {
        // Clear existing spell slots
        foreach (Transform child in gridParent.transform)
        {
            Destroy(child.gameObject);
        }

        gridParent.GetComponent<SpellGridPanel>().Instantiate(selectedSpellList);
        Debug.Log("Spell components added" + selectedSpellList.name);

    }

    void AddSpell()
    {
        //Set parent that holds spell nodes
        Transform spellSlotsParent = gridParent.transform;

        //Spin up a new spell sequence
        SpellListManager newSpellListManager = new SpellListManager();

        //save original colours
        ColorBlock cb = addButton.colors;

        //Assign a list of nodes
        List<SpellNode> nodeList = new List<SpellNode>();

        //Find the head node
        foreach (Transform child in spellSlotsParent)
        {
            SpellNodeSlot spellSlot = child.GetComponent<SpellNodeSlot>();
            spellSlot.node.position = child.localPosition;
            // Debug.Log("Setting Node position to: " + spellSlot.node.position);
            nodeList.Add(spellSlot.node);
            if (spellSlot.node.previous == null && spellSlot.node.next != null)
            {
                newSpellListManager.AddSpell(spellSlot.node);
            }
        }

        //If no head or there no valid keycode flare red
        if (newSpellListManager.head == null || !IsValidKeyCode(keyField.text)) //no head to spell
        {
            Debug.Log("head or keycode invalid");
            cb.selectedColor = Color.red;
            addButton.colors = cb;
        }
        else
        {
            //assign list to manager nodelist, it's a non-recursive list of nodes
            newSpellListManager.nodelist = nodeList;

            addButton.colors = cb;
            keyField.colors = cb;
            //replace
            //KeyCode selectedKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyDropdown.options[keyDropdown.value].text);

            KeyCode selectedKeyCode = (KeyCode)Enum.Parse(typeof(KeyCode), keyField.text, true);

            //Assign a Key and Name to the spell
            newSpellListManager.key = selectedKeyCode;
            newSpellListManager.name = inputField.text;

            //if the name is in spellholder list
            int spellIndex = GetSpellListIndexByName(newSpellListManager.name);
            if (spellIndex < 0)
            {
                //Add the spell to the grimoire
                spellListHolder.allSpellLists.Add(newSpellListManager);
                cb.selectedColor = Color.blue;
            }
            else
            {
                spellListHolder.allSpellLists[spellIndex] = newSpellListManager;
                cb.selectedColor = Color.green;
            }

            PopulateDropdown();

            selectedSpellList = newSpellListManager;
        }

    }

    void RemoveSpell()
    {
        //get dropdown index
        int selectedSpellListIndex = spellListDropdown.value;

        //if index is greater than or equal to 0 and the index is less than the count of the grimoire
        if (selectedSpellListIndex >= 0 && selectedSpellListIndex < spellListHolder.allSpellLists.Count)
        {
            //remove index of both dropdown and grimoire
            spellListHolder.allSpellLists.RemoveAt(selectedSpellListIndex);
            spellListDropdown.options.RemoveAt(selectedSpellListIndex);
            spellListDropdown.RefreshShownValue();
        }
    }


    void OnSpellListDropdownValueChanged(TMP_Dropdown change)
    {
        int selectedSpellListIndex = spellListDropdown.value;
        if (selectedSpellListIndex >= 0 && selectedSpellListIndex < spellListHolder.allSpellLists.Count)
        {
            selectedSpellList = spellListHolder.allSpellLists[selectedSpellListIndex];
            PopulateSpellSlots();
            inputField.text = selectedSpellList.name;
        }
    }


    public int GetSpellListIndexByName(string name)
    {
        // Iterate over each item in the allSpellLists list using a for loop
        for (int i = 0; i < spellListHolder.allSpellLists.Count; i++)
        {
            // Check if the name matches the provided name
            if (spellListHolder.allSpellLists[i].name == name)
            {
                // Return the index of the matching item
                return i;
            }
        }

        // Return -1 if no match is found
        return -1;
    }

    void ClearSpell()
    {
        int childCount = gridParent.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            GameObject.DestroyImmediate(gridParent.transform.GetChild(i).gameObject);
        }
    }

    bool IsValidKeyCode(string key)
    {
        try
        {
            KeyCode keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), key, true);
            return true;
        }
        catch
        {
            return false;
        }
    }

    void OnInputFieldSelect(string text)
    {
        // When input field is selected, start waiting for key press
        inputSelected = true;
    }

    void AddListeners()
    {
        spellListDropdown.onValueChanged.AddListener(delegate
        {
            OnSpellListDropdownValueChanged(spellListDropdown);
        });

        addButton.onClick.AddListener(AddSpell);
        removeButton.onClick.AddListener(RemoveSpell);
        clearButton.onClick.AddListener(ClearSpell);
    }

}
