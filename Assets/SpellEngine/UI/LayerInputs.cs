using UnityEngine;
using TMPro;

public class LayerInputs : MonoBehaviour
{
    // Array of TMP InputFields for layer indices
    public TMP_InputField[] inputFields;

    // Method to get layer indices from the InputFields
    public int[] GetLayerIndicesFromInput()
    {
        int[] values = new int[inputFields.Length];

        for (int i = 0; i < inputFields.Length; i++)
        {
            values[i] = ParseInputField(inputFields[i]);
        }

        return values;
    }

    // Helper method to parse the input field text to int
    private int ParseInputField(TMP_InputField inputField)
    {
        if (int.TryParse(inputField.text, out int value))
        {
            if (value >= 0 && value <= 31) // Unity layers are in the range 0-31
            {
                return value;
            }
            else
            {
                Debug.LogWarning($"Layer index out of range in field {inputField.name}. Returning 0.");
                return 0; // Default value if out of range
            }
        }
        else
        {
            Debug.LogWarning($"Invalid input in field {inputField.name}. Returning 0.");
            return 0; // Default value if parsing fails
        }
    }

    // Method to be called on button click
    public void AddParam()
    {
        // Get the layer indices from the input fields
        int[] values = GetLayerIndicesFromInput();

        // Find the GameObject with the "SpellParameters" component
        SpellParameters spellParameters = FindObjectOfType<SpellParameters>();

        if (spellParameters != null)
        {
            // Call the AddParameter method on the SpellParameters component for each layer index
            foreach (int val in values)
            {
                spellParameters.AddParameter((object)val);
            }
        }
        else
        {
            Debug.LogWarning("SpellParameters component not found in the scene.");
        }
    }
}
