using UnityEngine;

public class FollowPosition : MonoBehaviour
{
    [SerializeField]
    private Transform targetObject; // The object to follow

    void Update()
    {
        if (targetObject != null)
        {
            // Continuously update this object's position to match the target's position
            transform.position = targetObject.position;
        }
        else
        {
            Debug.LogWarning("Target Object is not assigned!", this);
        }
    }
}
