using UnityEngine;
using TMPro;

public class FloatInputs : MonoBehaviour
{
    // Array of TMP InputFields for float values
    public TMP_InputField[] inputFields;

    // Method to get float values from the InputFields
    public float[] GetFloatValuesFromInput()
    {
        float[] values = new float[inputFields.Length];

        for (int i = 0; i < inputFields.Length; i++)
        {
            values[i] = ParseInputField(inputFields[i]);
        }

        return values;
    }

    // Helper method to parse the input field text to float
    private float ParseInputField(TMP_InputField inputField)
    {
        if (float.TryParse(inputField.text, out float value))
        {
            return value;
        }
        else
        {
            Debug.LogWarning($"Invalid input in field {inputField.name}. Returning 0.");
            return 0f; // Default value if parsing fails
        }
    }

    // Method to be called on button click
    public void AddParam()
    {
        // Get the float values from the input fields
        float[] values = GetFloatValuesFromInput();

        // Find the GameObject with the "SpellParameters" component
        SpellParameters spellParameters = GetComponent<SpellParameters>();

        if (spellParameters != null)
        {
            // Call the AddParameter method on the SpellParameters component
            foreach (float val in values)
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
