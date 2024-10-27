using System.Collections;
using UnityEngine;

public class AddCollisionHelper : MonoBehaviour
{
    public int layerToAdd;
    public float duration;
    private Collider[] colliders;

    void Start()
    {
        // Get all colliders attached to the object
        colliders = GetComponents<Collider>();

        // Enable collisions with the specified layer
        foreach (var collider in colliders)
        {
            Physics.IgnoreLayerCollision(collider.gameObject.layer, layerToAdd, false);
        }

        // Start a coroutine to disable collision after the specified duration
        StartCoroutine(DisableCollisionAfterDuration());
    }

    private IEnumerator DisableCollisionAfterDuration()
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Re-disable collisions with the specified layer
        foreach (var collider in colliders)
        {
            Physics.IgnoreLayerCollision(collider.gameObject.layer, layerToAdd, true);
        }

        // Destroy this helper script after disabling the collision
        Destroy(this);
    }

    void OnDestroy()
    {
        // Ensure that collisions are re-disabled when this script is destroyed
        foreach (var collider in colliders)
        {
            Physics.IgnoreLayerCollision(collider.gameObject.layer, layerToAdd, true);
        }
    }
}
