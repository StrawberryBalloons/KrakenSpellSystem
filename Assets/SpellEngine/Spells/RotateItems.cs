using System.Collections.Generic;
using UnityEngine;

public class RotateItems : MonoBehaviour, ICast
{
    // Rotation angle in degrees
    [SerializeField] private float rotationAngle = 90f;
    public List<GameObject> affectedObjects;

    // Implementation of the Cast method from the ICast interface
    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        //LOAD PARAMS
        if (parameters.Count == 1 && parameters[0] is float paramFloat)
        {
            rotationAngle = paramFloat;
        }
        else if (parameters.Count == 0)
        {
            Debug.Log("No params");
        }
        else
        {
            Debug.Log("Param Error");
        }

        //MAKE NEW LIST
        foreach (var item in inputs)
        {
            if (item != null)
            {
                // Rotate the item around its local Y axis
                item.transform.Rotate(Vector3.up, rotationAngle);
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
