using System.Collections.Generic;
using UnityEngine;

public class RaycastForward : MonoBehaviour, ICast
{

    [SerializeField] public float raycastDistance = 10f; // Distance of the raycast
    [SerializeField] public LayerMask layerMask; // Layer mask to filter by
    [SerializeField] private List<int> layerMasks = new List<int>(); // List of layer masks to filter raycast hits
    public List<GameObject> affectedObjects;

    // Implementation of the Cast method from the ICast interface
    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        //LOAD PARAMS
        if (parameters.Count == 1 && parameters[0] is float paramDistance && parameters[1] is LayerMask paramLayers)
        {
            raycastDistance = paramDistance;
            layerMask = paramLayers;
            layerMasks.Add(layerMask);
        }
        else if (parameters.Count == 0)
        {
            // Debug.Log("No params");
        }
        else
        {
            // Debug.Log("Param Error");
        }

        //MAKE NEW LIST
        List<GameObject> result = new List<GameObject>();
        Debug.Log("Raycast if nothing follows no items in inputs");

        foreach (var item in inputs)
        {
            if (item != null)
            {
                Debug.Log("Raycast from " + item);
                // Debug.Log("Item not null");
                // Calculate raycast origin and direction
                Vector3 raycastOrigin = item.transform.position;
                Vector3 raycastDirection = item.transform.forward;

                // Perform raycast for each layer mask
                if (layerMasks.Count == 0)
                {
                    // Debug.Log("LayerMarks list is empty, adding " + layerMask.value + LayerMask.LayerToName(layerMask.value) + " to list");
                    layerMasks.Add(layerMask);
                }
                foreach (var layerMask in layerMasks)
                {

                    Debug.Log("Layer in list");
                    // Debug.Log("raycastDistance and origin " + raycastDistance + " " + raycastOrigin);
                    RaycastHit hit;
                    if (Physics.Raycast(raycastOrigin, raycastDirection, out hit, raycastDistance, layerMask))
                    {

                        Debug.Log("Hit Object: " + hit.collider.gameObject.name);
                        // Debug.Log("Layermask: " + layerMask);
                        GameObject hitObject = hit.collider.gameObject;
                        if (!result.Contains(hitObject))
                        {

                            // Debug.Log("Added to list");
                            result.Add(hitObject);
                            Debug.Log(hitObject.name);
                        }
                    }
                }
            }
        }
        affectedObjects = result;

        return result;
    }
    public List<GameObject> ReturnAffectedObjects()
    {
        return affectedObjects;
    }
}
