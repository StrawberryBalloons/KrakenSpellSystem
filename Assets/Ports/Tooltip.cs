using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import TextMeshPro namespace

public class Tooltip : MonoBehaviour
{
    private Text tooltipText;
    private RectTransform backgroundRectTransform;
    public RectTransform parentObject;
    public GameObject text;
    public GameObject background;
    public float textPaddingSize = 10f;


    private void Awake()
    {

        backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();
        tooltipText = transform.Find("text").GetComponent<Text>();
        parentObject = transform.parent.GetComponent<RectTransform>();
        HideTooltip();
    }
    private void Update()
    {
        // Check if the cursor is hovering over the Tooltip object
        RectTransform rectTransform = parentObject;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, Camera.main, out localPoint);

        if (rectTransform.rect.Contains(localPoint))
        {
            // If the cursor is hovering, display the tooltip
            DisplayTooltip();
        }
        else
        {
            // If not, hide the tooltip
            HideTooltip();
        }
    }
    public void DisplayTooltip()
    {
        text.SetActive(true);
        background.SetActive(true);


        //tooltipText.text = text;

        Vector2 backgroundSize = new Vector2(tooltipText.preferredWidth + textPaddingSize * 3f,
         tooltipText.preferredHeight + textPaddingSize * 2);
        // Position the tooltip slightly above the mouse position
        backgroundRectTransform.sizeDelta = backgroundSize;
    }

    public void HideTooltip()
    {
        text.SetActive(false);
        background.SetActive(false);
    }
}
