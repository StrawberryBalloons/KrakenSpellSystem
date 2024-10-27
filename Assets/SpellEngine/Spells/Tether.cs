using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tether : MonoBehaviour, ICast
{
    //COMPLEX many to one
    [SerializeField] private float tetherForce = 10f; // Force magnitude to tether objects to the target position
    [SerializeField] private float maxDistance = 5f; // Maximum distance within which objects are affected
    [SerializeField] private float tetherDuration = 2f; // Duration of tethering effect in seconds
    private float tetherTimer = 0f; // Timer to track tethering duration
    private int counter = 0;
    public List<GameObject> affectedObjects;

    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        Debug.Log("First pass " + counter);
        counter++;
        // Load parameters if provided
        if (parameters != null && parameters.Count == 3 && parameters[0] is float paramForce && parameters[1] is float paramDistance && parameters[2] is float paramDuration)
        {
            Debug.Log("Params applied appropriately");
            tetherForce = paramForce;
            maxDistance = paramDistance;
            tetherDuration = paramDuration;
        }
        else if (parameters.Count == 0)
        {
            // Debug.Log("No params");
        }
        else
        {
            // Debug.Log("Param Error");
        }



        // Tether objects while timer is within duration
        if (affectedObjects != null)
        {

            // Debug.Log("First list check");
            tetherTimer += Time.fixedDeltaTime;

            foreach (var item in affectedObjects)
            {
                if (item != null && inputs.Count > 0 && inputs[0] != null && !affectedObjects.Contains(inputs[0]))
                {
                    Debug.Log("Item: " + item);
                    Debug.Log("Input: " + inputs[0]);
                    Debug.Log("Applying Tether");

                    SpringJoint springJoint = item.AddComponent<SpringJoint>();

                    if (inputs[0].GetComponent<Collider>() != null && inputs[0].GetComponent<Collider>().attachedRigidbody != null)
                    {
                        Debug.Log("Adding Spring Joint " + inputs[0].name);

                        springJoint.connectedBody = inputs[0].GetComponent<Rigidbody>();
                    }
                    else
                    {
                        Debug.Log("Adding Anchored Spring Joint: " + inputs[0].name);
                        Vector3 localPosition = item.transform.InverseTransformPoint(inputs[0].transform.position);
                        springJoint.connectedAnchor = localPosition;
                    }

                    springJoint.spring = 500f; // Adjust the spring force as needed
                    springJoint.maxDistance = maxDistance;
                    springJoint.breakForce = tetherForce;
                    springJoint.damper = 10f;
                    StartCoroutine(RemoveSpringJointAfterTime(springJoint, tetherDuration));
                }
            }
        }

        // Start tethering timer if it's the first cast
        if ((affectedObjects == null || affectedObjects.Count == 0) && inputs != null)
        {
            Debug.Log("Setting affected");
            affectedObjects = new List<GameObject>(inputs);
            // Debug.Log("affected objects set to: " + inputs[0]);
        }

        return affectedObjects;
    }

    public List<GameObject> ReturnAffectedObjects()
    {
        return affectedObjects;
    }

    IEnumerator RemoveSpringJointAfterTime(SpringJoint joint, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(joint);
        Debug.Log("Spring Joint removed after " + delay + " seconds.");
    }

}
