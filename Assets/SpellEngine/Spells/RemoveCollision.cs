using System.Collections.Generic;
using UnityEngine;

public class RemoveCollision : MonoBehaviour, ICast //to be removed
{
    public float duration = 5f;   // Duration to remove collision
    public int layerToIgnore;     // Layer to ignore collision with
    public List<GameObject> affectedObjects;
    private List<RemoveCollisionHelper> addedHelpers = new List<RemoveCollisionHelper>();

    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        // Load parameters (duration and layer to ignore)
        if (parameters.Count == 2 && parameters[0] is float paramDuration && parameters[1] is int paramLayer)
        {
            duration = paramDuration;
            layerToIgnore = paramLayer;
        }
        else
        {
            Debug.Log("Param Error: Expected duration and layer");
        }

        // Attach RemoveCollisionHelper to each input GameObject
        foreach (var obj in inputs)
        {
            if (obj != null)
            {
                var helper = obj.AddComponent<RemoveCollisionHelper>();
                helper.duration = duration;
                helper.layerToIgnore = layerToIgnore;

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
                Destroy(helper);  // Destroy the helper script, which restores collisions
            }
        }
    }

    public List<GameObject> ReturnAffectedObjects()
    {
        return affectedObjects;
    }
    public float ReturnManaCost()
    {
        return 100;
    }
}
