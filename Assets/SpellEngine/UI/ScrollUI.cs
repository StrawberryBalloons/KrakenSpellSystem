using UnityEngine;

public class ScrollUI : MonoBehaviour
{
    public RectTransform targetUI; // Assign the UI object to move
    public float scrollSpeed = 50f; // Adjust scrolling speed
    public float minY = 0f; // Minimum Y position
    public float maxY = 500f; // Maximum Y position

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            Vector3 newPosition = targetUI.anchoredPosition;
            newPosition.y += scroll * scrollSpeed * -1f; // Invert scroll direction if needed
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY); // Limit movement
            targetUI.anchoredPosition = newPosition;
        }
    }
}
