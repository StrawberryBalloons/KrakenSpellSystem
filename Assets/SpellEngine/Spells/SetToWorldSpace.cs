using System.Collections.Generic;
using UnityEngine;

public class SetToWorldSpace : MonoBehaviour, ICast // to undo SetToLocalSpace
{
    public float ManaCost = 10;
    public List<GameObject> affectedObjects;

    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        if (inputs == null || inputs.Count == 0)
        {
            return null;
        }

        affectedObjects = new List<GameObject>();

        foreach (var item in inputs)
        {
            if (item != null)
            {
                // Store the world position and rotation
                Vector3 worldPosition = item.transform.position;
                Quaternion worldRotation = item.transform.rotation;

                // Unparent the item to make it a root object in the scene
                item.transform.SetParent(null);

                // Restore the world position and rotation
                item.transform.position = worldPosition;
                item.transform.rotation = worldRotation;

                affectedObjects.Add(item);
            }
        }

        return affectedObjects;
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
