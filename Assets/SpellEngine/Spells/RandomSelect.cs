using System.Collections.Generic;
using UnityEngine;

public class RandomSelect : MonoBehaviour, ICast
{
    public List<GameObject> affectedObjects;
    // Implementation of the Cast method from the ICast interface
    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        if (inputs == null || inputs.Count == 0)
        {
            return inputs;
        }

        // Randomly select one GameObject from the inputs list
        int selectedIndex = Random.Range(0, inputs.Count);
        GameObject selectedGameObject = inputs[selectedIndex];

        // Create a new list to hold only the selected GameObject
        List<GameObject> result = new List<GameObject> { selectedGameObject };

        // Remove all other GameObjects from the original list
        inputs.Clear();
        inputs.Add(selectedGameObject);

        affectedObjects = result;
        return result;
    }
    public List<GameObject> ReturnAffectedObjects()
    {
        return affectedObjects;
    }
}
