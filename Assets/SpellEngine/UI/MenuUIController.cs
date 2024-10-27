using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUIController : MonoBehaviour
{
    // Button references
    public Button settingsButton;
    public Button exitButton;
    public Button restartButton;
    public Button helpButton;
    public GameObject menu;

    public GameObject help;
    public GameObject settings;
    public void OpenSettings()
    {
        // Open the settings menu
        Debug.Log("Settings menu opened.");
        help.SetActive(false);
        menu.SetActive(false);
        settings.SetActive(true);
        // Implement your settings menu logic here
    }

    public void ExitGame()
    {
        // Load the StartScreen scene
        Debug.Log("Loading StartScreen...");
        SceneManager.LoadScene("StartScreen");
    }

    public void RestartGame()
    {
        // Restart the game by reloading the current scene
        Debug.Log("Restarting game...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenHelp()
    {
        // Open the help menu
        Debug.Log("Help menu opened.");
        help.SetActive(true);
        menu.SetActive(false);
        settings.SetActive(false);
        // Implement your help menu logic here
    }
}
