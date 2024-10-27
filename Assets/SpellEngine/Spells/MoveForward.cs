using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour, ICast
{
    public float speed = 1f;
    public List<GameObject> affectedObjects;

    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        //LOAD PARAMS
        if (parameters.Count == 1 && parameters[0] is float paramFloat)
        {
            speed = paramFloat;
        }
        else if (parameters.Count == 0)
        {
            Debug.Log("No params");
        }
        else
        {
            Debug.Log("Param Error");
        }

        foreach (GameObject input in inputs)
        {
            Debug.Log("moving: " + input);
            // Set a speed
            Vector3 move = input.transform.forward * speed;

            //check for rigidbody
            Rigidbody rb = input.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //apply vector
                //rb.MovePosition(move);
                rb.AddForce(move, ForceMode.Force);
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
