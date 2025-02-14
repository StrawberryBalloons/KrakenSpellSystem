using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public Text tooltipText;  // Assign from UI (TextMeshPro)
    public RectTransform backgroundRectTransform;
    public GameObject textObject;
    public GameObject backgroundObject;
    public CharacterActions charActions;
    public float maxRayDistance = 20f;
    public float textPaddingSize = 10f;

    private void Start()
    {
        HideTooltip();
    }

    private void Update()
    {
        // Check if cursor is active
        if (!Cursor.visible)
        {
            HideTooltip();
            return;
        }

        // Only raycast when right mouse button is held
        if (Input.GetMouseButton(1))
        {
            Ray ray = charActions.GetActiveCamera().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxRayDistance))
            {
                ShowTooltip(hit.collider.gameObject.name);
            }
            else
            {
                HideTooltip();
            }
        }
        else
        {
            HideTooltip();
        }
    }

    private void ShowTooltip(string tooltipContent)
    {
        textObject.SetActive(true);
        backgroundObject.SetActive(true);
        tooltipText.text = tooltipContent;

        // Adjust background size based on text
        Vector2 backgroundSize = new Vector2(tooltipText.preferredWidth + textPaddingSize * 3f,
         tooltipText.preferredHeight + textPaddingSize * 2);
        // Position the tooltip slightly above the mouse position
        backgroundRectTransform.sizeDelta = backgroundSize;
    }

    private void HideTooltip()
    {
        textObject.SetActive(false);
        backgroundObject.SetActive(false);
    }
}
