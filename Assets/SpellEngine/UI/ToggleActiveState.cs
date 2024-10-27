using UnityEngine;
using UnityEngine.UI;

public class ToggleActiveState : MonoBehaviour
{
    public GameObject targetObject; // The object to toggle

    public void ToggleObjectActiveState()
    {
        if (targetObject != null)
        {
            bool isActive = targetObject.activeSelf;
            targetObject.SetActive(!isActive);
        }
    }
}
