
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;

    public ICast castAction;
    public GameObject spellNodeSlotPrefab; // Reference to the SpellNodeSlot prefab
    public GameObject parameterPrefab; // UI element prefab that stores pre-configured parameters

    [SerializeField]
    private RectTransform panelRectTransform;
    private Vector2 initialPanelPosition;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private GridLayoutGroup gridLayoutGroup;
    [HideInInspector] public Transform parentAfterDrag;

    private Transform originalParent;
    private int originalSiblingIndex;
    private Transform higherParent;


    private void Awake()
    {
        castAction = GetComponent<ICast>(); // Get the ICast component attached to the same GameObject
        gridLayoutGroup = GetComponentInParent<GridLayoutGroup>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>(); // Add CanvasGroup for better control
    }

    void Start()
    {
        panelRectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        TriggerTinkerButton triggerTinkerButton = GetComponent<TriggerTinkerButton>();

        if (triggerTinkerButton != null)
        {
            // Successfully found the component, you can now interact with it
            triggerTinkerButton.TriggerTinker(); // Replace SomeMethod with your method
        }
        else
        {
            // Component not found
            Debug.LogError("TriggerTinkerButton component not found!");
        }

        initialPanelPosition = panelRectTransform.anchoredPosition;
        image.raycastTarget = false;
        ChangeImageAlpha(.6f);

        // Save the original parent and sibling index
        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();

        // Move the item two levels up in the hierarchy and set as the last child
        higherParent = originalParent.parent?.parent;
        if (higherParent != null)
        {
            transform.SetParent(higherParent);
            transform.SetAsLastSibling();
        }

        // Disable GridLayoutGroup on original parent
        if (gridLayoutGroup != null)
        {
            gridLayoutGroup.enabled = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        panelRectTransform.anchoredPosition += eventData.delta;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform, eventData.position, eventData.pressEventCamera, out localPoint);
        panelRectTransform.localPosition = localPoint - (Vector2)canvas.transform.localPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        ChangeImageAlpha(1f);
        panelRectTransform.anchoredPosition = initialPanelPosition;

        // Return the item to its original parent and sibling index
        transform.SetParent(originalParent);
        transform.SetSiblingIndex(originalSiblingIndex);

        // Re-enable GridLayoutGroup on original parent
        if (gridLayoutGroup != null)
        {
            gridLayoutGroup.enabled = true;
        }
    }

    private void ChangeImageAlpha(float newAlpha)
    {
        Color newColor = image.color;
        newColor.a = newAlpha;
        image.color = newColor;
    }
}
