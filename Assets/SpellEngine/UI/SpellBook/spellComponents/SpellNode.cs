using System.Collections.Generic;
using UnityEngine;

public class SpellNode
{
    public ICast spell; // The spell action associated with this node
    public Vector3 position; // The position of the node in the UI
    public SpellNode previous; // Reference to the previous node in the linked list
    public List<SpellNode> next; // List of references to the next nodes in the linked list
    public List<object> parameters; // Parameters to apply to the spell

    // State moved to the node
    public List<GameObject> affectedObjects = new List<GameObject>(); // Affected objects after cast
    public List<object> affectedParameters = new List<object>(); // Affected parameters after cast

    public SpellNode(ICast spell)
    {
        this.spell = spell;
        this.position = new Vector3();
        this.previous = null;
        this.next = new List<SpellNode>();
        this.parameters = new List<object>();
    }
}
