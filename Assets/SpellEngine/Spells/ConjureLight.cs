using System.Collections.Generic;
using UnityEngine;

public class ConjureLight : MonoBehaviour, ICast
{
    public Light lightPrefab;           // Reference to the light prefab
    public float lightStrength = 1f;    // Default light strength
    public List<GameObject> affectedObjects;  // List of affected objects
    private List<ConjureLightHelper> addedLightHelpers = new List<ConjureLightHelper>();  // Track added light helpers


    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        // Load parameters if any are provided
        if (parameters.Count == 1 && parameters[0] is float paramFloat)
        {
            lightStrength = paramFloat;
        }
        else if (parameters.Count == 0)
        {
            Debug.Log("No params provided, using default light strength.");
        }
        else
        {
            Debug.Log("Param Error");
        }

        // Attach ConjureLightHelper to each GameObject in the inputs list
        foreach (GameObject input in inputs)
        {
            if (input == null) continue;

            // Attach the helper script to the input GameObject
            var lightHelper = input.AddComponent<ConjureLightHelper>();
            lightHelper.lightPrefab = lightPrefab;
            lightHelper.lightStrength = lightStrength;

            // Keep track of the added helper scripts
            addedLightHelpers.Add(lightHelper);
        }

        // Store the affected objects
        affectedObjects = inputs;
        return inputs;
    }



    void OnDestroy()
    {
        // Remove all light helpers from the affected objects when this script is destroyed
        foreach (var helper in addedLightHelpers)
        {
            if (helper != null)
            {
                Destroy(helper);
            }
        }
    }

    public List<GameObject> ReturnAffectedObjects()
    {
        return affectedObjects;
    }
    public float ReturnManaCost()
    {
        return 10 * lightStrength;
    }
}
