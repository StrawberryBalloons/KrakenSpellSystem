using UnityEngine;

public class LookInMovementDirection : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        // Get the Rigidbody component attached to the same GameObject
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Only rotate if the object is moving
        if (rb.velocity != Vector3.zero)
        {
            // Calculate the rotation needed to look in the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(rb.velocity);

            // Apply the rotation to the GameObject
            transform.rotation = targetRotation;
        }
    }
}
