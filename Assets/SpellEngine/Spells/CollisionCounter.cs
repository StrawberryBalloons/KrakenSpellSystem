using System.Collections.Generic;
using UnityEngine;

public class CollisionCounter : MonoBehaviour, ICast, IWaitableSpell, CollisionReturn
{
    [SerializeField] private int collisionCountThreshold = 1; // Number of collisions needed to trigger
    private int currentCollisionCount = 0; // Current collision count
    private bool isTriggered = false; // Flag to indicate if threshold is reached

    public float WaitDuration { get; private set; } // Implementing IWaitableSpell interface
    public bool IsTriggered => isTriggered; // Implementing IWaitableSpell interface
    public List<GameObject> affectedObjects;

    private List<CollisionReporter> addedCollisionReporters = new List<CollisionReporter>(); // Track attached reporters

    void Awake()
    {
        WaitDuration = 0f; // No wait time needed for this implementation
    }

    public void ReportCollision(GameObject collidedObject)
    {
        // Increment collision count
        currentCollisionCount++;

        // Check if the collision count has reached the threshold
        if (currentCollisionCount >= collisionCountThreshold)
        {
            isTriggered = true;
        }
    }

    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        isTriggered = false;
        currentCollisionCount = 0;

        // Load parameters
        if (parameters.Count == 1 && parameters[0] is float paramFloat)
        {
            collisionCountThreshold = (int)paramFloat;
        }
        else if (parameters.Count == 0)
        {
            Debug.Log("No params");
        }
        else
        {
            Debug.Log("Param Error");
        }

        // Attach CollisionReporter to each affected object
        foreach (var obj in inputs)
        {
            if (obj != null)
            {
                Debug.Log("Adding collision reporter");
                var reporter = obj.AddComponent<CollisionReporter>();
                reporter.collisionCounter = this;

                // Keep track of the added reporters
                addedCollisionReporters.Add(reporter);
            }
        }

        // Store the affected objects
        affectedObjects = inputs;
        return inputs;
    }

    void OnDestroy()
    {
        // Clean up: Remove the CollisionReporter from each affected object when this script is destroyed
        foreach (var reporter in addedCollisionReporters)
        {
            if (reporter != null)
            {
                Destroy(reporter); // Destroy the added CollisionReporter
            }
        }
    }

    public List<GameObject> ReturnAffectedObjects()
    {
        return affectedObjects;
    }
}
