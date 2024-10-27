using System.Collections.Generic;
using UnityEngine;

public class PushAwayFromCaster : MonoBehaviour, ICast
{
    [SerializeField] private float pushForce = 10f; // Force magnitude to push objects away
    [SerializeField] private float maxDistance = 5f; // Maximum distance within which objects are affected
    public List<GameObject> affectedObjects;

    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        //LOAD PARAMS
        if (parameters.Count == 2 && parameters[0] is float paramForce && parameters[1] is float paramDistance)
        {
            pushForce = paramForce;
            maxDistance = paramDistance;
        }
        else if (parameters.Count == 0)
        {
            Debug.Log("No params");
        }
        else
        {
            Debug.Log("Param Error");
        }

        if (caster == null)
        {
            Debug.LogWarning("Caster GameObject is null. Cannot push items.");
            return inputs;
        }

        foreach (var item in inputs)
        {
            if (item != null)
            {
                // Calculate direction from item to caster
                Vector3 direction = item.transform.position - caster.transform.position;

                // Apply force to push the item away from the caster
                if (direction.magnitude < maxDistance)
                {
                    Vector3 pushDirection = direction.normalized;
                    item.GetComponent<Rigidbody>().AddForce(pushDirection * pushForce, ForceMode.Impulse);
                }
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
