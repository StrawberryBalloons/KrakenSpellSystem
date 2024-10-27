
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SpellNodeSlot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    public SpellNode node;
    public Vector2 originalPosition;
    public GameObject linePrefab; // Prefab with an Image component to use as the line




    public void InitializeNode(ICast spell, Vector3 position, List<object> parameters)
    {
        node = new SpellNode(spell);
        node.position = position;
        transform.position = position;
        node.parameters = parameters;
    }

    public void SetNextNode(SpellNodeSlot nextNodeSlot)
    {
        if (node.previous != nextNodeSlot.node)
        {
            // Setup next node connection
            node.next.Add(nextNodeSlot.node);
            nextNodeSlot.node.previous = node;
            Debug.Log("SetPreviousNode to: " + node.spell);

            // Instantiate a new line
            // Debug.Log(node.position + " " + nextNodeSlot.node.position);
            //LineDrawer.Instance.DrawLine(node.position, nextNodeSlot.node.position);
            LoadNextNode(nextNodeSlot);
        }
    }

    public void LoadNextNode(SpellNodeSlot nextNodeSlot)
    {
        GameObject lineObject = Instantiate(linePrefab, transform);

        StretchLinePrefab lineInstance = lineObject.GetComponent<StretchLinePrefab>();
        RectTransform lineRectTransform = lineObject.GetComponent<RectTransform>();
        // lineObject.transform.parent = transform; // Make it a child of this GameObject

        Vector2 direction = nextNodeSlot.GetComponent<RectTransform>().localPosition - GetComponent<RectTransform>().localPosition;
        float distance = direction.magnitude;
        // Debug.Log("Position, distance" + node.position + nextNodeSlot.node.position + distance);

        // transform.GetComponent<RectTransform>().InverseTransformPoint(worldPoint)

        lineRectTransform.sizeDelta = new Vector2(distance, lineRectTransform.sizeDelta.y);
        lineRectTransform.pivot = new Vector2(0, 0.5f);
        lineRectTransform.position = node.position;
        lineRectTransform.rotation = Quaternion.FromToRotation(Vector3.right, direction);

        // Assign the linePrefab to the instance
        lineInstance.UpdateLine(transform, nextNodeSlot.transform, lineObject);
    }

    public void Remove()
    {
        GetComponent<DisableRaycastOnHover>().enableRaycastTarget();

        Destroy(gameObject);
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Debug.Log("OnDrop - spellnodeslot");

        // Check if the dragged object has a DraggableItem component
        SpellNodeSlot nodeItem = eventData.pointerDrag.GetComponent<SpellNodeSlot>();
        if (nodeItem != null)
        {
            // Debug.Log("node item found");
            MoveSpellNode move = eventData.pointerDrag.GetComponent<MoveSpellNode>();
            move.ResetPosition();

            // Update node.position to the current RectTransform position
            // RectTransform rectTransform = GetComponent<RectTransform>();
            // Vector3 currentPosition = rectTransform.localPosition;
            // node.position = currentPosition;

            nodeItem.SetNextNode(this);
        }
    }
}
//one to many change

