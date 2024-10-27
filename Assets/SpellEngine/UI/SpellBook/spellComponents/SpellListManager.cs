using System.Collections.Generic;
using UnityEngine;

public class SpellListManager : MonoBehaviour
{
    public string name = "empty";
    public KeyCode key; // KeyCode to trigger the spell
    public SpellNode head; // Head of the linked list of SpellNodes
    public List<SpellNode> nodelist;

    public void AddSpell(SpellNode start)
    {
        head = start;
    }

    public void RemoveSpell()
    {
        head = null;
    }

    // Deep copy method
    public SpellListManager DeepCopy()
    {
        SpellListManager copy = new SpellListManager
        {
            name = this.name,
            key = this.key
        };

        // Deep copy of the linked list of SpellNodes
        if (head != null)
        {
            copy.head = DeepCopyNode(head, new Dictionary<SpellNode, SpellNode>());
        }

        return copy;
    }

    private SpellNode DeepCopyNode(SpellNode originalNode, Dictionary<SpellNode, SpellNode> copiedNodes)
    {
        // Check if this node has already been copied to avoid circular references
        if (copiedNodes.TryGetValue(originalNode, out SpellNode copiedNode))
        {
            return copiedNode; // Return the already copied instance
        }

        // Create a new SpellNode instance
        copiedNode = new SpellNode(originalNode.spell) // Ensure to copy the spell if it's mutable
        {
            position = originalNode.position,
            parameters = new List<object>(originalNode.parameters), // Shallow copy of parameters
            affectedObjects = new List<GameObject>(originalNode.affectedObjects), // Shallow copy of affected objects
            affectedParameters = new List<object>(originalNode.affectedParameters) // Shallow copy of affected parameters
        };

        // Store the copy in the dictionary
        copiedNodes[originalNode] = copiedNode;

        // Recursively copy the next nodes
        foreach (var nextNode in originalNode.next)
        {
            SpellNode nextCopiedNode = DeepCopyNode(nextNode, copiedNodes);
            copiedNode.next.Add(nextCopiedNode);
            nextCopiedNode.previous = copiedNode; // Ensure the previous reference points to the copied node
        }

        return copiedNode;
    }

    // Other methods for editing the spell list as needed
}
