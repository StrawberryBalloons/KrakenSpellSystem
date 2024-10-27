using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldOrigin : MonoBehaviour, ICast
{
    public GameObject worldOrigin;
    public List<GameObject> affectedObjects;
    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    { //Needs constraints, some things shouldn't be made to use world
        affectedObjects = inputs;

        inputs.Add(worldOrigin);

        //do nothing
        return inputs;
    }
    public List<GameObject> ReturnAffectedObjects()
    {
        return affectedObjects;
    }
}
