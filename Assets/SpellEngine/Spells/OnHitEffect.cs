using System.Collections.Generic;
using UnityEngine;

public class OnHitEffect : MonoBehaviour, ICast, IWaitableSpell, CollisionReturn
{
    private int collisionCountThreshold = 1; // Number of collisions needed to trigger
    private int currentCollisionCount = 0; // Current collision count
    private bool isTriggered = false; // Flag to indicate if threshold is reached

    public float WaitDuration { get; private set; } // Implementing IWaitableSpell interface
    public bool IsTriggered => isTriggered; // Implementing IWaitableSpell interface
    public List<GameObject> affectedObjects;

    void Awake()
    {
        WaitDuration = 0f; // No wait time needed for this implementation
    }

    public void ReportCollision(GameObject collidedObject)
    {
        // Debug.Log("Collision Reported");
        // Check if the collided object is in the affectedObjects list
        currentCollisionCount++;

        // Check if the collision count has reached the threshold
        isTriggered = true;

        affectedObjects.Add(collidedObject);

    }

    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {

        //LOAD PARAMS
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

            Debug.Log("Adding collision reporter");
            var reporter = obj.AddComponent<CollisionReporter>();
            reporter.collisionCounter = this;
        }

        // Implement ICast interface - not used in this specific script
        affectedObjects = inputs;
        return inputs;
    }
    public List<GameObject> ReturnAffectedObjects()
    {
        return affectedObjects;
    }
}
