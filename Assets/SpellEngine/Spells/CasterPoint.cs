using System.Collections.Generic;
using UnityEngine;

public class CasterPoint : MonoBehaviour, ICast
{
    [SerializeField] private Vector3 offsetFromCaster = Vector3.up; // Offset from the caster's position
    public List<GameObject> affectedObjects;

    // Implementation of the Cast method from the ICast interface
    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {

        //LOAD PARAMS
        if (parameters.Count == 1 && parameters[0] is Vector3 paramVector)
        {
            offsetFromCaster = paramVector;
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

        if (caster != null)
        {
            // Calculate specific point around the caster
            Vector3 specificPosition = caster.transform.position + offsetFromCaster;

            // Create a new GameObject at the specific position
            GameObject specificPointObject = new GameObject("SpecificPoint");
            specificPointObject.transform.position = specificPosition;
            specificPointObject.transform.parent = transform;

            // Add the instantiated GameObject to the result list
            result.Add(specificPointObject);
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
        return (Vector3.Distance(offsetFromCaster, Vector3.zero) * 10) + 10;
    }
}
