using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

public class SpellListExecutor : MonoBehaviour
{
    public GameObject caster; // The game object casting the spells
    public List<GameObject> inputs = new List<GameObject>(); // Input GameObjects for the spell
    private Queue<SpellNode> queue; // Queue for breadth-first traversal
    public float castStepDelay = 1.0f; // Default delay between each spell cast in seconds
    public float spellDuration = 30f; // Default delay between each spell cast in seconds

    private bool isWaiting = false;
    private bool doPreviousNodeInputs = true;

    void Awake()
    {
        Debug.Log("SpellDuration: " + spellDuration);
        Destroy(gameObject, spellDuration);
    }
    void FixedUpdate()
    {
        // If not waiting and there are nodes to process, execute the next spell node
        if (!isWaiting && queue != null && queue.Count > 0)
        {
            StartCoroutine(ProcessNextSpellNode());
        }
    }

    public void ExecuteSpellList(SpellListManager spellListManager)
    {
        if (caster == null)
        {
            Debug.LogError("Caster is not assigned.");
            return;
        }


        //IF CASTER HAS MANA || IF USE HEALTHFORMANA IS TRUE ELSE DON'T EXECUTE SPELL LIST

        // Deep copy the spell list to ensure no changes to the original++
        SpellListManager copiedHead = spellListManager.DeepCopy();

        // Initialize the queue and enqueue the head node
        queue = new Queue<SpellNode>();
        queue.Enqueue(copiedHead.head);
    }

    private IEnumerator ProcessNextSpellNode()
    {
        if (queue.Count > 0)
        {
            SpellNode currentNode = queue.Dequeue();
            Debug.Log("Executing spell: " + currentNode.spell);

            //If there is a previous node then get the affected objects from that and set it as the new inputs for the current node
            // if (currentNode.previous != null && doPreviousNodeInputs)
            // {
            //     // Debug.Log("Inputs set to previous nodes affected objects: " + currentNode.previous.spell);
            //     ICast findICastComponent = (ICast)gameObject.GetComponent(currentNode.previous.spell.GetType());
            //     if (findICastComponent != null)
            //     {
            //         inputs = findICastComponent.ReturnAffectedObjects();
            //     }
            // }
            // else
            // {
            //     // Debug.Log("Didn't use previous, use previous next time");
            //     doPreviousNodeInputs = true;
            // }


            // If there is a previous node, get the affected objects from that and set them as the new inputs
            if (currentNode.previous != null && doPreviousNodeInputs)
            {
                Debug.Log("Do previous inputs: " + doPreviousNodeInputs);
                Debug.Log(currentNode.previous.affectedObjects[0]);
                inputs = currentNode.previous.affectedObjects; // Load affected objects from the previous node
            }
            else
            {
                doPreviousNodeInputs = true;
            }

            // Check if the script already exists
            ICast spellExe = (ICast)gameObject.GetComponent(currentNode.spell.GetType());
            if (spellExe == null)
            {
                // Add the component to SpellExecutor and get reference
                spellExe = (ICast)gameObject.AddComponent(currentNode.spell.GetType());
                AddICastComponent(spellExe, currentNode);
            }


            // Start with initial inputs, define the objects returned from a cast
            List<GameObject> currentInputs = new List<GameObject>(inputs);
            List<object> currentAffectedParameters = currentNode.affectedParameters;
            List<GameObject> outputs = new List<GameObject>();

            //IParam store affectedParams
            if (currentNode.spell is IParams flexiSpell && currentNode.affectedParameters.Count != 0)
            {
                // Use reflection to get the component of the correct type
                Component spellComponent = gameObject.GetComponent(currentNode.spell.GetType());

                // Ensure the component implements IParams
                if (spellComponent is IParams paramSpell)
                {
                    paramSpell.HandleAffectedParams(currentNode.affectedParameters);

                    outputs = spellExe.Cast(caster, currentInputs, currentNode.parameters);

                    currentNode.affectedParameters = paramSpell.ReturnAffectedParams();
                }
            }
            else
            {
                outputs = spellExe.Cast(caster, currentInputs, currentNode.parameters);
            }

            ManaCostHandler(caster, spellExe.ReturnManaCost());


            // // Execute the spell associated with the current node
            // outputs.AddRange(spellExe.Cast(caster, currentInputs, currentNode.parameters));
            // Debug.Log(outputs[0]);

            // Save the outputs and parameters in the current node for further use
            currentNode.affectedObjects = outputs; // Store the affected objects in the node
            // Update currentInputs for the next iteration
            inputs = outputs;


            // Enqueue all next nodes for traversal
            foreach (var nextNode in currentNode.next)
            {

                queue.Enqueue(nextNode);
                if (currentNode.spell != nextNode.previous.spell)
                {
                    // Debug.Log(nextNode.spell + " doesn't use " + currentNode.spell + " as previous " + nextNode.previous.spell);
                    doPreviousNodeInputs = false;
                }
            }

            // Check if the spell requires waiting
            if (currentNode.spell is IWaitableSpell waitableSpell)
            {
                isWaiting = true;
                yield return StartCoroutine(WaitForSpell(waitableSpell));
                isWaiting = false;
            }
            else
            {
                yield return new WaitForSeconds(castStepDelay);
            }
        }
    }

    void AddICastComponent(ICast spellExe, SpellNode currentNode)
    {
        // Get the type of the source component (currentNode.spell)
        Type sourceType = currentNode.spell.GetType();

        // Get the type of the target component (spellExe)
        Type targetType = spellExe.GetType();

        // Get all fields from the source component
        FieldInfo[] sourceFields = sourceType.GetFields();

        // Iterate through each field and copy its value to the corresponding field in the target component
        foreach (FieldInfo field in sourceFields)
        {
            // Find the matching field in the target component
            FieldInfo targetField = targetType.GetField(field.Name);

            // Check if both fields exist and are of compatible types
            if (targetField != null && targetField.FieldType == field.FieldType)
            {
                // Get the value from the source field
                object value = field.GetValue(currentNode.spell);

                // Set the value to the target field
                targetField.SetValue(spellExe, value);
            }
        }

        return;
    }

    private IEnumerator WaitForSpell(IWaitableSpell waitableSpell)
    {
        Debug.Log("Waiting for time for spell");
        if (waitableSpell.WaitDuration > 0)
        {
            yield return new WaitForSeconds(waitableSpell.WaitDuration);
        }
        else
        {
            yield return new WaitUntil(() => waitableSpell.IsTriggered);
        }
    }

    void ManaCostHandler(GameObject caster, float ManaCost)
    {
        if (caster.layer == LayerMask.NameToLayer("Player"))
        {
            // Caster is on the Player layer
            Debug.Log("Caster is on the Player layer.");
            caster.GetComponent<PlayerStats>().UseMana(ManaCost);
            // Perform player-specific actions here
        }
        else if (caster.layer == LayerMask.NameToLayer("Rune"))
        {
            // Caster is on the Rune layer
            Debug.Log("Caster is on the Rune layer.");
            // Perform rune-specific actions here
        }
        else
        {
            // Caster is on another layer
            Debug.Log("Caster is on an unhandled layer.");
        }
    }

}
