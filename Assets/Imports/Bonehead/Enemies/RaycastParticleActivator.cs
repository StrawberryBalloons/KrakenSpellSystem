using UnityEngine;

public class RaycastParticleActivator : MonoBehaviour
{
    // Reference to the particle system to control
    public ParticleSystem particleSystem;
    public Color rayColor = Color.red;

    // Distance for the raycast
    public float raycastDistance = 10f;

    void Update()
    {

        Debug.DrawRay(transform.position, transform.forward * 10, rayColor);
        // Perform the raycast forward from the game object's position
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))
        {
            // Debug.Log("FireBreath");
            // If the raycast hits something, enable the particle system
            if (!particleSystem.isPlaying)
            {
                particleSystem.Play();
            }
        }
        else
        {
            // If the raycast does not hit anything, disable the particle system
            if (particleSystem.isPlaying)
            {
                particleSystem.Stop();
            }
        }
    }


}
