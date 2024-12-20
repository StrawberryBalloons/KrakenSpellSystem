using UnityEngine;

public class IKTargetAdjuster : MonoBehaviour
{
    [Header("References")]
    public Transform handLeft;        // Reference to the left hand bone
    public Transform handRight;       // Reference to the right hand bone
    public Transform footLeft;        // Reference to the left foot bone
    public Transform footRight;       // Reference to the right foot bone

    public Transform ikTargetHandLeft;  // IK Target for left hand
    public Transform ikTargetHandRight; // IK Target for right hand
    public Transform ikTargetFootLeft;  // IK Target for left foot
    public Transform ikTargetFootRight; // IK Target for right foot

    [Header("Adjustment Settings")]
    public float offsetDistance = 0.2f; // Distance to offset the IK target from the bone
    [Header("Offsets")]
    public Vector3 positionOffset = new Vector3(0, 0, -1);
    public Vector3 rotationOffset = Vector3.zero; // Optional manual rotation offset

    [Header("Distance Capping")]
    public float maxDistance = 5f; // Maximum distance the target can be from the shoulder/hip

    [Header("Rotation Ranges for Foot Left")]
    [Range(-180f, 180f)] public float maxRotationXFootLeft = 45f; // Max rotation on X axis for left foot
    [Range(-180f, 180f)] public float maxRotationYFootLeft = 45f; // Max rotation on Y axis for left foot
    [Range(-180f, 180f)] public float maxRotationZFootLeft = 45f; // Max rotation on Z axis for left foot

    [Header("Rotation Ranges for Foot Right")]
    [Range(-180f, 180f)] public float maxRotationXFootRight = 45f; // Max rotation on X axis for right foot
    [Range(-180f, 180f)] public float maxRotationYFootRight = 45f; // Max rotation on Y axis for right foot
    [Range(-180f, 180f)] public float maxRotationZFootRight = 45f; // Max rotation on Z axis for right foot

    [Header("Look Away Options")]
    public bool shouldLookAway = true; // Whether to apply LookAwayFromTarget behavior

    void LateUpdate()
    {
        // Conditionally apply LookAwayFromTarget based on the shouldLookAway flag
        if (shouldLookAway)
        {
            LookAwayFromTargetWithLine(ikTargetHandLeft, handLeft);
            LookAwayFromTargetWithLine(ikTargetHandRight, handRight);
        }

        // For feet, keep the original LookAwayFromTarget logic
        LookAwayFromTarget(ikTargetFootLeft, footLeft);
        LookAwayFromTarget(ikTargetFootRight, footRight);

        // Cap the distance between the targets and their parent
        CapTargetDistance(ikTargetHandLeft, handLeft);
        CapTargetDistance(ikTargetHandRight, handRight);
        CapTargetDistance(ikTargetFootLeft, footLeft);
        CapTargetDistance(ikTargetFootRight, footRight);

        // Apply rotation limits for feet in local space
        LimitRotationLocal(ikTargetFootLeft, maxRotationXFootLeft, maxRotationYFootLeft, maxRotationZFootLeft);
        LimitRotationLocal(ikTargetFootRight, maxRotationXFootRight, maxRotationYFootRight, maxRotationZFootRight);
    }

    public static void LookAwayFromTarget(Transform source, Transform target)
    {
        // Calculate the direction away from the target
        Vector3 directionAway = (source.position - target.position).normalized;

        // Calculate the new rotation
        Quaternion lookAwayRotation = Quaternion.LookRotation(directionAway);

        // Apply the rotation to the source object
        source.rotation = lookAwayRotation;
    }

    public void LookAwayFromTargetWithLine(Transform source, Transform target)
    {
        // Draw a line from the hand to its parent (shoulder/hip)
        if (target != null && target.parent != null)
        {
            Debug.DrawLine(source.position, target.parent.position, Color.red, 0.1f);
        }

        // Calculate the direction away from the parent (line from hand to shoulder/hip)
        Vector3 directionAway = (source.position - target.parent.position).normalized;

        // Calculate the new rotation
        Quaternion lookAwayRotation = Quaternion.LookRotation(directionAway);

        // Apply the rotation to the source object
        source.rotation = lookAwayRotation;
    }

    public void CapTargetDistance(Transform ikTarget, Transform parent)
    {
        // Get the parent's parent (e.g., shoulder, hip)
        Transform baseTransform = parent.parent;

        // Calculate the direction from the base transform to the ikTarget
        Vector3 direction = ikTarget.position - baseTransform.position;

        // If the distance between the base and the target exceeds the max distance, adjust it
        if (direction.magnitude > maxDistance)
        {
            // Normalize the direction and set the target position within the max distance
            ikTarget.position = baseTransform.position + direction.normalized * maxDistance;
        }
    }

    public void LimitRotationLocal(Transform target, float maxX, float maxY, float maxZ)
    {
        // Get current rotation in local space
        Vector3 localEulerRotation = target.localEulerAngles;

        // Normalize the local rotation values to the -180 to 180 range
        if (localEulerRotation.x > 180f) localEulerRotation.x -= 360f;
        if (localEulerRotation.y > 180f) localEulerRotation.y -= 360f;
        if (localEulerRotation.z > 180f) localEulerRotation.z -= 360f;

        // Clamp each axis of the rotation based on the provided range
        localEulerRotation.x = Mathf.Clamp(localEulerRotation.x, -maxX, maxX);
        localEulerRotation.y = Mathf.Clamp(localEulerRotation.y, -maxY, maxY);
        localEulerRotation.z = Mathf.Clamp(localEulerRotation.z, -maxZ, maxZ);

        // Apply the clamped rotation back to the target's local rotation
        target.localEulerAngles = localEulerRotation;
    }
}
