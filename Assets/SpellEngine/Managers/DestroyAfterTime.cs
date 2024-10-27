using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    // Time in seconds after which the GameObject will be destroyed
    public float timeToDestroy = 5f;

    // Start is called before the first frame update
    void Start()
    {
        // Destroy the GameObject after timeToDestroy seconds
        Destroy(gameObject, timeToDestroy);
    }
}
