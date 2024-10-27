using System.Collections.Generic;
using UnityEngine;

public class FilterByLayer : MonoBehaviour, ICast
{
    [SerializeField] private LayerMask layerMask; // Layer mask to filter by
    public List<GameObject> affectedObjects;

    // Implementation of the Cast method from the ICast interface
    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {

        //LOAD PARAMS
        if (parameters.Count == 1 && parameters[0] is LayerMask paramLayer)
        {
            layerMask = paramLayer;
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
        List<GameObject> filteredList = new List<GameObject>();

        foreach (var item in inputs)
        {
            if (item != null && IsInLayerMask(item.layer, layerMask))
            {
                filteredList.Add(item);
            }
        }
        affectedObjects = filteredList;
        return filteredList;
    }

    // Function to check if a layer is in the specified layer mask
    private bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        return layerMask == (layerMask | (1 << layer));
    }
    public List<GameObject> ReturnAffectedObjects()
    {
        return affectedObjects;
    }
}
