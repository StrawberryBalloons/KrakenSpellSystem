using System.Collections.Generic;
using UnityEngine;

public class RemoveFromList : MonoBehaviour, ICast
{
    [SerializeField] private List<GameObject> objectsToRemove; // List of GameObjects to remove
    public List<GameObject> affectedObjects;

    // Implementation of the Cast method from the ICast interface
    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        List<GameObject> filteredList = new List<GameObject>(inputs); // Copy the original inputs list

        if (objectsToRemove == null)
        {
            objectsToRemove = new List<GameObject>(inputs);
        }
        else
        {
            foreach (var itemToRemove in objectsToRemove)
            {
                filteredList.RemoveAll(item => item == itemToRemove); // Remove all occurrences of itemToRemove
            }
        }

        affectedObjects = filteredList;

        return filteredList;
    }
    public List<GameObject> ReturnAffectedObjects()
    {
        return affectedObjects;
    }
}
