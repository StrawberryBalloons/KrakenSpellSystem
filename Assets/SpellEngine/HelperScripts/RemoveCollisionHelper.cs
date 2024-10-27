using System.Collections;
using UnityEngine;

public class RemoveCollisionHelper : MonoBehaviour
{
    public int layerToIgnore;
    public float duration;
    private Collider[] colliders;

    void Start()
    {
        // Get all colliders attached to the object
        colliders = GetComponents<Collider>();

        // Ignore collisions with the specified layer
        foreach (var collider in colliders)
        {
            Physics.IgnoreLayerCollision(collider.gameObject.layer, layerToIgnore, true);
        }

        // Start a coroutine to re-enable collision after the specified duration
        StartCoroutine(RestoreCollisionAfterDuration());
    }

    private IEnumerator RestoreCollisionAfterDuration()
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Re-enable collisions with the ignored layer
        foreach (var collider in colliders)
        {
            Physics.IgnoreLayerCollision(collider.gameObject.layer, layerToIgnore, false);
        }

        // Destroy this helper script after restoring the collision
        Destroy(this);
    }

    void OnDestroy()
    {
        // Ensure that collisions are restored when this script is destroyed
        foreach (var collider in colliders)
        {
            Physics.IgnoreLayerCollision(collider.gameObject.layer, layerToIgnore, false);
        }
    }
}
