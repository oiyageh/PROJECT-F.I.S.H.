using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject pausePanel;

    [Header("References")]
    public ThirdPersonController player;

    private bool isPaused;

    void Start()
    {
        pausePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        isPaused = true;

        pausePanel.SetActive(true);
        Time.timeScale = 0.0001f;

        AudioListener.pause = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        player.SetPaused(true);
    }

    public void Resume()
    {
        isPaused = false;

        pausePanel.SetActive(false);
        Time.timeScale = 1f;

        AudioListener.pause = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        player.SetPaused(false);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene("MainMenu");
    }
    public void SetMouseSensitivity(float value)
    {
        player.mouseSensitivity = value;
    }
}