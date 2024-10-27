using UnityEngine;

public class GravityScript : MonoBehaviour
{
    public Vector3 gravityDirection = Vector3.down; // Custom gravity direction
    public float gravityStrength = 1.0f;            // Strength of the custom gravity

    private Rigidbody rb;

    void Start()
    {
        // Try to get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // If there's no Rigidbody, destroy this script
        if (rb == null)
        {
            Debug.LogWarning("GravityScript removed: No Rigidbody found on the GameObject.");
            Destroy(this);
            return;
        }

        // Keep Unity's gravity enabled, so we add to it
        rb.useGravity = true;
    }

    void FixedUpdate()
    {
        // Apply the additional custom gravity to the Rigidbody
        Vector3 additionalGravity = gravityDirection.normalized * gravityStrength * Physics.gravity.magnitude;
        rb.AddForce(additionalGravity, ForceMode.Acceleration);
    }

    void OnDestroy()
    {
        // No special action needed since we're adding force to Unity's gravity
        // Unity's gravity will remain enabled after this script is destroyed.
    }
}
