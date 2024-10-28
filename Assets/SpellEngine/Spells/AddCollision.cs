using System.Collections.Generic;
using UnityEngine;

public class AddCollision : MonoBehaviour, ICast //to be removed
{

    public float duration = 5f;   // Duration to add collision
    public int layerToAdd;        // Layer to add collision with
    public List<GameObject> affectedObjects;
    private List<AddCollisionHelper> addedHelpers = new List<AddCollisionHelper>();

    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        // Load parameters (duration and layer to add collision with)
        if (parameters.Count == 2 && parameters[0] is float paramDuration && parameters[1] is int paramLayer)
        {
            duration = paramDuration;
            layerToAdd = paramLayer;
        }
        else
        {
            Debug.Log("Param Error: Expected duration and layer");
        }

        // Attach AddCollisionHelper to each input GameObject
        foreach (var obj in inputs)
        {
            if (obj != null)
            {
                var helper = obj.AddComponent<AddCollisionHelper>();
                helper.duration = duration;
                helper.layerToAdd = layerToAdd;

                addedHelpers.Add(helper); // Keep track of added helpers
            }
        }

        affectedObjects = inputs;
        return inputs;
    }

    void OnDestroy()
    {
        // Clean up the helpers if the script is destroyed
        foreach (var helper in addedHelpers)
        {
            if (helper != null)
            {
                Destroy(helper);  // Destroy the helper script, which re-disables collisions
            }
        }
    }

    public List<GameObject> ReturnAffectedObjects()
    {
        return affectedObjects;
    }

    public float ReturnManaCost()
    {
        return 100f;
    }
}
