using System.Collections.Generic;
using UnityEngine;

public class SetToLocalSpace : MonoBehaviour, ICast
{
    // Implementation of the Cast method from the ICast interface
    public List<GameObject> affectedObjects;
    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        foreach (var item in inputs)
        {
            if (item != null)
            {
                // Set the item's parent to the caster's transform
                item.transform.SetParent(caster.transform, true);

                // Optionally, reset local position and rotation to align exactly with the caster's local space
                item.transform.localPosition = Vector3.zero;
                item.transform.localRotation = Quaternion.identity;
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
