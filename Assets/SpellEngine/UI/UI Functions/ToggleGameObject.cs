using UnityEngine;
using UnityEngine.UI;

public class ToggleGameObject : MonoBehaviour
{
    // Assign the GameObject you want to toggle in the Inspector
    public GameObject targetObject;

    // Reference to the Button component
    private Button toggleButton;

    void Start()
    {
        // Get the Button component attached to this GameObject
        toggleButton = GetComponent<Button>();

        // Add a listener to the button's onClick event
        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(ToggleObject);
        }
    }

    // Method to toggle the target GameObject's active state
    void ToggleObject()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(!targetObject.activeSelf);
        }
    }
}
