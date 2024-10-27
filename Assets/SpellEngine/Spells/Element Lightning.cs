using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour, ICast
{
    public GameObject particleSystemPrefab; // Assign this in the Inspector
    public List<GameObject> affectedObjects;
    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        foreach (GameObject targetGameObject in inputs)
        {
            // Add the ParticleSystemFromNormals script to the target GameObject
            ParticleSystemFromNormals particleScript = targetGameObject.AddComponent<ParticleSystemFromNormals>();
            // Assign the particle system prefab to the script
            particleScript.particleSystemPrefab = particleSystemPrefab;
        }

        affectedObjects = inputs;
        return inputs;
    }
    public List<GameObject> ReturnAffectedObjects()
    {
        return affectedObjects;
    }
}
