using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour, ICast
{
    [SerializeField] private List<GameObject> affectedObjects; // List of objects affected by this behavior

    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        // Ensure inputs are valid
        if (inputs == null || inputs.Count == 0)
        {
            Debug.Log("Inputs list is null or empty");
            return null;
        }

        // Update affectedObjects to be inputs on the first cast
        if (affectedObjects == null)
        {
            affectedObjects = new List<GameObject>(inputs);
        }

        // Calculate the average position of all inputs
        Vector3 averagePosition = Vector3.zero;
        int validInputsCount = 0;

        foreach (var input in inputs)
        {
            if (input != null)
            {
                averagePosition += input.transform.position;
                validInputsCount++;
            }
        }

        // Avoid division by zero
        if (validInputsCount == 0)
        {
            Debug.Log("No valid inputs to look at.");
            return affectedObjects;
        }

        averagePosition /= validInputsCount;  // Compute the average position of valid inputs

        // Make each affectedObject look at the average position
        foreach (var item in affectedObjects)
        {
            if (item != null)
            {
                item.transform.LookAt(averagePosition);
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
        return 10;
    }
}
