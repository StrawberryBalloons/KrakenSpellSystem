using UnityEngine;
using TMPro;

public class Vector3Input : MonoBehaviour
{
    // References to the TMP InputFields for x, y, and z components
    public TMP_InputField inputFieldX;
    public TMP_InputField inputFieldY;
    public TMP_InputField inputFieldZ;

    // Method to get the Vector3 from the InputFields
    public Vector3 GetVector3FromInput()
    {
        // Parse the input fields to float values
        float x = ParseInputField(inputFieldX);
        float y = ParseInputField(inputFieldY);
        float z = ParseInputField(inputFieldZ);

        // Create and return the Vector3
        return new Vector3(x, y, z);
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
        // Get the Vector3 from the input fields
        Vector3 vector = GetVector3FromInput();

        // Find the GameObject with the "SpellParameters" component
        SpellParameters spellParameters = GetComponent<SpellParameters>();

        if (spellParameters != null)
        {
            // Call the AddParameter method on the SpellParameters component
            spellParameters.AddParameter((object)vector);
        }
        else
        {
            Debug.LogWarning("SpellParameters component not found in the scene.");
        }
    }
}
