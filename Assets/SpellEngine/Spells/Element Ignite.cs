using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ignite : MonoBehaviour, ICast
{
    public GameObject particleSystemPrefab; // Assign this in the Inspector
    public GameObject fireProjectilePrefab; // Assign the fire projectile prefab in the Inspector
    public GameObject burningVFXPrefab; // Assign the burning VFX prefab in the Inspector
    public List<GameObject> affectedObjects;

    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        foreach (GameObject targetGameObject in inputs)
        {
            // Check if the target GameObject is an instance of the ProjectileHolder prefab
            if (targetGameObject.name.Contains("ProjectileHolder(Clone)"))
            {
                // Find the child called "Element"
                Transform elementTransform = targetGameObject.transform.Find("Element");
                if (elementTransform != null)
                {
                    foreach (Transform child in elementTransform)
                    {
                        Destroy(child.gameObject);
                    }

                    // Instantiate the fire projectile prefab as a child of the Element child
                    GameObject fireProjectile = Instantiate(fireProjectilePrefab, elementTransform);
                    //fireProjectile.transform.localPosition = Vector3.zero; // Optional: reset position relative to parent
                }
            }
            else
            {
                // // Instantiate the burning VFX prefab as a child of the input GameObject
                // GameObject burningVFX = Instantiate(burningVFXPrefab, targetGameObject.transform);
                // burningVFX.transform.localPosition = Vector3.zero; // Optional: reset position relative to parent

                // Add the ParticleSystemFromNormals script to the target GameObject
                ParticleSystemFromNormals particleScript = targetGameObject.AddComponent<ParticleSystemFromNormals>();
                // Assign the particle system prefab to the script
                particleScript.particleSystemPrefab = particleSystemPrefab;
            }

        }

        affectedObjects = inputs;
        return inputs;
    }

    public List<GameObject> ReturnAffectedObjects()
    {
        return affectedObjects;
    }
}
