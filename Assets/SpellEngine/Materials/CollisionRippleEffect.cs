using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class CollisionRippleEffect : MonoBehaviour
{
    private Material material;
    private int collisionOriginID;
    private int collisionTimeID;

    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        collisionOriginID = Shader.PropertyToID("_CollisionOrigin");
        collisionTimeID = Shader.PropertyToID("_CollisionTime");

        // Initialize shader properties to default values (optional)
        material.SetVector(collisionOriginID, Vector3.zero);
        material.SetFloat(collisionTimeID, 0f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collisionOriginID != null && collisionTimeID != null && material != null)
        {
            Vector3 collisionPoint = collision.contacts[0].point;
            float collisionTime = Time.time;


            // Update shader properties with collision data
            material.SetVector(collisionOriginID, collisionPoint);
            material.SetFloat(collisionTimeID, collisionTime);

            //Destroy(gameObject);
        }
    }
}
