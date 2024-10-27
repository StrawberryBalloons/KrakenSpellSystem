using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import TextMeshPro namespace
using System.Collections.Generic;

public class TooltipMagickSubmenu : MonoBehaviour
{
    private Text tooltipText;
    private RectTransform backgroundRectTransform;
    private GameObject parentGameObject;
    private RectTransform parentObject;
    // public GameObject text;
    // public GameObject background;
    public List<GameObject> gameObjectsList;
    public float offset = 10f;
    private bool buttonToggle = true;

    private void Awake()
    {

        // backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();
        // tooltipText = transform.Find("text").GetComponent<Text>();
        parentGameObject = transform.parent.gameObject;
        HideTooltip();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            buttonToggle = !buttonToggle;
        }
        // Check if the cursor is hovering over the Tooltip object
        RectTransform rectTransform = parentGameObject.GetComponent<RectTransform>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, Camera.main, out localPoint);

        if (buttonToggle)
        {
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
    }
    public void DisplayTooltip()
    {

        if (gameObjectsList != null && gameObjectsList.Count > 0)
        {
            // Iterate through the list and set each GameObject active
            foreach (GameObject obj in gameObjectsList)
            {
                // Set the GameObject active
                obj.SetActive(true);
            }
        }

        //tooltipText.text = text;
        float textPaddingSize = 4f;

        // Vector2 backgroundSize = new Vector2(tooltipText.preferredWidth + textPaddingSize * 2,
        //  tooltipText.preferredHeight + textPaddingSize * 2);
        // // Position the tooltip slightly above the mouse position
        // backgroundRectTransform.sizeDelta = backgroundSize;
    }

    public void HideTooltip()
    {
        if (gameObjectsList != null && gameObjectsList.Count > 0)
        {
            // Iterate through the list and set each GameObject active
            foreach (GameObject obj in gameObjectsList)
            {
                // Set the GameObject active
                obj.SetActive(false);
            }
        }
    }
}
