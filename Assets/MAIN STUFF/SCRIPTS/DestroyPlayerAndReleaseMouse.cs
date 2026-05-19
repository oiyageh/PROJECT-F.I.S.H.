using UnityEngine;

public class DestroyPlayerAndReleaseMouse : MonoBehaviour
{
    [Header("Optional (auto-detects if null)")]
    public GameObject player;

    [Header("Game Over Settings")]
    public bool pauseGame = true;
    public bool unlockCursor = true;

    private bool hasTriggered = false;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            TriggerGameOver(other.gameObject);
        }
    }

    private void TriggerGameOver(GameObject playerObj)
    {
        hasTriggered = true;

        // Destroy player
        Destroy(playerObj);

        // Unlock mouse
        if (unlockCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Pause game
        if (pauseGame)
        {
            Time.timeScale = 0f;
        }
    }
}