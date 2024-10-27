using System.Collections.Generic;
using UnityEngine;

public class SetToWorldSpace : MonoBehaviour, ICast
{
    // Implementation of the Cast method from the ICast interface
    public List<GameObject> affectedObjects;
    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        foreach (var item in inputs)
        {
            if (item != null)
            {
                // Store the world position and rotation
                Vector3 worldPosition = item.transform.position;
                Quaternion worldRotation = item.transform.rotation;

                // Set the item's parent to null to make it a root object in the scene
                item.transform.SetParent(null);

                // Restore the world position and rotation -- not sure if needed
                // item.transform.position = worldPosition;
                // item.transform.rotation = worldRotation;
            }
        }
        affectedObjects = inputs;

        return inputs;
    }
    public List<GameObject> ReturnAffectedObjects()
    {
        return affectedObjects;
    }
}
