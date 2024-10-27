using UnityEngine;

public class BasicOrbit : MonoBehaviour
{
    public float radius = 5.0f;  // Radius of the circular path
    public float speed = 1.0f;   // Speed of the movement

    private float angle = 0f;    // Current angle

    void Update()
    {
        // Calculate the angle in radians
        angle += speed * Time.deltaTime;

        // Calculate the new position
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;

        // Update the GameObject's position
        transform.position = new Vector3(x, transform.position.y, z);
    }
}
