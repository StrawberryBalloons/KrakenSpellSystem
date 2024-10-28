using System.Collections.Generic;
using UnityEngine;

public class SetToLocalSpace : MonoBehaviour, ICast
{
    public List<GameObject> affectedObjects;
    public List<GameObject> objectsToUnparent;

    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        if (inputs == null || inputs.Count == 0)
        {
            return null;
        }

        affectedObjects = new List<GameObject>();
        bool resetPositionRotation = parameters.Contains("resetPositionRotation");

        foreach (var item in inputs)
        {
            if (item != null)
            {
                int itemLayer = item.layer;
                if (itemLayer == LayerMask.NameToLayer("player") ||
                    itemLayer == LayerMask.NameToLayer("terrain") ||
                    itemLayer == LayerMask.NameToLayer("water") ||
                    itemLayer == LayerMask.NameToLayer("weather"))
                {
                    continue;
                }

                item.transform.SetParent(caster.transform, true);

                if (resetPositionRotation)
                {
                    item.transform.localPosition = Vector3.zero;
                    item.transform.localRotation = Quaternion.identity;
                }

                objectsToUnparent.Add(item);
                affectedObjects.Add(item);
            }
        }

        return affectedObjects;
    }

    // OnDestroy unparents affected objects from caster if they still exist
    private void OnDestroy()
    {
        foreach (var item in objectsToUnparent)
        {
            if (item != null && item.transform.parent == transform)
            {
                item.transform.SetParent(null); // Unparent from the caster
            }
        }
    }

    public List<GameObject> ReturnAffectedObjects()
    {
        return affectedObjects;
    }
    public float ReturnManaCost()
    {
        return 10 * affectedObjects.Count;
    }
}
