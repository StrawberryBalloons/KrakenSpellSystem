using UnityEngine;
using UnityEngine.UI;

public class TriggerTinkerButton : MonoBehaviour
{
    public void TriggerTinker()
    {
        // Find the ParamPrefab GameObject
        Transform paramPrefab = transform.Find("ParamPrefab");

        // Check if ParamPrefab was found
        if (paramPrefab != null && paramPrefab.gameObject.activeInHierarchy)
        {
            // Find the Tinker child GameObject within ParamPrefab
            Transform tinker = paramPrefab.Find("Tinker");

            // Check if Tinker was found
            if (tinker != null)
            {
                // Get the Button component attached to Tinker
                Button tinkerButton = tinker.GetComponent<Button>();

                // Check if the Button component exists
                if (tinkerButton != null)
                {
                    // Trigger the OnClick event
                    tinkerButton.onClick.Invoke();
                    Debug.Log("Tinker button clicked.");
                }
                else
                {
                    Debug.LogError("Tinker does not have a Button component.");
                }
            }
        }
    }
}
