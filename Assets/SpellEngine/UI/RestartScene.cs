using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    // This method can be called to restart the current scene
    public void RestartCurrentScene()
    {
        // Get the name of the current active scene
        string sceneName = SceneManager.GetActiveScene().name;

        // Load the scene with the retrieved name
        SceneManager.LoadScene(sceneName);
    }
}
