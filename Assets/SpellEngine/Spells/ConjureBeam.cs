using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConjureBeam : MonoBehaviour, ICast
{
    public GameObject spherePrefab; // Reference to the prefab for the fireball
    public List<GameObject> affectedObjects;

    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        List<GameObject> outputs = new List<GameObject>();


        // Iterate over all GameObjects in the inputs list
        foreach (GameObject input in inputs)
        {
            // Create a sphere at the position and rotation of the input GameObject
            GameObject sphere = Instantiate(spherePrefab, input.transform.position + input.transform.forward, input.transform.rotation);

            // Add the created sphere to the outputs list
            outputs.Add(sphere);
        }

        affectedObjects = outputs;
        return outputs;
    }
    public List<GameObject> ReturnAffectedObjects()
    {
        return affectedObjects;
    }
}
