using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour, ICast
{
    public GameObject playerCamera;
    public List<GameObject> affectedObjects;
    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        List<GameObject> outputs = new List<GameObject>();
        outputs.Add(playerCamera);

        affectedObjects = outputs;

        return outputs;
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
