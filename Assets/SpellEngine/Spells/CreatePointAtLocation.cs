using System.Collections.Generic;
using UnityEngine;

public class CreatePointAtLocation : MonoBehaviour, ICast
{
    [SerializeField] private Vector3 pointLocation = Vector3.zero; // Global variable for the specific point location
    public List<GameObject> affectedObjects;

    // Implementation of the Cast method from the ICast interface
    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        //LOAD PARAMS
        if (parameters.Count == 1 && parameters[0] is Vector3 paramLocation)
        {
            pointLocation = paramLocation;  // Use the provided location
        }
        else if (parameters.Count == 0)
        {
            Debug.Log("No params, using default location.");
        }
        else
        {
            Debug.Log("Param Error");
        }

        // MAKE NEW LIST
        List<GameObject> result = new List<GameObject>();

        foreach (var item in inputs)
        {
            if (item != null)
            {
                // Create a point at the predefined location
                GameObject pointObject = new GameObject("PointAtLocation");
                pointObject.transform.position = pointLocation;  // Set position to the predefined point
                pointObject.transform.parent = transform;  // Parent it to the current object

                // Add the instantiated GameObject to the result list
                result.Add(pointObject);
            }
        }
        affectedObjects = result;

        return result;
    }

    // Method to return the affected objects
    public List<GameObject> ReturnAffectedObjects()
    {
        return affectedObjects;
    }
    public float ReturnManaCost()
    {
        return (Vector3.Distance(pointLocation, Vector3.zero) * 10) + 10;
    }
}
