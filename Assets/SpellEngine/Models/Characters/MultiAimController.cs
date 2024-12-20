using UnityEngine;
using UnityEngine.Animations.Rigging;

public class MultiAimController : MonoBehaviour
{
    public MultiAimConstraint multiAimConstraint; // Reference to the MultiAimConstraint component
    public GameObject player;
    public Transform[] newSourceObjects; // Array of transforms to assign as source objects
    public GameObject defaultView;

    private Transform currentFocus;

    void Start()
    {
        // Initialize the focus at the start
        currentFocus = player.GetComponent<CharacterActions>().focus.transform;

        // Ensure we have a reference to the MultiAimConstraint
        if (multiAimConstraint == null)
        {
            Debug.LogError("MultiAimConstraint reference is missing!");
        }
    }

    void FixedUpdate()
    {
        // Check if the focus has changed
        Transform newFocus;
        if (player.GetComponent<CharacterActions>().focus.transform == null)
        {
            newFocus = defaultView.transform;
        }
        else
        {
            newFocus = player.GetComponent<CharacterActions>().focus.transform;
        }

        if (newFocus != currentFocus)
        {
            currentFocus = newFocus; // Update the current focus

            // Ensure we have a reference to the MultiAimConstraint
            if (multiAimConstraint != null)
            {
                // Get the current data from the MultiAimConstraint
                MultiAimConstraintData data = multiAimConstraint.data;

                // Ensure the newSourceObjects array has enough space for the new source
                var sourceObjectsList = new System.Collections.Generic.List<Transform>(newSourceObjects);

                // Add the new transform (player's updated focus) to the list
                sourceObjectsList.Add(currentFocus);

                // Convert the list back to an array
                newSourceObjects = sourceObjectsList.ToArray();

                // Create a new WeightedTransformArray and manually populate it
                WeightedTransformArray weightedArray = new WeightedTransformArray(newSourceObjects.Length);

                // Populate the WeightedTransformArray
                for (int i = 0; i < newSourceObjects.Length; i++)
                {
                    var weightedTransform = weightedArray[i]; // Access the element

                    weightedTransform.transform = newSourceObjects[i];
                    weightedTransform.weight = 1f; // Set the weight (you can modify this as needed)

                    weightedArray[i] = weightedTransform; // Reassign the modified element
                }

                // Assign the updated array to the data
                data.sourceObjects = weightedArray;

                // Apply the updated data back to the MultiAimConstraint
                multiAimConstraint.data = data;
            }
            else
            {
                Debug.LogError("MultiAimConstraint reference is missing!");
            }
        }
    }
}
