using UnityEngine;

public class ParticleColourBySpeed : MonoBehaviour
{
    public Rigidbody parentRigidbody;   // Reference to the parent's Rigidbody
    public ParticleSystem particleSystem; // Reference to the Particle System

    private ParticleSystem.ColorBySpeedModule colorBySpeedModule;
    public float maxSpeed = 20f;        // The maximum speed that maps to the end of the color gradient


    void Start()
    {
        // Get the Color by Speed module from the Particle System
        colorBySpeedModule = particleSystem.colorBySpeed;
        colorBySpeedModule.enabled = true;
    }

    void Update()
    {
        // Get the magnitude of the parent's velocity
        float speed = parentRigidbody.velocity.magnitude;
        float minSpeed = 0f;

        // Calculate the adjusted speed
        float adjustedSpeed = Mathf.Clamp(maxSpeed - speed, 1f, maxSpeed);


        // Set the range to use this adjusted speed to sample the gradient
        colorBySpeedModule.range = new Vector2(minSpeed, adjustedSpeed);
    }
}
