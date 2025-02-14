using UnityEngine;
using UnityEngine.Animations.Rigging;

public class MultiAimController : MonoBehaviour
{
    public CharacterActions characterActions; // Reference to the CharacterActions script
    public Transform defaultView; // The GameObject's default view (Transform)
    public Vector3 defaultLocalPosition; // Default local position for the view
    public float distanceBreak = 10f; // The maximum allowed distance before resetting

    private void FixedUpdate()
    {
        // Ensure CharacterActions is assigned
        if (characterActions == null)
        {
            Debug.LogWarning("CharacterActions reference is missing.");
            return;
        }

        // Check if lookingAt has a valid transform
        if (characterActions.lookingAt != null)
        {
            // Set defaultView to the position of the lookingAt transform
            defaultView.position = characterActions.lookingAt.position;

            // Check the distance between the player and the defaultView
            float distance = Vector3.Distance(transform.position, defaultView.position);

            if (distance > distanceBreak)
            {
                // Reset defaultView to its local position
                defaultView.localPosition = defaultLocalPosition;
            }
        }
        else
        {
            // Debug.Log("CharacterActions.lookingAt is null.");
        }
    }
}
