using System.Collections.Generic;
using UnityEngine;

public class IKTargetMover : MonoBehaviour
{
    [System.Serializable]
    public struct IKTarget
    {
        public Transform target;         // The IK target
        public Transform[] positions;   // Positions to transition for this IK target
    }

    [System.Serializable]
    public struct PositionSet
    {
        public string name;                // Name of the position set
        public IKTarget[] ikTargets;       // Array of IK targets with their respective positions
        public float speed;                // Movement speed for the entire set
        public bool loop;                  // Should the set loop?
        public KeyCode activationKey;      // Keybinding to activate this set
    }

    [Header("Position Sets")]
    public PositionSet[] positionSets;     // Array of position sets

    private PositionSet currentSet;        // Active position set
    private PositionSet nextSet;           // Target position set
    private Dictionary<Transform, int> currentIndices; // Track current position indices per target
    private bool isTransitioning;          // Flag for transition between sets

    private Dictionary<string, PositionSet> setLookup; // Lookup for triggering by name

    private void Start()
    {
        // Initialize set lookup
        setLookup = new Dictionary<string, PositionSet>();
        foreach (var set in positionSets)
        {
            setLookup[set.name] = set;
        }

        // Initialize current indices
        currentIndices = new Dictionary<Transform, int>();
        if (positionSets.Length > 0)
        {
            currentSet = positionSets[0];
            InitializeIndices(currentSet);
        }
    }

    private void Update()
    {
        // Check for input to trigger position sets
        foreach (var set in positionSets)
        {
            if (Input.GetKeyDown(set.activationKey))
            {
                TriggerSet(set.name);
                break;
            }
        }

        // Update positions for current set
        if (!isTransitioning)
        {
            UpdatePositionSet(currentSet);
        }
        else
        {
            // Handle transitions
            UpdateTransition();
        }
    }

    private void InitializeIndices(PositionSet set)
    {
        currentIndices.Clear();
        foreach (var ikTarget in set.ikTargets)
        {
            if (ikTarget.target != null && ikTarget.positions.Length > 0)
            {
                currentIndices[ikTarget.target] = 0;
            }
        }
    }

    private void UpdatePositionSet(PositionSet set)
    {
        foreach (var ikTarget in set.ikTargets)
        {
            if (ikTarget.target == null || ikTarget.positions.Length == 0) continue;

            var targetTransform = ikTarget.target;
            int index = currentIndices[targetTransform];
            Transform currentPosition = ikTarget.positions[index];

            // Move the IK target
            targetTransform.position = Vector3.MoveTowards(
                targetTransform.position,
                currentPosition.position,
                set.speed * Time.deltaTime
            );

            // Check if the target reached the position
            if (Vector3.Distance(targetTransform.position, currentPosition.position) < 0.01f)
            {
                // Advance to the next position or loop
                index++;
                if (index >= ikTarget.positions.Length)
                {
                    index = set.loop ? 0 : ikTarget.positions.Length - 1;
                }
                currentIndices[targetTransform] = index;
            }
        }
    }

    private void UpdateTransition()
    {
        bool transitionComplete = true;

        foreach (var ikTarget in nextSet.ikTargets)
        {
            if (ikTarget.target == null || ikTarget.positions.Length == 0) continue;

            Transform targetTransform = ikTarget.target;
            Transform transitionTarget = ikTarget.positions[0];

            // Move the IK target toward the start of the new set
            targetTransform.position = Vector3.MoveTowards(
                targetTransform.position,
                transitionTarget.position,
                nextSet.speed * Time.deltaTime
            );

            // Check if transition is complete for this target
            if (Vector3.Distance(targetTransform.position, transitionTarget.position) > 0.01f)
            {
                transitionComplete = false;
            }
        }

        if (transitionComplete)
        {
            // Complete the transition
            currentSet = nextSet;
            InitializeIndices(currentSet);
            isTransitioning = false;
        }
    }

    /// <summary>
    /// Trigger a position set by name.
    /// </summary>
    /// <param name="setName">The name of the position set to activate.</param>
    public void TriggerSet(string setName)
    {
        if (setLookup.TryGetValue(setName, out var set))
        {
            if (currentSet.name != set.name)
            {
                nextSet = set;
                isTransitioning = true;
            }
        }
        else
        {
            Debug.LogWarning($"Position set with name '{setName}' not found.");
        }
    }
}
