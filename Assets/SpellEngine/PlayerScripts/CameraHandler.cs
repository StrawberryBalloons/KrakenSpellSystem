using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    // List of cameras to manage
    public List<Camera> cameras;

    // Index to track the current active camera
    public int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize all cameras, setting only the currentIndex camera to active
        SetActiveCamera(currentIndex);
    }

    // Function to increase the index and set the corresponding camera as active
    public void NextCamera()
    {
        if (cameras.Count == 0)
        {
            Debug.LogWarning("Camera list is empty!");
            return;
        }

        // Increase the index
        currentIndex++;

        // Loop the index if it exceeds the number of cameras
        if (currentIndex >= cameras.Count)
        {
            currentIndex = 0;
        }

        // Set the active camera
        SetActiveCamera(currentIndex);
    }

    // Function to set a specific camera as active based on index
    private void SetActiveCamera(int index)
    {
        // Disable all cameras
        for (int i = 0; i < cameras.Count; i++)
        {
            if (cameras[i] != null)
            {
                cameras[i].gameObject.SetActive(false);
            }
        }

        // Enable the camera at the current index
        if (cameras[index] != null)
        {
            cameras[index].gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"Camera at index {index} is null!");
        }
    }
}
