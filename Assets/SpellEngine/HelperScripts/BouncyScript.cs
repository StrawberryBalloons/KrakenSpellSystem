using UnityEngine;

public class BouncyScript : MonoBehaviour
{
    public float bounciness = 1.0f; // How bouncy you want the Rigidbody to be
    public float friction = 0.0f;   // Set to 0 for no friction

    private Rigidbody rb;
    private Collider col;
    private PhysicMaterial originalMaterial; // To store the original PhysicMaterial

    void Start()
    {
        // Try to get the Rigidbody and Collider components
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        // Check if both Rigidbody and Collider exist, otherwise destroy the script
        if (rb == null || col == null)
        {
            Debug.LogWarning("BouncyScript removed: No Rigidbody or Collider found on the GameObject.");
            Destroy(this);
            return;
        }

        // Store the original material so we can revert changes later
        originalMaterial = col.material;

        // Create a new PhysicMaterial for bounciness
        PhysicMaterial bouncyMaterial = new PhysicMaterial();

        // Set bounciness and friction
        bouncyMaterial.bounciness = bounciness;
        bouncyMaterial.dynamicFriction = friction;
        bouncyMaterial.staticFriction = friction;

        // Set bounceCombine to Maximum to get the highest bounce result
        bouncyMaterial.bounceCombine = PhysicMaterialCombine.Maximum;

        // Assign the new PhysicMaterial to the object's collider
        col.material = bouncyMaterial;
    }

    void OnDestroy()
    {
        // Restore the original PhysicMaterial when the script is destroyed
        if (col != null && originalMaterial != null)
        {
            col.material = originalMaterial;
        }
    }
}
