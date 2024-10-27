using UnityEngine;

public class DebugRaycast : MonoBehaviour
{
    public float raycastDistance = 10f;
    public Color rayColor = Color.red;

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, forward, rayColor);
    }
}
