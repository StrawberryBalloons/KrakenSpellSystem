using UnityEngine;
using Unity.Netcode;
public class UIController : MonoBehaviour
{

    public KeyCode spellBookKey = KeyCode.Space;
    public KeyCode menuKey = KeyCode.Space;
    public GameObject spellBook;
    public GameObject hud;
    public GameObject menu;
    public GameObject help;
    public GameObject settings;
    private bool isCursorHidden = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isCursorHidden = true;
    }
    void Update()
    {
        if (Input.GetKeyDown(spellBookKey))
        {
            ToggleSpellbook();
        }
        if (Input.GetKeyDown(menuKey))
        {
            ToggleMenu();
        }
        HandleCursorVisibility();
    }

    void ToggleSpellbook()
    {
        if (spellBook != null && hud != null)
        {
            // Toggle the active state of the target object
            spellBook.SetActive(!spellBook.activeSelf);
            hud.SetActive(!hud.activeSelf);
        }
        else
        {
            Debug.LogWarning("object not assigned!");
        }
        HandleCursorVisibility();
    }

    void ToggleMenu()
    {
        if (menu != null)
        {
            // Toggle the active state of the target object
            spellBook.SetActive(false);
            settings.SetActive(false);
            help.SetActive(false);
            hud.SetActive(true);
            menu.SetActive(!menu.activeSelf);
        }
        else
        {
            Debug.LogWarning("object not assigned!");
        }
    }
    void HandleCursorVisibility()
    {
        if (Input.GetKeyDown(spellBookKey) || Input.GetKeyDown(menuKey))
        {
            if (isCursorHidden)
            {
                RevealCursor();
            }
        }
        if (isCursorHidden == false && (!help.activeSelf && !menu.activeSelf && !spellBook.activeSelf && !settings.activeSelf))
        {
            HideCursor();
        }
    }
    void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the game window
        Cursor.visible = false; // Hide the cursor
        isCursorHidden = true;
    }
    void RevealCursor()
    {
        Cursor.lockState = CursorLockMode.None; // Release the cursor from the game window
        Cursor.visible = true; // Show the cursor
        isCursorHidden = false;
    }
}
