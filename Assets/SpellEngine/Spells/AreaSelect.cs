using System.Collections.Generic;
using UnityEngine;

public class AreaSelect : MonoBehaviour, ICast
{
    [SerializeField] private float searchRadius = 5f; // Radius to search for nearby objects
    [SerializeField] private LayerMask layerToInclude; // Layer to include in selection
    public List<GameObject> affectedObjects;

    // Implementation of the Cast method from the ICast interface
    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        //LOAD PARAMS
        if (parameters.Count == 2 && parameters[0] is float paramFloat && parameters[1] is LayerMask paramLayer)
        {
            searchRadius = paramFloat;
            layerToInclude = paramLayer;
        }
        else if (parameters.Count == 0)
        {
            Debug.Log("No params");
        }
        else
        {
            Debug.Log("Param Error");
        }

        //MAKE NEW LIST
        List<GameObject> result = new List<GameObject>();

        //ITERATE OVER LIST
        foreach (var item in inputs)
        {
            if (item != null)
            {
                // Find nearby objects using Physics.OverlapSphere with layerToInclude mask
                Collider[] nearbyColliders = Physics.OverlapSphere(item.transform.position, searchRadius, layerToInclude);
                Debug.Log("Including Layer: " + layerToInclude);

                // Add nearby objects to the result list
                foreach (var collider in nearbyColliders)
                {
                    GameObject nearbyObject = collider.gameObject;
                    if (!result.Contains(nearbyObject))
                    {
                        result.Add(nearbyObject);
                        Debug.Log("Area Selected: " + nearbyObject);
                    }
                }

                // Optionally, add the original item back to the result list
                // if (!result.Contains(item))
                // {
                //     result.Add(item);
                // }
            }
        }
        affectedObjects = result;
        return result;
    }

    public List<GameObject> ReturnAffectedObjects()
    {
        return affectedObjects;
    }
    public float ReturnManaCost()
    {
        return 10f * searchRadius;
    }
}
