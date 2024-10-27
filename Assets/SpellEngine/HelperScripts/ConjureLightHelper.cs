using UnityEngine;

public class ConjureLightHelper : MonoBehaviour
{
    public Light lightInstance;   // The spawned light instance
    public Light lightPrefab;     // The light prefab to spawn
    public float lightStrength = 1f;

    void Start()
    {
        // Spawn the light instance at the GameObject's position and orientation
        lightInstance = Instantiate(lightPrefab, transform.position, transform.rotation);

        // Set the lightInstance as a child of the GameObject this script is attached to
        lightInstance.transform.SetParent(transform);

        // Set the light intensity and other parameters as needed
        lightInstance.intensity = lightStrength;
    }

    void OnDestroy()
    {
        // Destroy the light instance when this script is destroyed
        if (lightInstance != null)
        {
            Destroy(lightInstance.gameObject);
        }
    }
}
