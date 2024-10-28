using System.Collections.Generic;
using UnityEngine;

public class ClearInputs : MonoBehaviour, ICast
{
    // Implementation of the Cast method from the ICast interface
    public List<GameObject> affectedObjects;
    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        inputs.Clear();

        // Optionally, return null or an empty list since the method signature requires List<GameObject>
        affectedObjects = new List<GameObject>();
        return new List<GameObject>(); // Return an empty list
    }
    public List<GameObject> ReturnAffectedObjects()
    {
        return affectedObjects;
    }
    public float ReturnManaCost()
    {
        return 1;
    }
}
