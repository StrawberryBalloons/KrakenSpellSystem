using System.Collections.Generic;
using UnityEngine;

public class RandomPointsAroundItems : MonoBehaviour, ICast
{
    [SerializeField] private float maxDistanceFromItem = 5f; // Maximum distance from each item to generate random points
    [SerializeField] private int numberOfPointsPerItem = 3; // Number of random points to generate around each item
    public List<GameObject> affectedObjects;

    // Implementation of the Cast method from the ICast interface
    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        //LOAD PARAMS
        if (parameters.Count == 2 && parameters[0] is float paramDistance && parameters[1] is float paramPoints)
        {
            maxDistanceFromItem = paramDistance;
            numberOfPointsPerItem = (int)paramPoints;
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
                for (int i = 0; i < numberOfPointsPerItem; i++)
                {
                    // Generate random point around the item
                    Vector3 randomOffset = Random.insideUnitSphere * maxDistanceFromItem;
                    Vector3 randomPosition = item.transform.position + randomOffset;

                    // Instantiate a new GameObject as a child of the item
                    GameObject randomPointObject = new GameObject("RandomPoint_" + i);
                    randomPointObject.transform.position = randomPosition;
                    randomPointObject.transform.parent = transform;

                    // Add the instantiated GameObject to the result list
                    result.Add(randomPointObject);
                }
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
        return (maxDistanceFromItem * numberOfPointsPerItem) + 100;
    }
}
