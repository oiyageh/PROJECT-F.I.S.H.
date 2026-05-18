using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Scenes")]
    public string firstLevelScene = "Game";
    public string mainMenuScene = "MainMenu";

    [Header("Save Keys")]
    public static string continueSceneKey = "SavedScene";
    public static string playerPosXKey = "PlayerPosX";
    public static string playerPosYKey = "PlayerPosY";
    public static string playerPosZKey = "PlayerPosZ";

    // =========================
    // PLAY BUTTON
    // =========================
    public void PlayGame()
    {
        // Delete old save
        DeleteSave();

        SceneManager.LoadScene(firstLevelScene);
    }

    // =========================
    // CONTINUE BUTTON
    // =========================
    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey(continueSceneKey))
        {
            string savedScene = PlayerPrefs.GetString(continueSceneKey);
            SceneManager.LoadScene(savedScene);
        }
        else
        {
            Debug.Log("No save found.");
        }
    }

    // =========================
    // QUIT BUTTON
    // =========================
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    // =========================
    // SAVE GAME
    // =========================
    public static void SaveGame(Transform player)
    {
        // Save Scene
        string currentScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString(continueSceneKey, currentScene);

        // Save Position
        PlayerPrefs.SetFloat(playerPosXKey, player.position.x);
        PlayerPrefs.SetFloat(playerPosYKey, player.position.y);
        PlayerPrefs.SetFloat(playerPosZKey, player.position.z);

        PlayerPrefs.Save();

        Debug.Log("Game Saved");
    }

    // =========================
    // LOAD PLAYER POSITION
    // =========================
    public static void LoadPlayerPosition(Transform player)
    {
        if (PlayerPrefs.HasKey(playerPosXKey))
        {
            float x = PlayerPrefs.GetFloat(playerPosXKey);
            float y = PlayerPrefs.GetFloat(playerPosYKey);
            float z = PlayerPrefs.GetFloat(playerPosZKey);

            player.position = new Vector3(x, y, z);

            Debug.Log("Player Position Loaded");
        }
    }

    // =========================
    // DELETE SAVE
    // =========================
    public static void DeleteSave()
    {
        PlayerPrefs.DeleteKey(continueSceneKey);

        PlayerPrefs.DeleteKey(playerPosXKey);
        PlayerPrefs.DeleteKey(playerPosYKey);
        PlayerPrefs.DeleteKey(playerPosZKey);

        PlayerPrefs.Save();

        Debug.Log("Save Deleted");
    }
}