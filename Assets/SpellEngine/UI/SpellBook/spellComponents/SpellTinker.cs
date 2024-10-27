using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellTinker : MonoBehaviour, IDropHandler
{
    public GameObject tinkerObject;
    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("OnDrop");

        // Check if the dragged object has a DraggableItem component
        DraggableItem draggableItem = eventData.pointerDrag.GetComponent<DraggableItem>();
        if (draggableItem != null)
        {
            Transform prefabPreExist = eventData.pointerDrag.transform.Find("ParamPrefab");
            if (!prefabPreExist.gameObject.activeSelf)
            {

                if (tinkerObject != null)
                {
                    Destroy(tinkerObject);
                }
                tinkerObject = Instantiate(eventData.pointerDrag, transform);

                RectTransform rectTransform = tinkerObject.GetComponent<RectTransform>();
                RectTransform firstRect = eventData.pointerDrag.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = firstRect.anchoredPosition;

                Image newImage = tinkerObject.GetComponent<Image>();
                if (newImage != null)
                {
                    Color color = newImage.color;
                    color.a = 1f; // Set alpha to max
                    newImage.color = color;
                    newImage.raycastTarget = true; // Enable raycastTarget
                }

                Transform childTransform = tinkerObject.transform.Find("ParamPrefab");
                if (childTransform != null)
                {
                    childTransform.gameObject.SetActive(true);
                }
                else
                {
                    Debug.LogError("'ParamPrefab' not found.");
                }

            }
        }
        Debug.Log("Clone Detected");
    }
}
