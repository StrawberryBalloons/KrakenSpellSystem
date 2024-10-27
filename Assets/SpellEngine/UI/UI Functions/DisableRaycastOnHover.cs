using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DisableRaycastOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image spellGridImage;

    void Start()
    {
        // Get the Image component of the parent GameObject
        spellGridImage = transform.parent.GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Disable the Raycast Target property of the SpellGrid GameObject's Image component
        spellGridImage.raycastTarget = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Enable the Raycast Target property of the SpellGrid GameObject's Image component
        spellGridImage.raycastTarget = true;
    }
    public void enableRaycastTarget()
    {
        spellGridImage.raycastTarget = true;
    }
}
