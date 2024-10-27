using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MultiKeyFieldHandler : MonoBehaviour
{
    public CharacterActions characterActions;
    public UIController uIController;
    public List<TMP_InputField> keyFields; // List of key fields
    private bool inputSelected = false;
    private bool waitingForKey = false;
    private TMP_InputField currentSelectedField = null; // Track the currently selected field

    void Start()
    {
        foreach (TMP_InputField keyField in keyFields)
        {
            keyField.onSelect.AddListener(delegate { OnInputFieldSelect(keyField); });
        }
    }

    void Update()
    {
        if (waitingForKey)
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    // Set the input field's text to the name of the key pressed
                    if (currentSelectedField != null)
                    {
                        currentSelectedField.text = keyCode.ToString();
                        waitingForKey = false;

                        // Update CharacterActions or UIController based on the currentSelectedField
                        UpdateActionOrUI(currentSelectedField, keyCode);

                        currentSelectedField = null; // Reset the current selected field
                    }
                    return;
                }
            }
        }
        if (inputSelected)
        {
            waitingForKey = true;
            inputSelected = false;
        }
    }

    void OnInputFieldSelect(TMP_InputField selectedField)
    {
        // When input field is selected, start waiting for key press
        inputSelected = true;
        currentSelectedField = selectedField; // Set the current selected field
    }

    void UpdateActionOrUI(TMP_InputField selectedField, KeyCode keyPressed)
    {
        // Add logic here to update the appropriate action in CharacterActions or UIController
        if (selectedField.name == "Menu")
        {
            uIController.menuKey = keyPressed;
        }
        else if (selectedField.name == "SpellBook")
        {
            uIController.spellBookKey = keyPressed;
        }
        else if (selectedField.name == "Jump")
        {
            characterActions.jumpKey = keyPressed;
        }
        else if (selectedField.name == "Camera")
        {
            characterActions.cameraKey = keyPressed;
        }
        // Add more conditions as needed for other fields
    }
}
