using UnityEngine;

public class IKColliderAvoidance : MonoBehaviour
{
    public Transform IKTarget; // The IK Target transform
    public LayerMask collisionLayer; // Layer mask for collidable objects
    public float avoidDistance = 0.1f; // Minimum distance to keep from colliders
    public float raycastLength = 0.5f; // Length of raycast for collision detection

    private Vector3 originalPosition;

    void LateUpdate()
    {
        if (IKTarget == null) return;

        // Store the original position
        originalPosition = IKTarget.position;

        // Check for collision using raycast
        if (Physics.Raycast(originalPosition, Vector3.down, out RaycastHit hit, raycastLength, collisionLayer))
        {
            // Adjust the position to avoid the collider
            Vector3 avoidPosition = hit.point + hit.normal * avoidDistance;

            // Apply the adjusted position to the IK target
            IKTarget.position = Vector3.Lerp(originalPosition, avoidPosition, 0.5f);
        }
    }
}
