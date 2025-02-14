using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import TextMeshPro namespace

public class Tooltip : MonoBehaviour
{
    private Text tooltipText;
    private RectTransform backgroundRectTransform;
    private RectTransform rarityRectTransform;
    public RectTransform parentObject;
    public GameObject text;
    public GameObject background;
    public GameObject rarity;
    public float textPaddingSize = 10f;

    private int originalSiblingIndex; // Store the original sibling index
    private Transform originalParent; // Store the original parent
    private LayoutElement layoutElement; // Used to bypass GridLayoutGroup
    public bool forceHide = false;

    private void Awake()
    {
        backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();
        if (rarity != null)
        {
            rarityRectTransform = transform.Find("rarity").GetComponent<RectTransform>();
        }
        tooltipText = transform.Find("text").GetComponent<Text>();
        parentObject = transform.parent.GetComponent<RectTransform>();

        originalParent = transform.parent; // Store the initial parent
        originalSiblingIndex = transform.GetSiblingIndex(); // Store the initial index

        // Ensure LayoutElement exists
        layoutElement = GetComponent<LayoutElement>();
        if (layoutElement == null)
        {
            layoutElement = gameObject.AddComponent<LayoutElement>();
        }
        layoutElement.ignoreLayout = true; // Prevent layout interference

        HideTooltip();
    }


    private void FixedUpdate()
    {
        // Check if the cursor is hovering over the Tooltip object
        RectTransform rectTransform = parentObject;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, Camera.main, out localPoint);

        if (rectTransform.rect.Contains(localPoint) && tooltipText.text != "")
        {
            DisplayTooltip();
        }
        else
        {
            HideTooltip();
        }
    }

    public void DisplayTooltip()
    {
        text.SetActive(true);
        background.SetActive(true);
        if (rarity != null)
        {
            rarity.SetActive(true);
        }

        originalSiblingIndex = transform.GetSiblingIndex();

        // Store current world position
        Vector3 worldPosition = transform.position;

        // Move the tooltip object to be a child of its grandparent
        if (parentObject.parent != null)
        {
            transform.SetParent(parentObject.parent, true);
            transform.SetAsLastSibling();
        }

        // Restore world position to keep it visually the same
        transform.position = worldPosition;

        // Disable LayoutElement to prevent GridLayoutGroup interference
        if (layoutElement != null)
        {
            layoutElement.ignoreLayout = true;
        }

        Vector2 backgroundSize = new Vector2(
            tooltipText.preferredWidth + textPaddingSize * 3f,
            tooltipText.preferredHeight + textPaddingSize * 2
        );

        backgroundRectTransform.sizeDelta = backgroundSize;
        if (rarityRectTransform != null && rarity != null)
        {
            rarityRectTransform.sizeDelta = new Vector2(
    backgroundSize.x * 1.012f,
    backgroundSize.y * 1.05f
);
        }
    }

    public void HideTooltip()
    {
        text.SetActive(false);
        background.SetActive(false);
        if (rarity != null)
        {
            rarity.SetActive(false);
        }

        // Store current world position before restoring parent
        Vector3 worldPosition = transform.position;

        // Restore the original parent and sibling index
        transform.SetParent(originalParent, true);
        transform.SetSiblingIndex(originalSiblingIndex);

        // Restore world position to keep it visually the same
        transform.position = worldPosition;

        // Re-enable LayoutElement
        if (layoutElement != null)
        {
            layoutElement.ignoreLayout = false;
        }
    }
}
