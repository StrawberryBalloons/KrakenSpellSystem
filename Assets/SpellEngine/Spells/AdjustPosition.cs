using System.Collections.Generic;
using UnityEngine;

public class AdjustPosition : MonoBehaviour, ICast
{
    [SerializeField] private float power = 10f;          // Power of the applied force
    [SerializeField] private Vector3 direction = Vector3.forward;  // Direction of the applied force
    [SerializeField] private List<GameObject> affectedObjects;     // List of objects affected by this behavior

    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {

        //LOAD PARAMS
        if (parameters.Count == 2 && parameters[0] is float paramFloat && parameters[1] is Vector3 paramDropdown)
        {
            power = paramFloat;
            direction = paramDropdown;
        }
        else if (parameters.Count == 0)
        {
            Debug.Log("No params");
        }
        else
        {
            Debug.Log("Param Error");
        }

        // Validate inputs
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

        // Calculate the force to be applied
        Vector3 force = power * direction;

        // Apply the calculated force to each input object's position
        foreach (var input in inputs)
        {
            if (input != null)
            {
                // Add the calculated force to the input's current position
                input.transform.position += force;
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
        return (Vector3.Distance(direction, Vector3.zero) * 10) + 10;
    }
}
