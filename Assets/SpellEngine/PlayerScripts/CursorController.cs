using UnityEngine;

public class CursorController : MonoBehaviour
{
    private bool isCursorLocked = true;

    void Start()
    {
        // Lock the cursor at the start
        LockCursor();
    }

    void Update()
    {
        // Toggle cursor lock state when the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
        {
            if (isCursorLocked)
            {
                UnlockCursor();
            }
            else
            {
                LockCursor();
            }
        }
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isCursorLocked = true;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isCursorLocked = false;
    }
}
