using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{
    [Header("Main Menu Scene")]
    public string mainMenuScene = "MainMenu";

    // Call this from a UI Button
    public void ReturnMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}