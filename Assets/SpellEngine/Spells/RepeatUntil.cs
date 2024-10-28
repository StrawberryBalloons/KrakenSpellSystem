using System.Collections.Generic;
using UnityEngine;

public class RepeatUntil : MonoBehaviour, ICast, IParams
{
    public int triggerCounter = 1; // Default counter value (how many times to trigger)
    private int currentTriggerCount = 0; // Tracks how many times it's been triggered

    public List<GameObject> affectedObjects; // List to hold affected objects

    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        //LOAD PARAMS
        if (parameters.Count == 1 && parameters[0] is int paramInt)
        {
            triggerCounter = paramInt;
        }
        else if (parameters.Count == 0)
        {
            Debug.Log("No params, using default trigger count.");
        }
        else
        {
            Debug.Log("Param Error");
        }

        Trigger();

        // Attach this RepeatScript to all affected objects
        affectedObjects = inputs;
        return inputs;
    }



    public void HandleAffectedParams(List<object> affectedParameters)
    {
        // Load parameters if any are provided
        if (affectedParameters.Count == 1 && affectedParameters[0] is int paramInt)
        {
            currentTriggerCount = paramInt;
        }
        else if (affectedParameters.Count == 0)
        {
            Debug.Log("No params provided, using default light strength.");
        }
        else
        {
            Debug.Log("Param Error");
        }
    }

    public List<object> ReturnAffectedParams()
    {
        // Affected parameters after cast
        List<object> affectedParameters = new List<object>();
        // Call the AddParameter method on the affectedParameters component
        // foreach (float val in values)
        // {
        affectedParameters.Add((object)currentTriggerCount);
        // }

        return affectedParameters;
    }

    public void Trigger()
    {
        currentTriggerCount++;
        Debug.Log("RepeatScript triggered. Current count: " + currentTriggerCount);

        // Check if the trigger count has reached the specified counter
        if (currentTriggerCount >= triggerCounter)
        {
            Debug.Log("Trigger count reached. Destroying gameObject: " + gameObject.name);
            Destroy(gameObject);
        }
    }

    public List<GameObject> ReturnAffectedObjects()
    {
        return affectedObjects;
    }
    public float ReturnManaCost()
    {
        return (triggerCounter * 10) + 10;
    }
}
