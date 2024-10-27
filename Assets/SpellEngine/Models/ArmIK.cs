using UnityEngine;

public class ArmIK : MonoBehaviour
{
    public Transform target; // Target position (the hand position)

    public Transform shoulder; // Shoulder joint
    public Transform elbow;    // Elbow joint
    public Transform hand;     // Hand joint

    public bool useElbowRotation = true; // Whether to rotate the elbow to aim the hand properly

    void LateUpdate()
    {
        if (target == null || shoulder == null || elbow == null || hand == null)
        {
            Debug.LogError("One or more IK joints or target are not assigned.");
            return;
        }

        // Calculate vectors
        Vector3 shoulderToHand = target.position - shoulder.position;
        Vector3 shoulderToElbow = elbow.position - shoulder.position;

        // Calculate shoulder rotation
        shoulder.rotation = Quaternion.LookRotation(shoulderToHand, shoulder.up);

        // Calculate elbow rotation if enabled
        if (useElbowRotation)
        {
            Quaternion shoulderToHandRotation = Quaternion.FromToRotation(shoulderToElbow, shoulderToHand);
            elbow.rotation = shoulder.rotation * shoulderToHandRotation;
        }
        else
        {
            // Keep the elbow rotation fixed
        }

        // Position the hand
        hand.position = target.position;
    }
}
