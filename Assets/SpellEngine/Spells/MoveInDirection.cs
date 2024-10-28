using System.Collections.Generic;
using UnityEngine;

public class MoveInDirection : MonoBehaviour, ICast
{
    [SerializeField] private Vector3 orientationDirection = Vector3.forward; // Direction to orient towards
    public List<GameObject> affectedObjects;

    // Implementation of the Cast method from the ICast interface
    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        //LOAD PARAMS
        if (parameters.Count == 1 && parameters[0] is Vector3 paramVector)
        {
            orientationDirection = paramVector;
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
        foreach (var item in inputs)
        {
            if (item != null)
            {
                // Calculate target rotation based on orientationDirection
                Quaternion targetRotation = Quaternion.LookRotation(orientationDirection);

                // Apply rotation to the item
                item.transform.rotation = targetRotation;
            }
        }

        affectedObjects = inputs;

        // Return the modified inputs list (in this case, it remains unchanged)
        return inputs;
    }
    public List<GameObject> ReturnAffectedObjects()
    {
        return affectedObjects;
    }
    public float ReturnManaCost()
    {
        return (Vector3.Distance(orientationDirection, Vector3.zero) * 10) + 10;
    }
}
