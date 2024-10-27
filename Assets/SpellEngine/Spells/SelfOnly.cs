using System.Collections.Generic;
using UnityEngine;

public class SelfOnly : MonoBehaviour, ICast
{
    // Implementation of the Cast method from the ICast interface
    public List<GameObject> affectedObjects;
    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        List<GameObject> result = new List<GameObject>();

        if (caster != null)
        {
            result.Add(caster);
        }
        affectedObjects = result;

        return result;
    }
    public List<GameObject> ReturnAffectedObjects()
    {
        return affectedObjects;
    }
}
