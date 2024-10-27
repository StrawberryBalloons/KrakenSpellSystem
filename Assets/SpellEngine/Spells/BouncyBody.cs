using System.Collections.Generic;
using UnityEngine;

public class BouncyBody : MonoBehaviour, ICast
{
    public float bounciness = 1;
    public List<GameObject> affectedObjects;

    private List<BouncyScript> addedBouncyScripts = new List<BouncyScript>();

    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        //LOAD PARAMS
        if (parameters.Count == 1 && parameters[0] is float paramFloat)
        {
            bounciness = paramFloat;
        }
        else if (parameters.Count == 0)
        {
            Debug.Log("No params");
        }
        else
        {
            Debug.Log("Param Error");
        }

        // Attach BouncyScript to each affected object, if it has a Rigidbody and Collider
        foreach (var obj in inputs)
        {
            // Ensure the object has both a Rigidbody and Collider
            if (obj.GetComponent<Rigidbody>() == null || obj.GetComponent<Collider>() == null)
            {
                Debug.LogWarning("Cannot add BouncyScript: No Rigidbody or Collider found on " + obj.name);
                continue;
            }

            Debug.Log("Adding bouncy script to " + obj.name);

            var bouncyScript = obj.AddComponent<BouncyScript>();
            bouncyScript.bounciness = bounciness;
            addedBouncyScripts.Add(bouncyScript); // Keep track of added scripts
        }

        // Store the affected objects for later reference
        affectedObjects = inputs;
        return inputs;
    }

    void OnDestroy()
    {
        // Remove the BouncyScript from each affected object when this script is destroyed
        foreach (var bouncyScript in addedBouncyScripts)
        {
            if (bouncyScript != null)
            {
                Destroy(bouncyScript);
            }
        }
    }

    public List<GameObject> ReturnAffectedObjects()
    {
        return affectedObjects;
    }
}
