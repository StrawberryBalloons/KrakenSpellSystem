using System.Collections.Generic;
using UnityEngine;

public class PullTowardsCaster : MonoBehaviour, ICast
{
    [SerializeField] private float pullForce = 10f; // Force magnitude to pull objects towards the caster
    [SerializeField] private float maxDistance = 5f; // Maximum distance within which objects are affected
    public List<GameObject> affectedObjects;

    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        //LOAD PARAMS
        if (parameters.Count == 2 && parameters[0] is float paramForce && parameters[1] is float paramDistance)
        {
            pullForce = paramForce;
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

        //MAKE NEW LIST
        if (caster == null)
        {
            Debug.LogWarning("Caster GameObject is null. Cannot pull items.");
            return inputs;
        }

        foreach (var item in inputs)
        {
            if (item != null)
            {
                // Calculate direction from caster to item
                Vector3 direction = caster.transform.position - item.transform.position;

                // Apply force to pull the item towards the caster
                if (direction.magnitude < maxDistance)
                {
                    Vector3 pullDirection = direction.normalized;
                    item.GetComponent<Rigidbody>().AddForce(pullDirection * pullForce, ForceMode.Impulse);
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
    public float ReturnManaCost()
    {
        return (maxDistance * pullForce) + 10;
    }
}
