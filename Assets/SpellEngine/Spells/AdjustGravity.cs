using System.Collections.Generic;
using UnityEngine;

public class AdjustGravity : MonoBehaviour, ICast
{
    public Vector3 gravityDirection = Vector3.down; // Default gravity direction
    public List<GameObject> affectedObjects;

    private List<GravityScript> addedGravityScripts = new List<GravityScript>();

    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        //LOAD PARAMS
        if (parameters.Count == 1 && parameters[0] is Vector3 paramVector)
        {
            gravityDirection = paramVector;
        }
        else if (parameters.Count == 0)
        {
            Debug.Log("No params, using default gravity direction.");
        }
        else
        {
            Debug.Log("Param Error");
        }

        // Attach GravityScript to each affected object
        foreach (var obj in inputs)
        {
            // Ensure the object has a Rigidbody
            if (obj.GetComponent<Rigidbody>() == null)
            {
                Debug.LogWarning("Cannot add GravityScript: No Rigidbody found on " + obj.name);
                continue;
            }

            Debug.Log("Adding GravityScript to " + obj.name);

            var gravityScript = obj.AddComponent<GravityScript>();
            gravityScript.gravityDirection = gravityDirection;
            addedGravityScripts.Add(gravityScript); // Keep track of added scripts
        }

        // Store the affected objects for later reference
        affectedObjects = inputs;
        return inputs;
    }

    void OnDestroy()
    {
        // Remove the GravityScript from each affected object when this script is destroyed
        foreach (var gravityScript in addedGravityScripts)
        {
            if (gravityScript != null)
            {
                Destroy(gravityScript);
            }
        }
    }

    public List<GameObject> ReturnAffectedObjects()
    {
        return affectedObjects;
    }
}
