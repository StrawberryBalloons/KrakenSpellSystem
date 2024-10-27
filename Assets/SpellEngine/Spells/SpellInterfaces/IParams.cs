using UnityEngine;
using System.Collections.Generic;
public interface IParams
{
    void HandleAffectedParams(List<object> affectedParameters);
    List<object> ReturnAffectedParams();
}

/*

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

*/