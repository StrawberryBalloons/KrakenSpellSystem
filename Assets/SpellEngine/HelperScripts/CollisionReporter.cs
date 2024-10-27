using UnityEngine;

public class CollisionReporter : MonoBehaviour
{
    public CollisionReturn collisionCounter;

    void OnCollisionEnter(Collision collision)
    {
        if (collisionCounter != null)
        {
            collisionCounter.ReportCollision(collision.gameObject);
        }
        else
        {
            // Remove this script from the GameObject if collisionCounter is null
            Destroy(this);
        }
    }
}
