using UnityEngine;

public class SimpleHealth : MonoBehaviour
{
    // Public variable for the health value
    public float health = 1000f;

    // Mass of the current object (set this if the object doesn't have a Rigidbody)
    public float mass = 1f;

    // Method called when this object collides with another collider
    void OnCollisionEnter(Collision collision)
    {
        // Calculate the collision force
        float collisionForce;

        if (collision.rigidbody != null)
        {
            // Calculate the collision force using the other object's mass and velocity
            collisionForce = collision.relativeVelocity.magnitude * collision.rigidbody.mass;
        }
        else
        {
            // Calculate the collision force using the mass of this object and relative velocity
            collisionForce = collision.relativeVelocity.magnitude * mass;
        }

        // Reduce health based on the collision force divided by 100
        health -= collisionForce / 100f;

        // Clamp health to ensure it doesn't drop below 0
        health = Mathf.Max(health, 0f);

        // Log the new health value for debugging purposes
        // Debug.Log("Health: " + health);

        // Check if health is 0 and destroy the game object if it is
        if (health == 0f)
        {
            Destroy(gameObject);
        }
    }
}
