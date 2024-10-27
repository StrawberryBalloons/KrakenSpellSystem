using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSpawner : MonoBehaviour, ICast
{
    public GameObject explosionPrefab; // The explosion VFX prefab

    public List<GameObject> affectedObjects;

    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        affectedObjects = inputs;
        foreach (var obj in inputs)
        {
            SpawnExplosion(obj.transform.position);
        }
        return inputs;
    }

    public List<GameObject> ReturnAffectedObjects()
    {
        return affectedObjects;
    }

    private void SpawnExplosion(Vector3 position)
    {
        Instantiate(explosionPrefab, position, Quaternion.identity);
    }
}
