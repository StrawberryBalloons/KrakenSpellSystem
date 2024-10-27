using UnityEngine;
using UnityEngine.UI;

public class DamageFlyout : MonoBehaviour
{
    public TextMesh collisionForceText;  // Reference to the UI Text component
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (collisionForceText == null)
        {
            Debug.LogError("Collision Force Text is not assigned!");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Calculate the collision force
        float collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;

        // Calculate the damage
        float damage = collisionForce / 100f;

        // Update the text component
        if (collisionForceText != null)
        {
            collisionForceText.text = damage.ToString("F2");
        }
    }
}
