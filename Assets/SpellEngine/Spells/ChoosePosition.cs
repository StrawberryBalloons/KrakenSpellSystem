using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosePosition : MonoBehaviour, ICast
{
    public Vector3 position = Vector3.zero;
    public List<GameObject> affectedObjects;
    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        //LOAD PARAMS
        if (parameters.Count == 1 && parameters[0] is Vector3 paramVector)
        {
            position = paramVector;
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

        foreach (var item in inputs)
        {
            if (item != null)
            {
                // Instantiate a new GameObject as a child
                GameObject point = new GameObject("point");
                point.transform.position = item.transform.position + position;
                point.transform.parent = transform;

                // Add the instantiated GameObject to the result list
                result.Add(point);

            }
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
        return (Vector3.Distance(position, Vector3.zero) * 10) + 10;
    }
}
