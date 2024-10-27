using System.Collections.Generic;
using UnityEngine;

public class OrderNodesByDistance : MonoBehaviour
{
    public bool doOrNot = false;

    void FixedUpdate()
    {
        if (doOrNot)
        {
            OrderNodes();
        }
    }
    public void toggle()
    {
        doOrNot = !doOrNot;
    }

    void OrderNodes()
    {
        // Get all child nodes
        List<SpellNodeSlot> nodeSlots = new List<SpellNodeSlot>(GetComponentsInChildren<SpellNodeSlot>());
        if (nodeSlots.Count < 2)
            return;

        // Debug.Log("Ordering nodes");

        // Find the starting node (the node without a previous node)
        SpellNodeSlot startNodeSlot = nodeSlots.Find(slot => slot.node.previous == null);
        if (startNodeSlot == null)
            return;

        // Traverse the linked list and create a list of nodes with their distances to the previous node
        List<NodeDistance> nodeDistances = new List<NodeDistance>();
        SpellNode currentNode = startNodeSlot.node;

        // Debug.Log("NodeCount: " + currentNode.next.Count);
        while (currentNode.next.Count > 0)
        {
            foreach (SpellNode nextNode in currentNode.next)
            {
                float distance = Vector3.Distance(currentNode.position, nextNode.position);
                nodeDistances.Add(new NodeDistance { nodeSlot = GetNodeSlot(nextNode, nodeSlots), distance = distance });
                currentNode = nextNode;
            }
        }

        // Sort nodes based on distance
        nodeDistances.Sort((a, b) => a.distance.CompareTo(b.distance));

        // Reorder nodes in the hierarchy
        for (int i = 0; i < nodeDistances.Count; i++)
        {
            nodeDistances[i].nodeSlot.transform.SetSiblingIndex(i + 1); // +1 because the first node remains in place
        }
    }

    private SpellNodeSlot GetNodeSlot(SpellNode node, List<SpellNodeSlot> nodeSlots)
    {
        return nodeSlots.Find(slot => slot.node == node);
    }

    private class NodeDistance
    {
        public SpellNodeSlot nodeSlot;
        public float distance;
    }
}
