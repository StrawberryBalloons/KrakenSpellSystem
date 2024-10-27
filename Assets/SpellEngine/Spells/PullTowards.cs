using System.Collections.Generic;
using UnityEngine;

public class PullTowards : MonoBehaviour, ICast
{
    [SerializeField] private float pullForce = 10f; // Force magnitude to pull objects towards the transform
    [SerializeField] private float maxDistance = 5f; // Maximum distance within which objects are affected
    private float maxDistanceSquared;  // Precompute max distance squared for efficiency
    public List<GameObject> affectedObjects;

    private void Awake()
    {
        maxDistanceSquared = maxDistance * maxDistance;  // Precompute maxDistance squared for sqrMagnitude comparison
    }

    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        //LOAD PARAMS
        if (parameters.Count == 2 && parameters[0] is float paramForce && parameters[1] is float paramDistance)
        {
            pullForce = paramForce;
            maxDistance = paramDistance;
            maxDistanceSquared = maxDistance * maxDistance;  // Update maxDistanceSquared
        }
        else if (parameters.Count == 0)
        {
            Debug.Log("No params");
        }
        else
        {
            Debug.Log("Param Error");
        }

        // Ensure affectedObjects and inputs are valid
        if (inputs == null || inputs.Count == 0 || affectedObjects == null || affectedObjects.Count == 0)
        {
            Debug.Log("No valid inputs or affected objects.");
            return affectedObjects;
        }

        // Calculate the average position of affectedObjects
        Vector3 averagePosition = Vector3.zero;
        int validObjectsCount = 0;
        foreach (var affected in affectedObjects)
        {
            if (affected != null)
            {
                averagePosition += affected.transform.position;
                validObjectsCount++;
            }
        }

        // Avoid division by zero
        if (validObjectsCount == 0) return affectedObjects;

        averagePosition /= validObjectsCount;

        // Iterate over inputs and pull them towards the average affected position
        foreach (var input in inputs)
        {
            if (input != null)
            {
                Rigidbody rb = input.GetComponent<Rigidbody>();

                // Skip objects without Rigidbody
                if (rb == null) continue;

                // Calculate the direction and check distance using sqrMagnitude
                Vector3 direction = averagePosition - input.transform.position;
                if (direction.sqrMagnitude < maxDistanceSquared)
                {
                    // Normalize direction and apply force
                    Vector3 pullDirection = direction.normalized;
                    rb.AddForce(pullDirection * pullForce, ForceMode.Impulse);
                }
            }
        }

        return affectedObjects;
    }

    public List<GameObject> ReturnAffectedObjects()
    {
        return affectedObjects;
    }
}
