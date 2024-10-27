using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour, ICast
{
    public GameObject fireballPrefab; // Reference to the prefab for the fireball
    public float speed;
    public List<GameObject> affectedObjects;

    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        // Implementation for casting a fireball
        GameObject fireball = Instantiate(fireballPrefab, caster.transform.position, Quaternion.identity);

        List<GameObject> outputs = new List<GameObject>();
        outputs.Add(fireball);

        Vector3 move = transform.forward * speed;
        fireball.GetComponent<Rigidbody>().MovePosition(move);

        affectedObjects = outputs;

        return outputs;
    }
    public List<GameObject> ReturnAffectedObjects()
    {
        return affectedObjects;
    }
}
