using UnityEngine;
using TMPro;

public class DropdownInputs : MonoBehaviour
{
    // TMP Dropdown for directional input
    public TMP_Dropdown[] directionDropdowns;  // Array to handle multiple dropdowns

    // Predefined directions for the dropdown options
    private readonly Vector3[] directions = {
        Vector3.up,
        Vector3.down,
        Vector3.left,
        Vector3.right,
        Vector3.forward,
        Vector3.back
    };

    // Method to get the selected directions from all dropdowns
    public Vector3[] GetDirectionsFromDropdowns()
    {
        Vector3[] selectedDirections = new Vector3[directionDropdowns.Length];

        for (int i = 0; i < directionDropdowns.Length; i++)
        {
            int selectedIndex = directionDropdowns[i].value;
            selectedDirections[i] = directions[selectedIndex];  // Map the selected index to the corresponding direction
        }

        return selectedDirections;
    }

    // Method to be called on button click
    public void AddParam()
    {
        // Get the selected directions from the dropdowns
        Vector3[] selectedDirections = GetDirectionsFromDropdowns();

        // Find the GameObject with the "SpellParameters" component
        SpellParameters spellParameters = GetComponent<SpellParameters>();

        if (spellParameters != null)
        {
            // Add the selected directions as parameters
            foreach (Vector3 direction in selectedDirections)
            {
                spellParameters.AddParameter((object)direction);
            }
        }
        else
        {
            Debug.LogWarning("SpellParameters component not found in the scene.");
        }
    }
}
