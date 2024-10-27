using UnityEngine;

public class AlignWithGravity : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        // Get the Rigidbody component attached to the GameObject
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Get the direction of gravity (usually Vector3.down in Unity by default)
        Vector3 gravityDirection = Physics.gravity.normalized;

        // Align the GameObject to face the direction of gravity
        // Assuming the object should point downwards in the direction of gravity
        transform.rotation = Quaternion.LookRotation(-gravityDirection);
    }
}
