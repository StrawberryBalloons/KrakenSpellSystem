using UnityEngine;

public class StretchLinePrefab : MonoBehaviour
{
    public GameObject linePrefab; // Assign your line prefab in the Inspector
    private GameObject currentLine;
    private Transform startPoint;
    private Transform endPoint;

    // Method to create or update the line between two points
    public void UpdateLine(Transform start, Transform end, GameObject line)
    {
        startPoint = start;
        endPoint = end;
        if (startPoint == null || endPoint == null)
        {
            RemoveLine();
            return;
        }

        if (currentLine == null)
        {
            currentLine = line;
        }

        UpdateLinePosition();
    }


    // Method to update the position and rotation of the line
    private void UpdateLinePosition()
    {
        RectTransform rectTransform = currentLine.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            Vector3 startPosition = startPoint.localPosition;
            Vector3 endPosition = endPoint.localPosition;

            Vector3 direction = endPosition - startPosition;
            float distance = direction.magnitude;

            rectTransform.localPosition = Vector3.zero;
            rectTransform.sizeDelta = new Vector2(distance, rectTransform.sizeDelta.y);
            rectTransform.pivot = new Vector2(0, 0.5f);
            rectTransform.localRotation = Quaternion.FromToRotation(Vector3.right, direction);
        }
    }
    public void LoadLine(Vector3 start, Vector3 end, GameObject line)
    {
        if (start == null || end == null)
        {
            RemoveLine();
            return;
        }

        if (currentLine == null)
        {
            currentLine = line;
        }

        LoadLinePosition(start, end);
    }

    private void LoadLinePosition(Vector3 startPosition, Vector3 endPosition)
    {
        RectTransform rectTransform = currentLine.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            Vector3 direction = endPosition - startPosition;
            float distance = direction.magnitude;

            rectTransform.localPosition = Vector3.zero;
            rectTransform.sizeDelta = new Vector2(distance, rectTransform.sizeDelta.y);
            rectTransform.pivot = new Vector2(0, 0.5f);
            rectTransform.localRotation = Quaternion.FromToRotation(Vector3.right, direction);
        }
    }

    // Method to remove the line if one of the points is missing
    private void RemoveLine()
    {
        if (currentLine != null)
        {
            Destroy(currentLine);
            currentLine = null;
        }
    }

    // Example Update method (can be called from another script's Update)
    void Update()
    {
        // Example: Replace these with your actual Transform variables
        // Transform startPoint = GameObject.Find("StartPoint").transform;
        // Transform endPoint = GameObject.Find("EndPoint").transform;

        UpdateLine(startPoint, endPoint, currentLine);
    }
}
