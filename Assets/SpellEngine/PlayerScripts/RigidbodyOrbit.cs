using UnityEngine;

public class RigidbodyOrbit : MonoBehaviour
{
    public Transform target; // The object to orbit around
    public float orbitDistance = 5f; // Desired distance from the target
    public float orbitSpeed = 10f; // Speed of orbiting
    public Vector3 orbitAxis = Vector3.up; // Axis around which to orbit (usually Vector3.up)

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Position the object at the correct orbit distance from the target
        Vector3 offset = (transform.position - target.position).normalized * orbitDistance;
        transform.position = target.position + offset;
    }

    void FixedUpdate()
    {
        if (target == null) return;

        // Calculate the direction of the orbit
        Vector3 direction = (transform.position - target.position).normalized;

        // Calculate the perpendicular direction to the orbit axis
        Vector3 perpendicularDirection = Vector3.Cross(direction, orbitAxis).normalized;

        // Calculate the orbit velocity vector
        Vector3 orbitVelocity = perpendicularDirection * orbitSpeed;

        // Apply the orbit velocity to the Rigidbody
        rb.velocity = orbitVelocity + rb.velocity.magnitude * direction;
    }
}
