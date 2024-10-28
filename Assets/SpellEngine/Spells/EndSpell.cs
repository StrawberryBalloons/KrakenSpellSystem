using System.Collections.Generic;
using UnityEngine;

public class EndSpell : MonoBehaviour, ICast
{
    public List<GameObject> affectedObjects; // List to hold affected objects (if needed, but in this case unused)

    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        Debug.Log("DestroySelfScript: Destroying gameObject " + gameObject.name);

        // Destroy the GameObject this script is attached to
        Destroy(gameObject);

        return inputs; // Return inputs if necessary
    }

    public List<GameObject> ReturnAffectedObjects()
    {
        return affectedObjects; // If you need to track objects, you could add them here
    }
    public float ReturnManaCost()
    {
        return 0;
    }
}
