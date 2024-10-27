using UnityEngine;
using UnityEngine.UI;

public class ScrollRectGrid : MonoBehaviour
{
    // Reference to the ScrollRect component
    private ScrollRect scrollRect;

    // Start is called before the first frame update
    void Start()
    {
        // Get the ScrollRect component attached to this GameObject
        scrollRect = GetComponent<ScrollRect>();
    }

    // Update is called once per frame
    void Update()
    {
        // Example: Scroll the ScrollRect vertically with arrow keys
        float scrollInput = Input.GetAxis("Vertical");
        scrollRect.verticalNormalizedPosition += scrollInput * Time.deltaTime;
        scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollRect.verticalNormalizedPosition);
    }
}
